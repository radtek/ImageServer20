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

// This file is auto-generated by the ClearCanvas.Model.SqlServer2005.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model
{
    using System;
    using System.Collections.Generic;
    using ClearCanvas.ImageServer.Model.EntityBrokers;
    using ClearCanvas.ImageServer.Enterprise;
    using System.Reflection;

[Serializable]
public partial class RestoreQueueStatusEnum : ServerEnum
{
      #region Private Static Members
      private static readonly RestoreQueueStatusEnum _Pending = GetEnum("Pending");
      private static readonly RestoreQueueStatusEnum _InProgress = GetEnum("In Progress");
      private static readonly RestoreQueueStatusEnum _Completed = GetEnum("Completed");
      private static readonly RestoreQueueStatusEnum _Failed = GetEnum("Failed");
      private static readonly RestoreQueueStatusEnum _Restoring = GetEnum("Restoring");
      #endregion

      #region Public Static Properties
      /// <summary>
      /// Pending
      /// </summary>
      public static RestoreQueueStatusEnum Pending
      {
          get { return _Pending; }
      }
      /// <summary>
      /// In Progress
      /// </summary>
      public static RestoreQueueStatusEnum InProgress
      {
          get { return _InProgress; }
      }
      /// <summary>
      /// The Queue entry is completed.
      /// </summary>
      public static RestoreQueueStatusEnum Completed
      {
          get { return _Completed; }
      }
      /// <summary>
      /// The Queue entry has failed.
      /// </summary>
      public static RestoreQueueStatusEnum Failed
      {
          get { return _Failed; }
      }
      /// <summary>
      /// The Queue entry is waiting for the study to be restored by the archive.
      /// </summary>
      public static RestoreQueueStatusEnum Restoring
      {
          get { return _Restoring; }
      }

      #endregion

      #region Constructors
      public RestoreQueueStatusEnum():base("RestoreQueueStatusEnum")
      {}
      #endregion
      #region Public Members
      public override void SetEnum(short val)
      {
          ServerEnumHelper<RestoreQueueStatusEnum, IRestoreQueueStatusEnumBroker>.SetEnum(this, val);
      }
      static public List<RestoreQueueStatusEnum> GetAll()
      {
          return ServerEnumHelper<RestoreQueueStatusEnum, IRestoreQueueStatusEnumBroker>.GetAll();
      }
      static public RestoreQueueStatusEnum GetEnum(string lookup)
      {
          return ServerEnumHelper<RestoreQueueStatusEnum, IRestoreQueueStatusEnumBroker>.GetEnum(lookup);
      }
      #endregion
}
}
