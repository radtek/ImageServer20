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
using System.IO;

namespace ClearCanvas.ImageViewer.Externals.General
{
	public class FileSetArgumentHint : IArgumentHint
	{
		private IList<FileInfo> _fileInfos;

		public FileSetArgumentHint(IEnumerable<string> filenames) : this(EnumerateFiles(filenames)) {}

		public FileSetArgumentHint(IEnumerable<FileInfo> fileInfos)
		{
			_fileInfos = new List<FileInfo>(fileInfos).AsReadOnly();
		}

		public ArgumentHintValue this[string key]
		{
			get
			{
				Converter<FileInfo, string> converter;
				switch (key)
				{
					case "FILENAME":
						converter = delegate(FileInfo fileinfo) { return fileinfo.FullName; };
						break;
					case "DIRECTORY":
						converter = delegate(FileInfo fileinfo) { return fileinfo.DirectoryName; };
						break;
					case "FILENAMEONLY":
						converter = delegate(FileInfo fileinfo) { return fileinfo.Name; };
						break;
					case "EXTENSIONONLY":
						converter = delegate(FileInfo fileinfo) { return fileinfo.Extension; };
						break;
					default:
						return ArgumentHintValue.Empty;
				}

				List<string> list = new List<string>();
				foreach (FileInfo fileInfo in _fileInfos)
				{
					string result = converter(fileInfo);
					if (!list.Contains(result))
						list.Add(result);
				}
				return new ArgumentHintValue(list.ToArray());
			}
		}

		public void Dispose() {}

		private static IEnumerable<FileInfo> EnumerateFiles(IEnumerable<string> filenames)
		{
			foreach (string filename in filenames)
				yield return new FileInfo(filename);
		}
	}
}