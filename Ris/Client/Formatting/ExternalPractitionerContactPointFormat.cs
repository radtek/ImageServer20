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

using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Ris.Client.Formatting
{
    public static class ExternalPractitionerContactPointFormat
    {
        /// <summary>
        /// Formats the address according to the default format as specified in <see cref="FormatSettings"/>
        /// </summary>
        public static string Format(ExternalPractitionerContactPointDetail cp)
        {
            return Format(cp, FormatSettings.Default.ExternalPractitionerContactPointDefaultFormat);
        }

        /// <summary>
        /// Formats the address according to the specified format string.
        /// </summary>
        /// <remarks>
        /// Valid format specifiers are as follows:
        ///     %N - contact point name
        ///     %D - contact point description
        ///     %A - contact point address
        ///     %F - contact point fax
        ///     %T - contact point phone
        ///     %E - contact point email address
        /// </remarks>
        /// <returns></returns>
        public static string Format(ExternalPractitionerContactPointDetail cp, string format)
        {
            var result = format;
            result = result.Replace("%N", StringUtilities.EmptyIfNull(cp.Name));
            result = result.Replace("%D", StringUtilities.EmptyIfNull(cp.Description));
            result = result.Replace("%A", cp.CurrentAddress == null ? "" : AddressFormat.Format(cp.CurrentAddress));
            result = result.Replace("%F", cp.CurrentFaxNumber == null ? "" : TelephoneFormat.Format(cp.CurrentFaxNumber));
            result = result.Replace("%T", cp.CurrentPhoneNumber == null ? "" : TelephoneFormat.Format(cp.CurrentPhoneNumber));
            result = result.Replace("%E", cp.CurrentEmailAddress == null ? "" : cp.CurrentEmailAddress.Address);

            return result.Trim();
        }
    }
}
