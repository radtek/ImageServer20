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

using System;
using ClearCanvas.ImageServer.Model;

namespace ClearCanvas.ImageServer.Web.Application.SeriesDetails
{
    static public class SeriesDetailsAssembler
    {
        /// <summary>
        /// Returns an instance of <see cref="SeriesDetails"/> for a <see cref="Series"/>.
        /// </summary>
        /// <param name="series"></param>
        /// <returns></returns>
        /// <remark>
        /// 
        /// </remark>
        static public SeriesDetails CreateSeriesDetails(Model.Series series)
        {
            SeriesDetails details = new SeriesDetails();

            details.Modality = series.Modality;
            details.NumberOfSeriesRelatedInstances = series.NumberOfSeriesRelatedInstances;
            details.PerformedDate = series.PerformedProcedureStepStartDate;
            details.PerformedTime = series.PerformedProcedureStepStartTime;
            details.SeriesDescription = series.SeriesDescription;
            details.SeriesInstanceUid = series.SeriesInstanceUid;
            details.SeriesNumber = series.SeriesNumber;
            details.SourceApplicationEntityTitle = series.SourceApplicationEntityTitle;

            return details;
        }
    }
}
