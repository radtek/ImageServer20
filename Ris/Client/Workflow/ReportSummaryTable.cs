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
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tables;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.ReportingWorkflow;
using ClearCanvas.Ris.Client.Formatting;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Ris.Client.Reporting
{
    public class ReportSummaryTable : Table<ReportSummary>
    {
        public ReportSummaryTable()
        {
            this.Columns.Add(new TableColumn<ReportSummary, string>("Accession Number",
                delegate(ReportSummary report) { return report.AccessionNumber; }));
            this.Columns.Add(new TableColumn<ReportSummary, string>("Visit Number",
                delegate(ReportSummary report) { return VisitNumberFormat.Format(report.VisitNumber); }));
            this.Columns.Add(new TableColumn<ReportSummary, string>("Requested Procedure",
                delegate(ReportSummary report)
                {
                    return StringUtilities.Combine(report.Procedures, ", ",
                        delegate(RequestedProcedureSummary summary) { return summary.Type.Name; });
                }));
            this.Columns.Add(new TableColumn<ReportSummary, string>("Performed Location",
                delegate(ReportSummary report) { return "WHAT?"; }));
            this.Columns.Add(new TableColumn<ReportSummary, string>("Performed Date",
                delegate(ReportSummary report) { return "WHAT?"; }));
            this.Columns.Add(new TableColumn<ReportSummary, string>("Status",
                delegate(ReportSummary report) { return report.ReportStatus.Value; }));
        }
    }
}
