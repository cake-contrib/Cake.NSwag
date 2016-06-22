using System;
using System.IO;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.NSwag.Settings;
using NJsonSchema;
using NSwag.CodeGeneration.SwaggerGenerators;
using NSwag.CodeGeneration.SwaggerGenerators.WebApi;

namespace Cake.NSwag.Sources
{
    /// <summary>
    ///     Represents a .NET assembly (conventional or Web API) to gather API metadata from.
    /// </summary>
    public class AssemblySource : GenerationSource
    {
        internal AssemblySource(FilePath assemblyPath, ICakeEnvironment environment, IFileSystem fileSystem,
            bool useWebApi)
            : base(assemblyPath, environment, fileSystem)
        {
            Mode = useWebApi ? AssemblyMode.WebApi : AssemblyMode.Normal;
        }

        private AssemblyMode Mode { get; }

        /// <summary>
        ///     Generates a Swagger (Open API) specification at the given path using the specified settings
        /// </summary>
        /// <param name="outputFile">File path for the generated API specification</param>
        /// <param name="settings">Settings to further control the spec generation process</param>
        /// <returns>The metadata source</returns>
        /// <example>
        ///     <code><![CDATA[NSwag.FromAssembly("./assembly.dll").ToSwaggerDefinition("./swagger.json");]]></code>
        ///     <code><![CDATA[NSwag.FromWebApiAssembly("./apicontroller.dll").ToSwaggerDefinition("./swagger.json");]]></code>
        /// </example>
        public AssemblySource ToSwaggerDefinition(FilePath outputFile, SwaggerGeneratorSettings settings)
        {
            settings = settings ?? new SwaggerGeneratorSettings();
            if (Mode == AssemblyMode.Normal)
            {
                GenerateTypeSwagger(outputFile, settings);
            }
            else
            {
                GenerateWebApiSwagger(outputFile, settings);
            }
            return this;
        }

        private void GenerateTypeSwagger(FilePath outputFile, SwaggerGeneratorSettings settings)
        {
            var genSettings = new AssemblyTypeToSwaggerGeneratorSettings
            {
                AssemblyPath = Source.MakeAbsolute(Environment).FullPath,
                DefaultEnumHandling = settings.EnumAsString ? EnumHandling.String : EnumHandling.Integer,
                DefaultPropertyNameHandling = settings.CamelCaseProperties
                    ? PropertyNameHandling.CamelCase
                    : PropertyNameHandling.Default,
                NullHandling = NullHandling.Swagger,
                ReferencePaths = settings.AssemblyPaths.Select(a => a.FullPath).ToArray()
            };
            var gen = new AssemblyTypeToSwaggerGenerator(genSettings);
            var service = gen.Generate(gen.GetClasses());
            using (var stream = new StreamWriter(FileSystem.GetFile(outputFile).OpenWrite()))
            {
                stream.WriteAsync(service.ToJson());
            }
        }

        private void GenerateWebApiSwagger(FilePath outputFile, SwaggerGeneratorSettings settings)
        {
            var genSettings = new WebApiAssemblyToSwaggerGeneratorSettings
            {
                AssemblyPath = Source.MakeAbsolute(Environment).FullPath,
                DefaultUrlTemplate = settings.DefaultUrlTemplate,
                DefaultEnumHandling = settings.EnumAsString ? EnumHandling.String : EnumHandling.Integer,
                DefaultPropertyNameHandling = settings.CamelCaseProperties
                    ? PropertyNameHandling.CamelCase
                    : PropertyNameHandling.Default,
                NullHandling = NullHandling.Swagger,
                ReferencePaths = settings.AssemblyPaths.Select(a => a.FullPath).ToArray()
            };
            var gen = new WebApiAssemblyToSwaggerGenerator(genSettings);
            var service = gen.GenerateForControllers(gen.GetControllerClasses());
            using (var stream = new StreamWriter(FileSystem.GetFile(outputFile).OpenWrite()))
            {
                stream.WriteAsync(service.ToJson());
            }
        }

        /// <summary>
        ///     Generates a Swagger (Open API) specification at the given path using the specified settings
        /// </summary>
        /// <param name="outputFile">File path for the generated API specification</param>
        /// <param name="configure">Optional settings to further control the specification</param>
        /// <returns>The metadata source</returns>
        /// <example>
        ///     <code><![CDATA[NSwag.FromWebApiAssembly("./apicontroller.dll").ToSwaggerDefinition("./api.json");]]></code>
        /// </example>
        public AssemblySource ToSwaggerDefinition(FilePath outputFile, Action<SwaggerGeneratorSettings> configure = null)
        {
            var settings = new SwaggerGeneratorSettings();
            configure?.Invoke(settings);
            ToSwaggerDefinition(outputFile, settings);
            return this;
        }
    }
}