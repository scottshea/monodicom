﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.Dicom.ServiceModel.Streaming {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class StreamingSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static StreamingSettings defaultInstance = ((StreamingSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new StreamingSettings())));
        
        public static StreamingSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int ClientTimeoutSeconds {
            get {
                return ((int)(this["ClientTimeoutSeconds"]));
            }
        }
    }
}
