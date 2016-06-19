using System.Collections.Generic;

namespace Cake.NSwag.Settings
{
    public class CSharpGeneratorSettings
    {
        public List<string> Namespaces { get; set; } = new List<string>();
        public string BaseClass { get; set; }
        public bool GenerateInterfaces { get; set; }
    }

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