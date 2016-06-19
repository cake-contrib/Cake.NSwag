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

        public string ClassName { get; set; } = "Client";
        public string ModuleName { get; set; }
    }
}
