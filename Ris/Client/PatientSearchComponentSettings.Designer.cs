﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.Ris.Client {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
    internal sealed partial class PatientSearchComponentSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static PatientSearchComponentSettings defaultInstance = ((PatientSearchComponentSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new PatientSearchComponentSettings())));
        
        public static PatientSearchComponentSettings Default {
            get {
                return defaultInstance;
            }
        }
        
		/// <summary>
		/// Searches that would return more than this number of results will be rejected
		/// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Configuration.SettingsDescriptionAttribute("Searches that would return more than this number of results will be rejected")]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("200")]
        public int SearchCriteriaSpecificityThreshold {
            get {
                return ((int)(this["SearchCriteriaSpecificityThreshold"]));
            }
        }
    }
}
