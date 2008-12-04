﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1820
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.Ris.Shreds.Publication {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
    internal sealed partial class PublicationShredSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static PublicationShredSettings defaultInstance = ((PublicationShredSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new PublicationShredSettings())));
        
        public static PublicationShredSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        /// <summary>
        /// Number of items to pull from queue per read
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Number of items to pull from queue per read")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("20")]
        public int BatchSize {
            get {
                return ((int)(this["BatchSize"]));
            }
        }
        
        /// <summary>
        /// Number of seconds to sleep when queue is empty
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Number of seconds to sleep when queue is empty")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30")]
        public int EmptyQueueSleepTime {
            get {
                return ((int)(this["EmptyQueueSleepTime"]));
            }
        }
    }
}
