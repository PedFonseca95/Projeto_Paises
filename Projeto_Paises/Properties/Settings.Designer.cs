﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Projeto_Paises.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=ProjetoPaises.mssql.somee.com;Initial Catalog=ProjetoPaises;User ID=p" +
            "fonseca95_SQLLogin_1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=Fal" +
            "se")]
        public string ProjetoPaisesConnectionString {
            get {
                return ((string)(this["ProjetoPaisesConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=ProjetoPaises.mssql.somee.com;Initial Catalog=ProjetoPaises;User ID=p" +
            "fonseca95_SQLLogin_1;Password=1crom8fbet;Connect Timeout=30;Encrypt=False;TrustS" +
            "erverCertificate=False")]
        public string ProjetoPaisesConnectionString1 {
            get {
                return ((string)(this["ProjetoPaisesConnectionString1"]));
            }
        }
    }
}
