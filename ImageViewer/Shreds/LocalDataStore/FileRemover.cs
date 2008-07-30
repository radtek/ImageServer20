#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
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

using System.IO;
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Dicom.DataStore;

namespace ClearCanvas.ImageViewer.Shreds.LocalDataStore
{
	public class FileRemover
	{
		private readonly string _topLevelDirectory;

		public FileRemover(string topLevelDirectory)
		{
			Platform.CheckForEmptyString(topLevelDirectory, "topLevelDirectory");
			_topLevelDirectory = System.IO.Path.GetDirectoryName(topLevelDirectory);
		}

		public void DeleteFilesInStudy(IStudy studyToRemove)
		{
			DeleteSopInstanceFiles(studyToRemove.GetSopInstances());
		}

		public void DeleteFilesInSeries(ISeries seriesToRemove)
		{
			DeleteSopInstanceFiles(seriesToRemove.GetSopInstances());
		}

		public void DeleteFileForSopInstance(ISopInstance sopIntanceToDelete)
		{
			if (sopIntanceToDelete.GetLocationUri().IsFile == false)
				return;

			string fileName = sopIntanceToDelete.GetLocationUri().LocalDiskPath;
			if (File.Exists(fileName))
				File.Delete(fileName);
		}

		private void DeleteSopInstanceFiles(IEnumerable<ISopInstance> sopInstancesToDelete)
		{
			List<string> directoriesToDelete = new List<string>();
			foreach (ISopInstance sop in sopInstancesToDelete)
			{
				DeleteFileForSopInstance(sop);

				string directoryName = System.IO.Path.GetDirectoryName(sop.GetLocationUri().LocalDiskPath);
				if (directoriesToDelete.Contains(directoryName) == false)
					directoriesToDelete.Add(directoryName);
			}

			// Recursively delete directories that may be empty
			DeleteEmptyDirectories(directoriesToDelete);
		}

		private void DeleteEmptyDirectories(List<string> directoriesToDelete)
		{
			if (directoriesToDelete.Count == 0)
				return;

			// Subdirectories will always be longer than parent directories
			// sort in descending order based on directory length
			directoriesToDelete.Sort();
			directoriesToDelete.Reverse();

			List<string> parentDirectoriesToDelete = new List<string>();
			foreach (string directoryName in directoriesToDelete)
			{
				if (Directory.Exists(directoryName))
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
					if (directoryInfo.GetFiles("*", SearchOption.AllDirectories).Length <= 0)
					{
						if (directoryInfo.FullName != _topLevelDirectory)
						{
							Directory.Delete(directoryName, true);
							parentDirectoriesToDelete.Add(directoryInfo.Parent.FullName);
						}
					}
				}
			}

			DeleteEmptyDirectories(parentDirectoriesToDelete);
		}
	}
}