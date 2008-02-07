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


using System.Net;
using System.ServiceModel;
using ClearCanvas.Common;
using ClearCanvas.ImageServer.Common.Utilities;

namespace ClearCanvas.ImageServer.Web.Services.Shreds.ValidationServer
{
    /// <summary>
    /// WCF service for data validation.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ValidationService : IValidationService
    {

        #region IValidationServerService Members

        public ValidationResult CheckPath(string path)
        {

            Platform.Log(LogLevel.Debug, "ValidationService: validating {0}" , path);
            FilesystemInfo fsInfo = FilesystemUtils.GetDirectoryInfo(path);
            ValidationResult res = new ValidationResult();
            if (fsInfo.Exists)
            {
                Platform.Log(LogLevel.Debug, "ValidationService: {0} exists " , path); 
                res.Success = true;
            }
            else
            {
                Platform.Log(LogLevel.Debug, "ValidationService: {0} doesn't exist or is not accessible", path); 
                
                IPHostEntry local = Dns.GetHostEntry("");
                res.ErrorText = "The specified path is invalid OR not accessible from " + local.HostName;
                res.Success = false;
                res.ErrorCode = -1;
                
            }

            return res;
        }

        #endregion
    }
}