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
public partial class StudyStatusEnum : ServerEnum
{
      #region Private Static Members
      private static readonly StudyStatusEnum _Online = GetEnum("Online");
      private static readonly StudyStatusEnum _OnlineLossless = GetEnum("OnlineLossless");
      private static readonly StudyStatusEnum _OnlineLossy = GetEnum("OnlineLossy");
      private static readonly StudyStatusEnum _Nearline = GetEnum("Nearline");
      #endregion

      #region Public Static Properties
      /// <summary>
      /// Study is online
      /// </summary>
      public static StudyStatusEnum Online
      {
          get { return _Online; }
      }
      /// <summary>
      /// Study is online and lossless compressed
      /// </summary>
      public static StudyStatusEnum OnlineLossless
      {
          get { return _OnlineLossless; }
      }
      /// <summary>
      /// Study is online and lossy compressed
      /// </summary>
      public static StudyStatusEnum OnlineLossy
      {
          get { return _OnlineLossy; }
      }
      /// <summary>
      /// The study is nearline (in an automated library)
      /// </summary>
      public static StudyStatusEnum Nearline
      {
          get { return _Nearline; }
      }

      #endregion

      #region Constructors
      public StudyStatusEnum():base("StudyStatusEnum")
      {}
      #endregion
      #region Public Members
      public override void SetEnum(short val)
      {
          ServerEnumHelper<StudyStatusEnum, IStudyStatusEnumBroker>.SetEnum(this, val);
      }
      static public List<StudyStatusEnum> GetAll()
      {
          return ServerEnumHelper<StudyStatusEnum, IStudyStatusEnumBroker>.GetAll();
      }
      static public StudyStatusEnum GetEnum(string lookup)
      {
          return ServerEnumHelper<StudyStatusEnum, IStudyStatusEnumBroker>.GetEnum(lookup);
      }
      #endregion
}
}
