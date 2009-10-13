﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.Ris.Application.Services {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SR {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SR() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ClearCanvas.Ris.Application.Services.SR", typeof(SR).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A canned text category is required.
        /// </summary>
        internal static string ExceptionCannedTextCategoryRequired {
            get {
                return ResourceManager.GetString("ExceptionCannedTextCategoryRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A canned text name is required.
        /// </summary>
        internal static string ExceptionCannedTextNameRequired {
            get {
                return ResourceManager.GetString("ExceptionCannedTextNameRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A practitioner with the name {0}, {1} already exists.
        /// </summary>
        internal static string ExceptionExternalPractitionerAlreadyExist {
            get {
                return ResourceManager.GetString("ExceptionExternalPractitionerAlreadyExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A facility with code {0} already exists.
        /// </summary>
        internal static string ExceptionFacilityAlreadyExist {
            get {
                return ResourceManager.GetString("ExceptionFacilityAlreadyExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to delete {0}.  The most likely reason is that the item is referenced by another item in the system..
        /// </summary>
        internal static string ExceptionFailedToDelete {
            get {
                return ResourceManager.GetString("ExceptionFailedToDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A family name must be provided.
        /// </summary>
        internal static string ExceptionFamilyNameMissing {
            get {
                return ResourceManager.GetString("ExceptionFamilyNameMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An identical canned text entry already exists for {0}.
        /// </summary>
        internal static string ExceptionIdenticalCannedTextExist {
            get {
                return ResourceManager.GetString("ExceptionIdenticalCannedTextExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attempt to import entities with the same Id but different names.
        /// </summary>
        internal static string ExceptionImportEntityNameIdMismatch {
            get {
                return ResourceManager.GetString("ExceptionImportEntityNameIdMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A location with the same facility, building, floor, point of care, room and bed already exists.
        /// </summary>
        internal static string ExceptionLocationAlreadyExist {
            get {
                return ResourceManager.GetString("ExceptionLocationAlreadyExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have reached the maximum number of worklists that can be defined for a staff..
        /// </summary>
        internal static string ExceptionMaximumWorklistsReachedForStaff {
            get {
                return ResourceManager.GetString("ExceptionMaximumWorklistsReachedForStaff", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have reached the maximum number of worklists that can be defined for a staff group..
        /// </summary>
        internal static string ExceptionMaximumWorklistsReachedForStaffGroup {
            get {
                return ResourceManager.GetString("ExceptionMaximumWorklistsReachedForStaffGroup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A modality with ID {0} already exists.
        /// </summary>
        internal static string ExceptionModalityAlreadyExist {
            get {
                return ResourceManager.GetString("ExceptionModalityAlreadyExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A patient profile with MRN {0} {1} already exists.
        /// </summary>
        internal static string ExceptionMrnAlreadyExists {
            get {
                return ResourceManager.GetString("ExceptionMrnAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No scheduled Protocol Assignment steps exist for this procedure.  Either the assignment step was never created for this procedure, or it has been started by another user..
        /// </summary>
        internal static string ExceptionNoProtocolAssignmentStep {
            get {
                return ResourceManager.GetString("ExceptionNoProtocolAssignmentStep", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Current user must be associated with a Staff in order to perform this operation.
        /// </summary>
        internal static string ExceptionNoStaffForUser {
            get {
                return ResourceManager.GetString("ExceptionNoStaffForUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A note category with name {0} already exists.
        /// </summary>
        internal static string ExceptionNoteCategoryAlreadyExist {
            get {
                return ResourceManager.GetString("ExceptionNoteCategoryAlreadyExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to find current user.
        /// </summary>
        internal static string ExceptionNoUser {
            get {
                return ResourceManager.GetString("ExceptionNoUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only one contact point can be set as the default..
        /// </summary>
        internal static string ExceptionOneDefaultContactPoint {
            get {
                return ResourceManager.GetString("ExceptionOneDefaultContactPoint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password has expired..
        /// </summary>
        internal static string ExceptionPasswordExpired {
            get {
                return ResourceManager.GetString("ExceptionPasswordExpired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Patient with MRN {0} {1} not found.
        /// </summary>
        internal static string ExceptionPatientNotFound {
            get {
                return ResourceManager.GetString("ExceptionPatientNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A practitioner with the name {0}, {1} already exists.
        /// </summary>
        internal static string ExceptionPractitionerAlreadyExist {
            get {
                return ResourceManager.GetString("ExceptionPractitionerAlreadyExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please check in the patient for this procedure and try again..
        /// </summary>
        internal static string ExceptionProcedureNotCheckedIn {
            get {
                return ResourceManager.GetString("ExceptionProcedureNotCheckedIn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A procedure plan Xml is required..
        /// </summary>
        internal static string ExceptionProcedurePlanXmlRequired {
            get {
                return ResourceManager.GetString("ExceptionProcedurePlanXmlRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A procedure type group with the name {0} already exists.
        /// </summary>
        internal static string ExceptionProcedureTypeGroupNameAlreadyExists {
            get {
                return ResourceManager.GetString("ExceptionProcedureTypeGroupNameAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A procedure type group name is required.
        /// </summary>
        internal static string ExceptionProcedureTypeGroupNameRequired {
            get {
                return ResourceManager.GetString("ExceptionProcedureTypeGroupNameRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to At least 2 patients must be specified for reconciliation.
        /// </summary>
        internal static string ExceptionReconciliationRequiresAtLeast2Patients {
            get {
                return ResourceManager.GetString("ExceptionReconciliationRequiresAtLeast2Patients", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A staff with the name {0}, {1} already exists.
        /// </summary>
        internal static string ExceptionStaffAlreadyExist {
            get {
                return ResourceManager.GetString("ExceptionStaffAlreadyExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hard enumeration cannot be deleted..
        /// </summary>
        internal static string ExceptionUnableToDeleteHardEnumeration {
            get {
                return ResourceManager.GetString("ExceptionUnableToDeleteHardEnumeration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A user with user ID &apos;{0}&apos; already exists.
        /// </summary>
        internal static string ExceptionUserIDAlreadyExists {
            get {
                return ResourceManager.GetString("ExceptionUserIDAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User not authorized..
        /// </summary>
        internal static string ExceptionUserNotAuthorized {
            get {
                return ResourceManager.GetString("ExceptionUserNotAuthorized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The report content is required to verify this step.
        /// </summary>
        internal static string ExceptionVerifyWithNoReport {
            get {
                return ResourceManager.GetString("ExceptionVerifyWithNoReport", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A worklist with the name {0} already exists.
        /// </summary>
        internal static string ExceptionWorklistNameAlreadyExists {
            get {
                return ResourceManager.GetString("ExceptionWorklistNameAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A worklist name is required..
        /// </summary>
        internal static string ExceptionWorklistNameRequired {
            get {
                return ResourceManager.GetString("ExceptionWorklistNameRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Documentation is not completed..
        /// </summary>
        internal static string MessageDocumentationIncomplete {
            get {
                return ResourceManager.GetString("MessageDocumentationIncomplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not all ordered procedures have been performed..
        /// </summary>
        internal static string MessageNotAllProceduresPerformed {
            get {
                return ResourceManager.GetString("MessageNotAllProceduresPerformed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Checked In.
        /// </summary>
        internal static string TextCheckedIn {
            get {
                return ResourceManager.GetString("TextCheckedIn", resourceCulture);
            }
        }
    }
}
