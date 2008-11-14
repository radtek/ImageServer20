﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.ImageServer.Web.Common.Exceptions {
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
    internal class ExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ClearCanvas.ImageServer.Web.Common.Exceptions.ExceptionMessages", typeof(ExceptionMessages).Assembly);
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
        ///   Looks up a localized string similar to The ImageServer encountered and error trying to update part of the web page you were just viewing..
        /// </summary>
        internal static string AJAXError {
            get {
                return ResourceManager.GetString("AJAXError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This message is being displayed because in some cases the ImageServer refreshes part of the pages using a technology called &quot;AJAX&quot;. Something happened on the server that prevented the page from updating properly..
        /// </summary>
        internal static string AJAXErrorDescription {
            get {
                return ResourceManager.GetString("AJAXErrorDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No Message Provided..
        /// </summary>
        internal static string EmptyLogMessage {
            get {
                return ResourceManager.GetString("EmptyLogMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The ImageServer can&apos;t find a partition with the name &quot;{0}&quot;..
        /// </summary>
        internal static string PartitionNotFound {
            get {
                return ResourceManager.GetString("PartitionNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This message is being displayed because the ImageServer can&apos;t find the requested partition. This usually occurs because the name provided is incorrect. .
        /// </summary>
        internal static string PartitonNotFoundDescription {
            get {
                return ResourceManager.GetString("PartitonNotFoundDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The ImageServer can&apos;t find a study with Study Instance UID &quot;{0}&quot;..
        /// </summary>
        internal static string StudyInstanceUIDNotFound {
            get {
                return ResourceManager.GetString("StudyInstanceUIDNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  This message is being displayed because the ImageServer can&apos;t find the study that has been requested. This usually occurs because the Study Instance UID is invalid and not contained within the ImageServer..
        /// </summary>
        internal static string StudyInstanceUIDNotFoundDescription {
            get {
                return ResourceManager.GetString("StudyInstanceUIDNotFoundDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The ImageServer can&apos;t find a study with Study Instance UID &quot;{0}&quot;..
        /// </summary>
        internal static string StudyNotFound {
            get {
                return ResourceManager.GetString("StudyNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This message is being displayed because the ImageServer can&apos;t find the study that has been requested. This usually occurs because the Study Instance UID is invalid and not contained within the ImageServer..
        /// </summary>
        internal static string StudyNotFoundDescription {
            get {
                return ResourceManager.GetString("StudyNotFoundDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The ImageServer can&apos;t find the work queue item requested..
        /// </summary>
        internal static string WorkQueueItemNotFound {
            get {
                return ResourceManager.GetString("WorkQueueItemNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This message is being displayed because the work queue item can&apos;t be found. This usually occurs when the work queue item no longer exists because it has been processed or deleted..
        /// </summary>
        internal static string WorkQueueItemNotFoundDescription {
            get {
                return ResourceManager.GetString("WorkQueueItemNotFoundDescription", resourceCulture);
            }
        }
    }
}
