﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.Ris.Client.Common {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
    internal sealed partial class FormatSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static FormatSettings defaultInstance = ((FormatSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new FormatSettings())));
        
        public static FormatSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        /// <summary>
        /// Mask used on healthcard number input fields
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Mask used on healthcard number input fields")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0000 000 000")]
        public string HealthcardNumberMask {
            get {
                return ((string)(this["HealthcardNumberMask"]));
            }
        }
        
        /// <summary>
        /// Mask used on healthcard version code input field
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Mask used on healthcard version code input field")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("LL")]
        public string HealthcardVersionCodeMask {
            get {
                return ((string)(this["HealthcardVersionCodeMask"]));
            }
        }
        
        /// <summary>
        /// Mask used on local telephone number input fields
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Mask used on local telephone number input fields")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("000-0000")]
        public string TelephoneNumberLocalMask {
            get {
                return ((string)(this["TelephoneNumberLocalMask"]));
            }
        }
        
        /// <summary>
        /// Mask used on full telephone number input fields
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Mask used on full telephone number input fields")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("(000) 000-0000")]
        public string TelephoneNumberFullMask {
            get {
                return ((string)(this["TelephoneNumberFullMask"]));
            }
        }
    }
}
