using System.Diagnostics;

namespace Cake.NSwag.Settings
{
    /// <summary>
    ///     Fluent extension methods for the <see cref="CSharpGeneratorSettings" /> class.
    /// </summary>
    [DebuggerStepThrough]
    public static class CSharpGeneratorSettingsExtensions
    {
        /// <summary>
        ///     Adds an additional namespace usage
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="ns">The namespace to add</param>
        /// <returns>The updated settings object</returns>
        public static CSharpGeneratorSettings AddNamespace(this CSharpGeneratorSettings settings, string ns)
        {
            settings.Namespaces.Add(ns);
            return settings;
        }

        /// <summary>
        ///     Sets the base class for the generated code
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="baseClassName">A valid C# class name to use as a base class</param>
        /// <returns>The updated settings object</returns>
        public static CSharpGeneratorSettings SetBaseClass(this CSharpGeneratorSettings settings, string baseClassName)
        {
            settings.BaseClass = baseClassName;
            return settings;
        }

        /// <summary>
        ///     Enables generating interfaces when generating the client
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <returns>The updated settings object</returns>
        public static CSharpGeneratorSettings EnableInterfaces(this CSharpGeneratorSettings settings)
        {
            settings.GenerateInterfaces = true;
            return settings;
        }
    }
}