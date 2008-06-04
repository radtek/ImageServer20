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

// This file is auto-generated by the ClearCanvas.Model.SqlServer2005.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model
{
    using System;
    using System.Collections.Generic;
    using ClearCanvas.ImageServer.Model.EntityBrokers;
    using ClearCanvas.ImageServer.Enterprise;
    using System.Reflection;

[Serializable]
public partial class ServiceLockTypeEnum : ServerEnum
{
      #region Private Static Members
      private static readonly ServiceLockTypeEnum _FilesystemDelete = GetEnum("FilesystemDelete");
      private static readonly ServiceLockTypeEnum _FilesystemReinventory = GetEnum("FilesystemReinventory");
      private static readonly ServiceLockTypeEnum _FilesystemStudyProcess = GetEnum("FilesystemStudyProcess");
      private static readonly ServiceLockTypeEnum _FilesystemLosslessCompress = GetEnum("FilesystemLosslessCompress");
      private static readonly ServiceLockTypeEnum _FilesystemLossyCompress = GetEnum("FilesystemLossyCompress");
      #endregion

      #region Public Static Properties
      /// <summary>
      /// Purge Data from a Filesystem
      /// </summary>
      public static ServiceLockTypeEnum FilesystemDelete
      {
          get { return _FilesystemDelete; }
      }
      /// <summary>
      /// Re-inventory Data within a Filesystem
      /// </summary>
      public static ServiceLockTypeEnum FilesystemReinventory
      {
          get { return _FilesystemReinventory; }
      }
      /// <summary>
      /// Reapply Study Processing rules within a Filesystem
      /// </summary>
      public static ServiceLockTypeEnum FilesystemStudyProcess
      {
          get { return _FilesystemStudyProcess; }
      }
      /// <summary>
      /// Lossless compress studies within a Filesystem
      /// </summary>
      public static ServiceLockTypeEnum FilesystemLosslessCompress
      {
          get { return _FilesystemLosslessCompress; }
      }
      /// <summary>
      /// Lossy compress studies within a Filesystem
      /// </summary>
      public static ServiceLockTypeEnum FilesystemLossyCompress
      {
          get { return _FilesystemLossyCompress; }
      }

      #endregion

      #region Constructors
      public ServiceLockTypeEnum():base("ServiceLockTypeEnum")
      {}
      #endregion
      #region Public Members
      public override void SetEnum(short val)
      {
          ServerEnumHelper<ServiceLockTypeEnum, IServiceLockTypeEnumBroker>.SetEnum(this, val);
      }
      static public IList<ServiceLockTypeEnum> GetAll()
      {
          return ServerEnumHelper<ServiceLockTypeEnum, IServiceLockTypeEnumBroker>.GetAll();
      }
      static public ServiceLockTypeEnum GetEnum(string lookup)
      {
          return ServerEnumHelper<ServiceLockTypeEnum, IServiceLockTypeEnumBroker>.GetEnum(lookup);
      }
      #endregion
}
}
