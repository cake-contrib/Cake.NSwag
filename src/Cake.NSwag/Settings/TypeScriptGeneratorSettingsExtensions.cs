using System.Diagnostics;

namespace Cake.NSwag.Settings
{
    /// <summary>
    ///     Fluent extension methods for the the <see cref="TypeScriptGeneratorSettings" /> class
    /// </summary>
    [DebuggerStepThrough]
    public static class TypeScriptGeneratorSettingsExtensions
    {
        /// <summary>
        ///     Sets the class name for the genertated client
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="className">A valid TypeScript class name for the generated client</param>
        /// <returns>The updated settings object</returns>
        public static TypeScriptGeneratorSettings WithClassName(this TypeScriptGeneratorSettings settings,
            string className)
        {
            settings.ClassName = className;
            return settings;
        }

        /// <summary>
        ///     Sets the module name for the generated client
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="moduleName">A valid TypeScript module name for the generated module</param>
        /// <returns>The updated settings object</returns>
        public static TypeScriptGeneratorSettings WithModuleName(this TypeScriptGeneratorSettings settings,
            string moduleName)
        {
            settings.ModuleName = moduleName;
            return settings;
        }
    }
}