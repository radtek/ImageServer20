﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.ImageViewer.Explorer.Dicom {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    internal sealed partial class DicomExplorerConfigurationSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static DicomExplorerConfigurationSettings defaultInstance = ((DicomExplorerConfigurationSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new DicomExplorerConfigurationSettings())));
        
        public static DicomExplorerConfigurationSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShowIdeographicName {
            get {
                return ((bool)(this["ShowIdeographicName"]));
            }
            set {
                this["ShowIdeographicName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShowPhoneticName {
            get {
                return ((bool)(this["ShowPhoneticName"]));
            }
            set {
                this["ShowPhoneticName"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(",")]
        public char NameSeparator {
            get {
                return ((char)(this["NameSeparator"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ShowNumberOfImagesInStudy {
            get {
                return ((bool)(this["ShowNumberOfImagesInStudy"]));
            }
            set {
                this["ShowNumberOfImagesInStudy"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SelectDefaultServerOnStartup {
            get {
                return ((bool)(this["SelectDefaultServerOnStartup"]));
            }
            set {
                this["SelectDefaultServerOnStartup"] = value;
            }
        }
    }
}
