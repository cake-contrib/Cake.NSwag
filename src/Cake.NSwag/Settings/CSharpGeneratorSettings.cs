using System.Collections.Generic;

namespace Cake.NSwag.Settings
{
    public class CSharpGeneratorSettings
    {
        internal CSharpGeneratorSettings()
        {
            
        }
        public List<string> Namespaces { get; set; } = new List<string>();
        public string BaseClass { get; set; }
        public bool GenerateInterfaces { get; set; }
    }
}