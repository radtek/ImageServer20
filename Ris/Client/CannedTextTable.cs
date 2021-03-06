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

using ClearCanvas.Desktop.Tables;
using ClearCanvas.Ris.Application.Common.CannedTextService;

namespace ClearCanvas.Ris.Client
{
	public class CannedTextTable : Table<CannedTextSummary>
	{
		public CannedTextTable()
		{
			this.Columns.Add(new TableColumn<CannedTextSummary, string>(SR.ColumnName, c => c.Name, 1.0f));
			this.Columns.Add(new TableColumn<CannedTextSummary, string>(SR.ColumnCategory, c => c.Category, 1.0f));
			this.Columns.Add(new TableColumn<CannedTextSummary, string>(SR.ColumnText, c => FormatCannedTextSnippet(c.TextSnippet), 3.0f));
			this.Columns.Add(new TableColumn<CannedTextSummary, string>(SR.ColumnCannedTextOwner,
				item => item.IsPersonal ? SR.ColumnPersonal : item.StaffGroup.Name, 1.0f));

			// Apply sort from settings
			var sortColumnIndex = this.Columns.FindIndex(column => column.Name.Equals(CannedTextSettings.Default.SummarySortColumnName));
			this.Sort(new TableSortParams(this.Columns[sortColumnIndex], CannedTextSettings.Default.SummarySortAscending));

			this.Sorted += OnCannedTextTableSorted;
		}

		private void OnCannedTextTableSorted(object sender, System.EventArgs e)
		{
			if (this.SortParams == null)
				return;

			// Save last sort
			CannedTextSettings.Default.SummarySortColumnName = this.SortParams.Column.Name;
			CannedTextSettings.Default.SummarySortAscending = this.SortParams.Ascending;
			CannedTextSettings.Default.Save();
		}

		private static string FormatCannedTextSnippet(string text)
		{
			return text.Length < CannedTextSummary.MaxTextLength ? text : string.Concat(text, "...");
		}
	}
}
