﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.832
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.Ris.Client {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
    internal sealed partial class OrderNoteConversationComponentSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static OrderNoteConversationComponentSettings defaultInstance = ((OrderNoteConversationComponentSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new OrderNoteConversationComponentSettings())));
        
        public static OrderNoteConversationComponentSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string PreferredOnBehalfOfGroupName {
            get {
                return ((string)(this["PreferredOnBehalfOfGroupName"]));
            }
            set {
                this["PreferredOnBehalfOfGroupName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string DefaultRecipients {
            get {
                return ((string)(this["DefaultRecipients"]));
            }
            set {
                this["DefaultRecipients"] = value;
            }
        }
    }
}
