#region License

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
using ClearCanvas.ImageServer.Enterprise;
using ClearCanvas.ImageServer.Model;

namespace ClearCanvas.ImageServer.Common
{
	/// <summary>
	/// A cache of <see cref="StudyStorageLocation"/> objects.
	/// </summary>
	public class StorageLocationCache : IDisposable
	{
		#region Private Members
		private readonly Dictionary<ServerEntityKey, ServerCache<string, StudyStorageLocation>> _caches = new Dictionary<ServerEntityKey, ServerCache<string, StudyStorageLocation>>();
		private readonly object _lock = new object();
		#endregion

		#region Public Methods
		public StudyStorageLocation GetCachedStudy(ServerEntityKey partitionKey, string studyUid)
		{
			ServerCache<string,StudyStorageLocation> partitionCache;
			lock (_lock)
			{
				if (!_caches.TryGetValue(partitionKey, out partitionCache))
					return null;
			}

			return partitionCache.GetValue(studyUid);
		}

		public void AddCachedStudy(StudyStorageLocation theLocation)
		{
			ServerCache<string, StudyStorageLocation> partitionCache;
			lock (_lock)
			{
				if (!_caches.TryGetValue(theLocation.ServerPartitionKey, out partitionCache))
				{
					partitionCache = new ServerCache<string, StudyStorageLocation>(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
					_caches.Add(theLocation.ServerPartitionKey,partitionCache);
				}
			}

			partitionCache.Add(theLocation.StudyInstanceUid, theLocation);
		}
		#endregion

		#region IDisposable Implementation
		public void Dispose()
		{
			foreach (ServerCache<string, StudyStorageLocation> partitionCache in _caches.Values)
				partitionCache.Dispose();
		}
		#endregion
	}
}
