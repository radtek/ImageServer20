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
using ClearCanvas.ImageServer.Common;
using ClearCanvas.ImageServer.Common.CommandProcessor;
using ClearCanvas.ImageServer.Model;

namespace ClearCanvas.ImageServer.Services.WorkQueue
{
    /// <summary>
    /// Represents the execution context where the WorkQueue item processor is run.
    /// </summary>
    public class WorkQueueProcessorContext : ExecutionContext
    {
        #region Private Fields
        private readonly Model.WorkQueue _item;
        #endregion

        public WorkQueueProcessorContext(Model.WorkQueue item)
            :base(item.GetKey().Key.ToString())
        {
            _item = item;
            
        }

        protected override string GetTemporaryPath()
        {
                IList<StudyStorageLocation> storages =
                    StudyStorageLocation.FindStorageLocations(StudyStorage.Load(_item.StudyStorageKey));

                if (storages == null || storages.Count == 0)
                {
                    // ???
                    return base.TempDirectory;
                }

                ServerFilesystemInfo filesystem = FilesystemMonitor.Instance.GetFilesystemInfo(storages[0].FilesystemKey);
                if (filesystem == null)
                {
                    // not ready?
                    return base.TempDirectory;
                }
                else
                {
                    string basePath = GetTempPathRoot();

                    if (String.IsNullOrEmpty(basePath))
                    {
                        basePath = Path.Combine(filesystem.Filesystem.FilesystemPath, "temp");
                    }
                    
                    String tempDirectory = Path.Combine(basePath, String.Format("{0}-{1}",_item.WorkQueueTypeEnum.Lookup, _item.GetKey()));

                    for (int i = 2; i < 1000; i++)
                    {
                        if (!Directory.Exists(tempDirectory))
                        {
                            break;
                        }

                        tempDirectory = Path.Combine(basePath, String.Format("{0}-{1}({2})",
                                _item.WorkQueueTypeEnum.Lookup, _item.GetKey(), i));
                    }

                    if (!Directory.Exists(tempDirectory))
                        Directory.CreateDirectory(tempDirectory);

                    return tempDirectory;
                }
            }
    }
}