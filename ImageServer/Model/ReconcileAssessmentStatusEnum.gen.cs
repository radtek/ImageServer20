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
public partial class ReconcileAssessmentStatusEnum : ServerEnum
{
      #region Private Static Members
      private static readonly ReconcileAssessmentStatusEnum _Pending = GetEnum("Pending");
      private static readonly ReconcileAssessmentStatusEnum _Accept = GetEnum("Accept");
      private static readonly ReconcileAssessmentStatusEnum _Reject = GetEnum("Reject");
      #endregion

      #region Public Static Properties
      /// <summary>
      /// Waiting for users to determine how the images should be reconciled
      /// </summary>
      public static ReconcileAssessmentStatusEnum Pending
      {
          get { return _Pending; }
      }
      /// <summary>
      /// Users have decided to accept images with such information
      /// </summary>
      public static ReconcileAssessmentStatusEnum Accept
      {
          get { return _Accept; }
      }
      /// <summary>
      /// Users have decided to reject images with such information
      /// </summary>
      public static ReconcileAssessmentStatusEnum Reject
      {
          get { return _Reject; }
      }

      #endregion

      #region Constructors
      public ReconcileAssessmentStatusEnum():base("ReconcileAssessmentStatusEnum")
      {}
      #endregion
      #region Public Members
      public override void SetEnum(short val)
      {
          ServerEnumHelper<ReconcileAssessmentStatusEnum, IReconcileAssessmentStatusEnumBroker>.SetEnum(this, val);
      }
      static public List<ReconcileAssessmentStatusEnum> GetAll()
      {
          return ServerEnumHelper<ReconcileAssessmentStatusEnum, IReconcileAssessmentStatusEnumBroker>.GetAll();
      }
      static public ReconcileAssessmentStatusEnum GetEnum(string lookup)
      {
          return ServerEnumHelper<ReconcileAssessmentStatusEnum, IReconcileAssessmentStatusEnumBroker>.GetEnum(lookup);
      }
      #endregion
}
}