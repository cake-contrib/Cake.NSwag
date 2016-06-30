using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.NSwag.Settings
{
    /// <summary>
    ///     Settings to control the creation of Swagger (Open API) specifications
    /// </summary>
    public class SwaggerGeneratorSettings : GeneratorSettings
    {
        /// <summary>
        ///     Default URL template to be used when parsing routes
        /// </summary>
        /// <remarks>Defaults to <c>api/{controller}/{action}/{id}</c></remarks>
        public string DefaultUrlTemplate { get; set; } = "api/{controller}/{action}/{id}";

        /// <summary>
        ///     Gets or sets a value indicating whether to represent enums as strings
        /// </summary>
        /// <value><c>true</c> to represent as strings or <c>false</c> to represent as integers</value>
        public bool EnumAsString { get; set; } = true;

        /// <summary>
        ///     Gets or sets a value indicating whether to represent properties in camel case in generated code.
        /// </summary>
        public bool CamelCaseProperties { get; set; }

        /// <summary>
        ///     Additional assembly paths to search for references when generating metadata
        /// </summary>
        public IEnumerable<Path> AssemblyPaths { get; set; } = new List<Path>();

        /// <summary>
        ///     Gets or sets the API title for the generated defintion
        /// </summary>
        public string ApiTitle { get; set; }

        /// <summary>
        ///     Gets or sets the API description for the generated specification
        /// </summary>
        public string ApiDescription { get; set; }

        /// <summary>
        ///     Gets or sets the base path for the API specification
        /// </summary>
        public string BasePath { get; set; }
    }
}