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

namespace ClearCanvas.ImageServer.Enterprise.SqlServer2005
{
	public class PersistentStoreVersionUpdateColumns : EntityUpdateColumns
	{
		public PersistentStoreVersionUpdateColumns()
			: base("DatabaseVersion_")
		{ }
		[EntityFieldDatabaseMappingAttribute(TableName = "DatabaseVersion_", ColumnName = "Build_")]
		public String Build
		{
			set { SubParameters["Build"] = new EntityUpdateColumn<String>("Build", value); }
		}
		[EntityFieldDatabaseMappingAttribute(TableName = "DatabaseVersion_", ColumnName = "Major_")]
		public String Major
		{
			set { SubParameters["Major"] = new EntityUpdateColumn<String>("Major", value); }
		}
		[EntityFieldDatabaseMappingAttribute(TableName = "DatabaseVersion_", ColumnName = "Minor_")]
		public String Minor
		{
			set { SubParameters["Minor"] = new EntityUpdateColumn<String>("Minor", value); }
		}
		[EntityFieldDatabaseMappingAttribute(TableName = "DatabaseVersion_", ColumnName = "Revision_")]
		public String Revision
		{
			set { SubParameters["Revision"] = new EntityUpdateColumn<String>("Revision", value); }
		}
	}
}
