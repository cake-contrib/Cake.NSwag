using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.NSwag.Settings
{
    public class TypeScriptGeneratorSettings
    {
        internal TypeScriptGeneratorSettings()
        {
            
        }
        public string ClassName { get; set; }
        public string ModuleName { get; set; }
    }

    public static class TypeScriptGeneratorSettingsExtensions
    {
        public static TypeScriptGeneratorSettings WithClassName(this TypeScriptGeneratorSettings settings,
            string className)
        {
            settings.ClassName = className;
            return settings;
        }

        public static TypeScriptGeneratorSettings WithModuleName(this TypeScriptGeneratorSettings settings,
            string moduleName)
        {
            settings.ModuleName = moduleName;
            return settings;
        }
    }
}
