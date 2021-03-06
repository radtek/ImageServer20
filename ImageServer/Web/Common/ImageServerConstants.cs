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

    public class ImageServerConstants
    {
        #region GridViewPagerPosition enum

        public enum GridViewPagerPosition
        {
            Top = 0,
            Bottom = 1,
        } ;

        #endregion

        public const string CookieDateTimeFormat = "yyyy-M-d H:m:s";

        public const string Default = "Default";
        public const string DefaultConfigurationXml = "<HsmArchive><RootDir>e:\\Archive</RootDir></HsmArchive>";
        public const string First = "first";
        public const string High = "high";
        public const string ImagePng = "image/png";
        public const string Last = "last";
        public const string Low = "low";
        public const string Next = "next";
        public const string PagerItemCount = "ItemCount";
        public const string Pct = "pct";
        public const string Prev = "prev";

        public static string[] ReasonCommentSeparator = {"::"};

        /// <summary>
        /// Sets or gets the theme for the web application.
        /// </summary>
        public static string Theme { set; get; }

        #region Nested type: ContextKeys

        public class ContextKeys
        {
            public const string ErrorDescription = "ERROR_DESCRIPTION";
            public const string ErrorMessage = "ERROR_MESSAGE";
            public const string StackTrace = "STACK_TRACE";
        }

        #endregion

        #region Nested type: ImageURLs

        public class ImageURLs
        {
            public const string AddButtonDisabled = "images/Buttons/AddDisabled.png";
            public const string AddButtonEnabled = "images/Buttons/AddEnabled.png";
            public const string AddButtonHover = "images/Buttons/AddHover.png";
            public const string UpdateButtonDisabled = "images/Buttons/UpdateDisabled.png";
            public const string UpdateButtonEnabled = "images/Buttons/UpdateEnabled.png";
            public const string UpdateButtonHover = "images/Buttons/UpdateHover.png";

            public static readonly string AcceptKOPRFeature =
                string.Format("~/App_Themes/{0}/images/Indicators/AcceptKOPRFeature.png", Theme);

            public static readonly string AutoRouteFeature =
                string.Format("~/App_Themes/{0}/images/Indicators/AutoRouteFeature.png", Theme);

            public static readonly string Blank = string.Format("~/App_Themes/{0}/images/blank.gif", Theme);

            public static readonly string CalendarIcon =
                string.Format("~/App_Themes/{0}/images/Buttons/CalendarIcon.png", Theme);

            public static readonly string Checked = string.Format("~/App_Themes/{0}/images/Indicators/checked.png",
                                                                  Theme);

            public static readonly string GridPagerFirstDisabled =
                string.Format("~/App_Themes/{0}/images/Controls/GridView/GridViewPagerFirstDisabled.png", Theme);

            public static readonly string GridPagerFirstEnabled =
                string.Format("~/App_Themes/{0}/images/Controls/GridView/GridViewPagerFirstEnabled.png", Theme);

            public static readonly string GridPagerLastDisabled =
                string.Format("~/App_Themes/{0}/images/Controls/GridView/GridViewPagerLastDisabled.png", Theme);

            public static readonly string GridPagerLastEnabled =
                string.Format("~/App_Themes/{0}/images/Controls/GridView/GridViewPagerLastEnabled.png", Theme);

            public static readonly string GridPagerNextDisabled =
                string.Format("~/App_Themes/{0}/images/Controls/GridView/GridViewPagerNextDisabled.png", Theme);

            public static readonly string GridPagerNextEnabled =
                string.Format("~/App_Themes/{0}/images/Controls/GridView/GridViewPagerNextEnabled.png", Theme);

            public static readonly string GridPagerPreviousDisabled =
                string.Format("~/App_Themes/{0}/images/Controls/GridView/GridViewPagerPreviousDisabled.png", Theme);

            public static readonly string GridPagerPreviousEnabled =
                string.Format("~/App_Themes/{0}/images/Controls/GridView/GridViewPagerPreviousEnabled.png", Theme);

            public static readonly string IdeographyName =
                string.Format("~/App_Themes/{0}/images/Indicators/IdeographicName.gif", Theme);

            public static readonly string PhoneticName =
                string.Format("~/App_Themes/{0}/images/Indicators/PhoneticName.gif", Theme);

            public static readonly string QueryFeature =
                string.Format("~/App_Themes/{0}/images/Indicators/QueryFeature.png", Theme);

            public static readonly string RetrieveFeature =
                string.Format("~/App_Themes/{0}/images/Indicators/RetrieveFeature.png", Theme);

            public static readonly string StoreFeature =
                string.Format("~/App_Themes/{0}/images/Indicators/StoreFeature.png", Theme);

            public static readonly string Unchecked = string.Format("~/App_Themes/{0}/images/Indicators/unchecked.png",
                                                                    Theme);

            public static readonly string UsageBar = string.Format("~/App_Themes/{0}/images/Indicators/usage.png", Theme);

            public static readonly string Watermark = string.Format("~/App_Themes/{0}/images/Indicators/Watermark.gif",
                                                                    Theme);
        }

        #endregion

        #region Nested type: PageURLs

        public class PageURLs
        {
            public const string AdminUserPage = "~/Pages/Admin/UserManagement/Users/Default.aspx";
            public const string ApplicationLog = "~/Pages/Admin/ApplicationLog/Default.aspx";
            public const string ArchiveQueuePage = "~/Pages/Queues/ArchiveQueue/Default.aspx";
            public const string AuthorizationErrorPage = "~/Pages/Error/AuthorizationErrorPage.aspx";
            public const string BarChartPage = "~/Pages/Common/BarChart.aspx?pct={0}&high={1}&low={2}";
            public const string CookiesErrorPage = "~/Pages/Error/CookiesRequired.aspx";
            public const string ErrorPage = "~/Pages/Error/ErrorPage.aspx";
            public const string JavascriptErrorPage = "~/Pages/Error/JavascriptRequired.aspx";
            public const string LoginPage = "~/Pages/Login/Default.aspx";
            public const string MoveSeriesPage = "~/Pages/Studies/MoveSeries/Default.aspx";
            public const string MoveStudyPage = "~/Pages/Studies/Move/Default.aspx";
            public const string NumberFormatScript = "~/Scripts/NumberFormat154.js";
            public const string RestoreQueuePage = "~/Pages/Queues/RestoreQueue/Default.aspx";
            public const string SearchPage = "~/Pages/Studies/Default.aspx";
            public const string SeriesDetailsPage = "~/Pages/Studies/SeriesDetails/Default.aspx";
            public const string StudiesPage = "~/Pages/Studies/Default.aspx";
            public const string StudyDetailsPage = "~/Pages/Studies/StudyDetails/Default.aspx";
            public const string StudyIntegrityQueuePage = "~/Pages/Queues/StudyIntegrityQueue/Default.aspx";
            public const string WebServices = "~/Services/webservice.htc";
            public const string WorkQueueItemDeletedPage = "~/Pages/Queues/WorkQueue/Edit/WorkQueueItemDeleted.aspx";
            public const string WorkQueueItemDetailsPage = "~/Pages/Queues/WorkQueue/Edit/Default.aspx";
            public const string WorkQueuePage = "~/Pages/Queues/WorkQueue/Default.aspx";
        }

        #endregion

        #region Nested type: QueryStrings

        public class QueryStrings
        {
            public const string SeriesUID = "seriesuid";
            public const string ServerAE = "serverae";
            public const string StudyInstanceUID = "siuid";
            public const string StudyUID = "studyuid";
        }

        #endregion
    }