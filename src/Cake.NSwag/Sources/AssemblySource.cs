using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.IO;
using Cake.NSwag.Settings;
using NJsonSchema;
using NSwag.SwaggerGeneration;
using NSwag.SwaggerGeneration.WebApi;

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
        ///     <code><![CDATA[NSwag.FromAssembly("./assembly.dll").ToSwaggerSpecification("./swagger.json");]]></code>
        ///     <code><![CDATA[NSwag.FromWebApiAssembly("./apicontroller.dll").ToSwaggerSpecification("./swagger.json");]]></code>
        /// </example>
        public AssemblySource ToSwaggerSpecification(FilePath outputFile, SwaggerGeneratorSettings settings)
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

        /// <summary>
        ///     Generates a Swagger (Open API) specification at the given path using the specified settings
        /// </summary>
        /// <param name="outputFile">File path for the generated API specification</param>
        /// <param name="configure">Optional settings to further control the specification</param>
        /// <returns>The metadata source</returns>
        /// <example>
        ///     <code><![CDATA[NSwag.FromWebApiAssembly("./apicontroller.dll").ToSwaggerSpecification("./api.json");]]></code>
        /// </example>
        public AssemblySource ToSwaggerSpecification(FilePath outputFile, Action<SwaggerGeneratorSettings> configure = null)
        {
            var settings = new SwaggerGeneratorSettings();
            configure?.Invoke(settings);
            ToSwaggerSpecification(outputFile, settings);
            return this;
        }

        private void GenerateTypeSwagger(FilePath outputFile, SwaggerGeneratorSettings settings)
        {
            var genSettings = settings.JsonSettings as AssemblyTypeToSwaggerGeneratorSettings ??
                              SettingsFactory.GetAssemblyToSwaggerSettings();
            genSettings.AssemblyPath = Source.MakeAbsolute(Environment).FullPath;
            genSettings.DefaultEnumHandling = settings.EnumAsString ? EnumHandling.String : EnumHandling.Integer;
            genSettings.DefaultPropertyNameHandling = settings.CamelCaseProperties
                ? PropertyNameHandling.CamelCase
                : PropertyNameHandling.Default;
            genSettings.ReferencePaths = settings.AssemblyPaths.Select(a => a.FullPath).ToArray();
            var gen = new AssemblyTypeToSwaggerGenerator(genSettings);
            var service = gen.GenerateAsync(gen.GetExportedClassNames());
            using (var stream = new StreamWriter(FileSystem.GetFile(outputFile).OpenWrite()))
            {
                stream.WriteAsync(service.Result.ToJson());
            }
        }

        private void GenerateWebApiSwagger(FilePath outputFile, SwaggerGeneratorSettings settings)
        {
            var genSettings = settings.JsonSettings as WebApiAssemblyToSwaggerGeneratorSettings ??
                SettingsFactory.GetWebApiToSwaggerSettings();
            genSettings.AssemblyPaths = new [] {Source.MakeAbsolute(Environment).FullPath };
            genSettings.DefaultUrlTemplate = settings.DefaultUrlTemplate;
            genSettings.DefaultEnumHandling = settings.EnumAsString ? EnumHandling.String : EnumHandling.Integer;
            genSettings.DefaultPropertyNameHandling = settings.CamelCaseProperties
                ? PropertyNameHandling.CamelCase
                : PropertyNameHandling.Default;
            genSettings.NullHandling = NullHandling.Swagger;
            genSettings.ReferencePaths = settings.AssemblyPaths.Select(a => a.FullPath).ToArray();
            var gen = new WebApiAssemblyToSwaggerGenerator(genSettings);
            var service = gen.GenerateForControllersAsync(gen.GetControllerClasses()).Result;
            service.BasePath = settings.BasePath ?? "";
            service.Info.Title = settings.ApiTitle ?? "";
            service.Info.Description = settings.ApiDescription ?? "";
            using (var stream = new StreamWriter(FileSystem.GetFile(outputFile).OpenWrite()))
            {
                stream.WriteAsync(service.ToJson()).Wait();
            }
        }
    }
}