﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.Ris.Client.Workflow {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
    internal sealed partial class ProtocollingSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static ProtocollingSettings defaultInstance = ((ProtocollingSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new ProtocollingSettings())));
        
        public static ProtocollingSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        /// <summary>
        /// Stores user default protocol groups.
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Stores user default protocol groups.")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-8\" ?><procedure-protocolgroup-defaults></proced" +
            "ure-protocolgroup-defaults>")]
        public string DefaultProtocolGroupsXml {
            get {
                return ((string)(this["DefaultProtocolGroupsXml"]));
            }
            set {
                this["DefaultProtocolGroupsXml"] = value;
            }
        }
        
        /// <summary>
        /// Stores user default supervisor.
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Stores user default supervisor.")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SupervisorID {
            get {
                return ((string)(this["SupervisorID"]));
            }
            set {
                this["SupervisorID"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShouldApplyDefaultSupervisor {
            get {
                return ((bool)(this["ShouldApplyDefaultSupervisor"]));
            }
            set {
                this["ShouldApplyDefaultSupervisor"] = value;
            }
        }
    }
}
