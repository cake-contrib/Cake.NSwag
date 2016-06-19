using System.Diagnostics;

namespace Cake.NSwag.Settings
{
    [DebuggerStepThrough]
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