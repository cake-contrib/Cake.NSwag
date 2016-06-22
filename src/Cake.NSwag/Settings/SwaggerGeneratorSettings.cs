using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;

namespace Cake.NSwag.Settings
{
    /// <summary>
    ///     Settings to control the creation of Swagger (Open API) specifications
    /// </summary>
    public class SwaggerGeneratorSettings
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
        public IEnumerable<Path> AssemblyPaths { get; set; }
    }

    /// <summary>
    ///     Fluent extensions methods for the <see cref="SwaggerGeneratorSettings" /> class
    /// </summary>
    public static class SwaggerGeneratorSettingsExtensions
    {
        /// <summary>
        ///     Sets the default URL template to be used when parsing routes
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="template">The url template to use as default</param>
        /// <returns>The updated settings object</returns>
        public static SwaggerGeneratorSettings UseUrlTemplate(this SwaggerGeneratorSettings settings, string template)
        {
            settings.DefaultUrlTemplate = template;
            return settings;
        }

        /// <summary>
        ///     Enables representing enums as strings in generated specifications
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <returns>The updated settings object</returns>
        public static SwaggerGeneratorSettings UseStringEnums(this SwaggerGeneratorSettings settings)
        {
            settings.EnumAsString = true;
            return settings;
        }

        /// <summary>
        ///     Enables representing enums as integers in generated specifications
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <returns>The updated settings object</returns>
        public static SwaggerGeneratorSettings UseIntegerEnums(this SwaggerGeneratorSettings settings)
        {
            settings.EnumAsString = false;
            return settings;
        }

        /// <summary>
        ///     Adds the given assemblies to the srach paths to gather additional metadata from.
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="assemblyPaths">Assemblies to search for metadata.</param>
        /// <returns>The updated settings object</returns>
        public static SwaggerGeneratorSettings SearchAssemblies(this SwaggerGeneratorSettings settings,
            params Path[] assemblyPaths)
        {
            var a = settings.AssemblyPaths as List<Path> ?? settings.AssemblyPaths.ToList();
            a.AddRange(assemblyPaths.ToList());
            settings.AssemblyPaths = a;
            return settings;
        }
    }
}