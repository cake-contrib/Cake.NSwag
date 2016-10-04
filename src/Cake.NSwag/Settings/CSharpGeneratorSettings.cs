using System.Collections.Generic;

namespace Cake.NSwag.Settings
{
    /// <summary>
    ///     Settings to further control the C# generation process
    /// </summary>
    public class CSharpGeneratorSettings : GeneratorSettings
    {
        internal CSharpGeneratorSettings()
        {
        }

        /// <summary>
        ///     Gets or sets a collection of additional namespace usages
        /// </summary>
        public List<string> Namespaces { get; set; } = new List<string>();

        /// <summary>
        ///     Gets or sets the base class to use for the generated client
        /// </summary>
        public string BaseClass { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to generate interfaces for the client code.
        /// </summary>
        public bool GenerateInterfaces { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating the class name for the generated client
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating the namespace for the generated client code
        /// </summary>
        public string Namespace { get; set; }
    }
}