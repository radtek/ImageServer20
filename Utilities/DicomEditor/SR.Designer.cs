﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.Utilities.DicomEditor {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ClearCanvas.Utilities.DicomEditor.SR", typeof(SR).Assembly);
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
        ///   Looks up a localized string similar to Group-Element.
        /// </summary>
        internal static string ColumnHeadingGroupElement {
            get {
                return ResourceManager.GetString("ColumnHeadingGroupElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Length.
        /// </summary>
        internal static string ColumnHeadingLength {
            get {
                return ResourceManager.GetString("ColumnHeadingLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tag Name.
        /// </summary>
        internal static string ColumnHeadingTagName {
            get {
                return ResourceManager.GetString("ColumnHeadingTagName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value.
        /// </summary>
        internal static string ColumnHeadingValue {
            get {
                return ResourceManager.GetString("ColumnHeadingValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to VR.
        /// </summary>
        internal static string ColumnHeadingVR {
            get {
                return ResourceManager.GetString("ColumnHeadingVR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Create.
        /// </summary>
        internal static string MenuCreate {
            get {
                return ResourceManager.GetString("MenuCreate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete.
        /// </summary>
        internal static string MenuDelete {
            get {
                return ResourceManager.GetString("MenuDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DICOM Editor.
        /// </summary>
        internal static string MenuDicomEditor {
            get {
                return ResourceManager.GetString("MenuDicomEditor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dump Files.
        /// </summary>
        internal static string MenuDumpFiles {
            get {
                return ResourceManager.GetString("MenuDumpFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Next.
        /// </summary>
        internal static string MenuNext {
            get {
                return ResourceManager.GetString("MenuNext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Previous.
        /// </summary>
        internal static string MenuPrevious {
            get {
                return ResourceManager.GetString("MenuPrevious", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quick anonymize.
        /// </summary>
        internal static string MenuQuickAnonymize {
            get {
                return ResourceManager.GetString("MenuQuickAnonymize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Replicate.
        /// </summary>
        internal static string MenuReplicate {
            get {
                return ResourceManager.GetString("MenuReplicate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Revert.
        /// </summary>
        internal static string MenuRevert {
            get {
                return ResourceManager.GetString("MenuRevert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Save.
        /// </summary>
        internal static string MenuSave {
            get {
                return ResourceManager.GetString("MenuSave", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The selected tag(s) will be deleted.  Are you sure you want to continue?.
        /// </summary>
        internal static string MessageConfirmDeleteSelectedTags {
            get {
                return ResourceManager.GetString("MessageConfirmDeleteSelectedTags", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you want to delete the selected tags from *all* the other loaded files in addition to the one selected?.
        /// </summary>
        internal static string MessageConfirmDeleteSelectedTagsFromAllFiles {
            get {
                return ResourceManager.GetString("MessageConfirmDeleteSelectedTagsFromAllFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The selected tag(s) and their values will be replicated in *all* loaded files.  Are you sure you want to continue?.
        /// </summary>
        internal static string MessageConfirmReplicateTagsInAllFiles {
            get {
                return ResourceManager.GetString("MessageConfirmReplicateTagsInAllFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The edits will be reverted to the last saved state.  Are you sure you want to continue?.
        /// </summary>
        internal static string MessageConfirmRevert {
            get {
                return ResourceManager.GetString("MessageConfirmRevert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Would you like to revert *all* loaded files in addition to this one?.
        /// </summary>
        internal static string MessageConfirmRevertAllFiles {
            get {
                return ResourceManager.GetString("MessageConfirmRevertAllFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The changes will be saved to *all* loaded files and the original files will be overwritten.  Are you sure you want to continue?.
        /// </summary>
        internal static string MessageConfirmSaveAllFiles {
            get {
                return ResourceManager.GetString("MessageConfirmSaveAllFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Processing Files....
        /// </summary>
        internal static string MessageDumpProgressBar {
            get {
                return ResourceManager.GetString("MessageDumpProgressBar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This tool is only valid when a DICOM Image is selected..
        /// </summary>
        internal static string MessagePleaseSelectAnImage {
            get {
                return ResourceManager.GetString("MessagePleaseSelectAnImage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Create Tag.
        /// </summary>
        internal static string TitleCreateTag {
            get {
                return ResourceManager.GetString("TitleCreateTag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DICOM Editor.
        /// </summary>
        internal static string TitleDicomEditor {
            get {
                return ResourceManager.GetString("TitleDicomEditor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Create.
        /// </summary>
        internal static string ToolbarCreate {
            get {
                return ResourceManager.GetString("ToolbarCreate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete.
        /// </summary>
        internal static string ToolbarDelete {
            get {
                return ResourceManager.GetString("ToolbarDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Next.
        /// </summary>
        internal static string ToolbarNext {
            get {
                return ResourceManager.GetString("ToolbarNext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Previous.
        /// </summary>
        internal static string ToolbarPrevious {
            get {
                return ResourceManager.GetString("ToolbarPrevious", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quick anonymize.
        /// </summary>
        internal static string ToolbarQuickAnonymize {
            get {
                return ResourceManager.GetString("ToolbarQuickAnonymize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Replicate.
        /// </summary>
        internal static string ToolbarReplicate {
            get {
                return ResourceManager.GetString("ToolbarReplicate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Revert.
        /// </summary>
        internal static string ToolbarRevert {
            get {
                return ResourceManager.GetString("ToolbarRevert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Save.
        /// </summary>
        internal static string ToolbarSave {
            get {
                return ResourceManager.GetString("ToolbarSave", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Create new tag.
        /// </summary>
        internal static string TooltipCreate {
            get {
                return ResourceManager.GetString("TooltipCreate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete tag.
        /// </summary>
        internal static string TooltipDelete {
            get {
                return ResourceManager.GetString("TooltipDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Next image.
        /// </summary>
        internal static string TooltipNext {
            get {
                return ResourceManager.GetString("TooltipNext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Previous image.
        /// </summary>
        internal static string TooltipPrevious {
            get {
                return ResourceManager.GetString("TooltipPrevious", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Anonymize all loaded files with one click.
        /// </summary>
        internal static string TooltipQuickAnonymize {
            get {
                return ResourceManager.GetString("TooltipQuickAnonymize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Replicates selected tag to all loaded files.
        /// </summary>
        internal static string TooltipReplicate {
            get {
                return ResourceManager.GetString("TooltipReplicate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Revert changes.
        /// </summary>
        internal static string TooltipRevert {
            get {
                return ResourceManager.GetString("TooltipRevert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Saves edits.
        /// </summary>
        internal static string TooltipSave {
            get {
                return ResourceManager.GetString("TooltipSave", resourceCulture);
            }
        }
    }
}
