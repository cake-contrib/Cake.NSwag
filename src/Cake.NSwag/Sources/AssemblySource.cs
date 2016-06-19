using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using NJsonSchema;
using NSwag.CodeGeneration.SwaggerGenerators;
using NSwag.CodeGeneration.SwaggerGenerators.WebApi;
using Path = Cake.Core.IO.Path;

namespace Cake.NSwag.Sources
{
    public enum AssemblyMode
    {
        Normal,
        WebApi
    }
    public class AssemblySource : GenerationSource
    {
        internal AssemblySource(FilePath assemblyPath, ICakeEnvironment environment, IFileSystem fileSystem, bool useWebApi) : base(assemblyPath)
        {
            Environment = environment;
            FileSystem = fileSystem;
            Mode = useWebApi ? AssemblyMode.WebApi : AssemblyMode.Normal;
        }

        private AssemblyMode Mode { get; set; }

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
                ReferencePaths = settings.AssemblyPaths.Select(a => a.FullPath).ToArray(),
            };
            var gen = new WebApiAssemblyToSwaggerGenerator(genSettings);
            var service = gen.GenerateForControllers(gen.GetControllerClasses());
            using (var stream = new StreamWriter(FileSystem.GetFile(outputFile).OpenWrite()))
            {
                stream.WriteAsync(service.ToJson());
            }
        }

        public AssemblySource ToSwaggerDefinition(FilePath outputFile, Action<SwaggerGeneratorSettings> configure = null)
        {
            var settings = new SwaggerGeneratorSettings();
            configure?.Invoke(settings);
            ToSwaggerDefinition(outputFile, settings);
            return this;
        }
    }

    public class SwaggerGeneratorSettings
    {
        public string DefaultUrlTemplate { get; set; } = "api/{controller}/{action}/{id}";
        public bool EnumAsString { get; set; }
        public bool CamelCaseProperties { get; set; }
        public IEnumerable<Path> AssemblyPaths { get; set; }
    }
}
