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
using System.IO;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Ris.Application.Common;

namespace ClearCanvas.Ris.Client
{
	/// <summary>
	/// Provides methods for working with attached documents stored on the RIS server.
	/// </summary>
	public static class AttachedDocument
	{
		/// <summary>
		/// Downloads a document at a specified relativeUrl to a temporary file.
		/// </summary>
		/// <param name="documentSummary"></param>
		/// <returns>The location of the downloaded file.</returns>
		public static string DownloadFile(AttachedDocumentSummary documentSummary)
		{
			Platform.CheckForNullReference(documentSummary, "documentSummary");

			// if already cached locally, return local file name
			var tempFile = TempFileManager.Instance.GetFile(documentSummary.DocumentRef);
			if (!string.IsNullOrEmpty(tempFile))
				return tempFile;

			var ftpFileTransfer = new FtpFileTransfer(
				AttachedDocumentSettings.Default.FtpUserId,
				AttachedDocumentSettings.Default.FtpPassword,
				AttachedDocumentSettings.Default.FtpBaseUrl,
				AttachedDocumentSettings.Default.FtpPassiveMode);

			var fullUrl = new Uri(ftpFileTransfer.BaseUri, documentSummary.ContentUrl);

			var fileName = Path.GetFileName(fullUrl.LocalPath);
			var localFilePath = Path.Combine(Path.GetTempPath(), fileName);

			ftpFileTransfer.Download(new FileTransferRequest(fullUrl, localFilePath));

			return localFilePath;
		}
	}
}
