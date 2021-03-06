﻿#region License

// Copyright (c) 2009, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using ClearCanvas.Common;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.ImageServer.Common;
using ClearCanvas.ImageServer.Common.CommandProcessor;
using ClearCanvas.ImageServer.Enterprise;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.EntityBrokers;

namespace ClearCanvas.ImageServer.Services.ServiceLock.ArchiveApplicationLog
{
	/// <summary>
	/// Processor for archiving the ApplicationLog from the database.
	/// </summary>
	public class ApplicationLogArchiveItemProcessor : BaseServiceLockItemProcessor, IServiceLockItemProcessor
	{
		
		private static void UpdateFilesystemKey(Model.ServiceLock item)
		{
			IPersistentStore store = PersistentStoreRegistry.GetDefaultStore();
			using (IUpdateContext ctx = store.OpenUpdateContext(UpdateContextSyncMode.Flush))
			{
				ServiceLockUpdateColumns columns = new ServiceLockUpdateColumns();
				columns.FilesystemKey = item.FilesystemKey;

				IServiceLockEntityBroker broker = ctx.GetBroker<IServiceLockEntityBroker>();
				broker.Update(item.Key, columns);
				ctx.Commit();
			}
		}

        protected override void OnProcess(Model.ServiceLock item)
		{
			ServiceLockSettings settings = ServiceLockSettings.Default;

			ServerFilesystemInfo archiveFilesystem = null;

			if (item.FilesystemKey != null)
			{
				archiveFilesystem = FilesystemMonitor.Instance.GetFilesystemInfo(item.FilesystemKey);
				if (archiveFilesystem == null)
				{
					Platform.Log(LogLevel.Warn,"Filesystem for archiving logs is no longer valid.  Assigning new filesystem.");
					item.FilesystemKey = null;
					UpdateFilesystemKey(item);
				}
			}

			if (archiveFilesystem == null)
			{
				ServerFilesystemInfo selectedFs = null;
				foreach (ServerFilesystemInfo fs in FilesystemMonitor.Instance.GetFilesystems())
				{
					if (selectedFs == null)
						selectedFs = fs;
					else if (fs.Filesystem.FilesystemTierEnum.Enum > selectedFs.Filesystem.FilesystemTierEnum.Enum)
						selectedFs = fs; // Lower tier
					else if ((fs.Filesystem.FilesystemTierEnum.Enum == selectedFs.Filesystem.FilesystemTierEnum.Enum)
					         &&(fs.FreeBytes > selectedFs.FreeBytes))
						selectedFs = fs; // same tier
				}
				if (selectedFs == null)
				{
					Platform.Log(LogLevel.Info, "No writable filesystems for archiving logs, delaying archival.");
					UnlockServiceLock(item, true, Platform.Time.AddHours(2));
					return;
				}
				item.FilesystemKey = selectedFs.Filesystem.Key;
				archiveFilesystem = selectedFs;
				UpdateFilesystemKey(item);
				Platform.Log(LogLevel.Info, "Selecting Filesystem {0} for archiving of ApplicationLog",
				             selectedFs.Filesystem.Description);
			}
		

			DateTime scheduledTime;
			if (!archiveFilesystem.Writeable)
			{
				Platform.Log(LogLevel.Warn, "Filesystem {0} is not writeable. Unable to archive log files.", archiveFilesystem.Filesystem.Description);
				scheduledTime = Platform.Time.AddMinutes(settings.ApplicationLogRecheckDelay);
			}
			else
			{
				Platform.Log(LogLevel.Info, "Checking for logs to archive to: {0}",
				             archiveFilesystem.Filesystem.Description);
				if (!ArchiveLogs(archiveFilesystem))
					scheduledTime = Platform.Time.AddMinutes(settings.ApplicationLogRecheckDelay);
				else
				{
					DateTime tomorrow = Platform.Time.Date.AddDays(1);
					// Set for 12:01 tomorrow morning
					scheduledTime = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, 1, 0);
					Platform.Log(LogLevel.Info, "Completed archival of logs, rescheduling log archive for {0}",
								 scheduledTime.ToLongTimeString());
				}
			}

			UnlockServiceLock(item, true, scheduledTime);     
		}

		private bool ArchiveLogs(ServerFilesystemInfo archiveFs)
		{
			try
			{
				using (ExecutionContext context = new ExecutionContext())
				{
					string archivePath = Path.Combine(archiveFs.Filesystem.FilesystemPath, "ApplicationLog");

					ApplicationLogSelectCriteria criteria = new ApplicationLogSelectCriteria();
					criteria.Timestamp.SortAsc(0);
					IApplicationLogEntityBroker broker = context.ReadContext.GetBroker<IApplicationLogEntityBroker>();
					ApplicationLog firstLog = broker.FindOne(criteria);
					if (firstLog == null)
						return true;

					DateTime currentCutOffTime = firstLog.Timestamp.AddMinutes(5);

					int cachedDays = ServiceLockSettings.Default.ApplicationLogCachedDays;
					if (cachedDays < 0) cachedDays = 0;

					DateTime cutOffTime = Platform.Time.Date.AddDays(cachedDays*-1);

					if (currentCutOffTime > cutOffTime)
						return true;

					using (
						ImageServerLogWriter<ApplicationLog> writer =
							new ImageServerLogWriter<ApplicationLog>(archivePath, "ApplicationLog"))
					{
						while (currentCutOffTime < cutOffTime)
						{
							if (!ArchiveTimeRange(writer, currentCutOffTime))
							{
								writer.FlushLog();
								return false;
							}
							currentCutOffTime = currentCutOffTime.AddMinutes(5);
						}

						// Now flush the last potential 5 minutes.
						if (!ArchiveTimeRange(writer, cutOffTime))
						{
							writer.FlushLog();
							return false;
						}

						writer.FlushLog();
					}

					return true;
				}
			}
			catch (Exception e)
			{
				Platform.Log(LogLevel.Error, e, "Unexpected exception when writing log file.");
				return false;
			}
		}

		private static bool ArchiveTimeRange(ImageServerLogWriter<ApplicationLog> writer, DateTime cutOffTime)
		{

			ApplicationLogSelectCriteria criteria = new ApplicationLogSelectCriteria();
			criteria.Timestamp.LessThan(cutOffTime);
			criteria.Timestamp.SortAsc(0);

			using (ExecutionContext context = new ExecutionContext())
			{
				IApplicationLogEntityBroker broker = context.ReadContext.GetBroker<IApplicationLogEntityBroker>();

				List<ServerEntityKey> keyList = new List<ServerEntityKey>(1000);
				try
				{
					broker.Find(criteria, delegate(ApplicationLog result)
					                      	{
					                      		keyList.Add(result.Key);

					                      		if (writer.WriteLog(result, result.Timestamp))
					                      		{
					                      			// The logs been flushed, delete the log entries cached.
					                      			// Purposely use a read context here, even though we're doing
					                      			// an update, so we don't use transaction wrappers, optimization
					                      			// is more important at this point.
					                      			using (
					                      				IReadContext update =
					                      					PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
					                      			{
					                      				IApplicationLogEntityBroker updateBroker =
					                      					update.GetBroker<IApplicationLogEntityBroker>();
					                      				foreach (ServerEntityKey key in keyList)
					                      					updateBroker.Delete(key);
					                      			}
					                      			keyList = new List<ServerEntityKey>(1000);
					                      		}
					                      	});

					if (keyList.Count > 0)
					{
						// Purposely use a read context here, even though we're doing an update, so we 
						// don't have to do an explicit commit and don't use transaction wrappers.
						using (
							IReadContext update =
								PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
						{
							IApplicationLogEntityBroker updateBroker = update.GetBroker<IApplicationLogEntityBroker>();
							foreach (ServerEntityKey key in keyList)
								updateBroker.Delete(key);
						}
					}

				}
				catch (Exception e)
				{
					Platform.Log(LogLevel.Error, e, "Unexpected exception when purging log files.");
					return false;
				}

				return true;
			}
		}

	}
}