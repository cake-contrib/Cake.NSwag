using System.Diagnostics;

namespace Cake.NSwag.Settings
{
    [DebuggerStepThrough]
    public static class CSharpGeneratorSettingsExtensions
    {
        public static CSharpGeneratorSettings AddNamespace(this CSharpGeneratorSettings settings, string ns)
        {
            settings.Namespaces.Add(ns);
            return settings;
        }

        public static CSharpGeneratorSettings SetBaseClass(this CSharpGeneratorSettings settings, string baseClassName)
        {
            settings.BaseClass = baseClassName;
            return settings;
        }

        public static CSharpGeneratorSettings EnableInterfaces(this CSharpGeneratorSettings settings)
        {
            settings.GenerateInterfaces = true;
            return settings;
        }
    }
}