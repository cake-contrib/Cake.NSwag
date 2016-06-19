using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.NSwag.Settings;
using NJsonSchema;
using NSwag.CodeGeneration.CodeGenerators.CSharp;
using NSwag.CodeGeneration.CodeGenerators.TypeScript;
using Swag = NSwag;

namespace Cake.NSwag.Sources
{
    public class SwaggerSource : GenerationSource
    {
        public SwaggerSource(FilePath sourceFilePath, ICakeEnvironment environment, IFileSystem fileSystem, ICakeLog log)
            : base(sourceFilePath, environment, fileSystem)
        {
            Log = log;
        }

        private ICakeLog Log { get; set; }

        public SwaggerSource ToCSharpClient(FilePath outputFile, string fullClientPath, Action<CSharpGeneratorSettings> configure = null)
        {
            var settings = new CSharpGeneratorSettings();
            configure?.Invoke(settings);
            var @class = fullClientPath.SplitClassPath();
            var genSettings = new SwaggerToCSharpClientGeneratorSettings()
            {
                ClassName = @class.Value,
                CSharpGeneratorSettings = new NJsonSchema.CodeGeneration.CSharp.CSharpGeneratorSettings
                {
                    Namespace = @class.Key,
                    NullHandling = NullHandling.Swagger,
                    ArrayType = "List"
                },
                AdditionalNamespaceUsages = settings.Namespaces.ToArray(),
                ClientBaseClass = settings.BaseClass,
                GenerateClientClasses = true,
                GenerateClientInterfaces = settings.GenerateInterfaces
            };
            var gen = new SwaggerToCSharpClientGenerator(Swag.SwaggerService.FromJson(FileSystem.ReadContent(Source)), genSettings);
            var cs = gen.GenerateFile();
            FileSystem.WriteContent(outputFile, cs);
            return this;
        }

        public SwaggerSource ToTypeScriptClient(FilePath outputFile, Action<TypeScriptGeneratorSettings> configure = null)
        {
            var settings = new TypeScriptGeneratorSettings();
            configure?.Invoke(settings);
            var genSettings = new SwaggerToTypeScriptClientGeneratorSettings
            {
                ClassName = settings.ClassName,
                PromiseType = PromiseType.Promise
            };
            if (!string.IsNullOrWhiteSpace(settings.ModuleName))
            {
                genSettings.TypeScriptGeneratorSettings = new NJsonSchema.CodeGeneration.TypeScript.
                    TypeScriptGeneratorSettings()
                {
                    ModuleName = settings.ModuleName,
                };
            }
            var service = Swag.SwaggerService.FromJson(FileSystem.ReadContent(Source));
            var gen = new SwaggerToTypeScriptClientGenerator(service, genSettings);
            var ts = gen.GenerateFile();
            FileSystem.WriteContent(outputFile, ts);
            return this;
        }

        public SwaggerSource ToWebApiController(FilePath outputFile, string classPath, Action<CSharpGeneratorSettings> configure = null)
        {
            var settings = new CSharpGeneratorSettings();
            configure?.Invoke(settings);
            var @class = classPath.SplitClassPath();
            var genSettings = new SwaggerToCSharpWebApiControllerGeneratorSettings
            {
                ClassName = @class.Value,
                CSharpGeneratorSettings = new NJsonSchema.CodeGeneration.CSharp.CSharpGeneratorSettings
                {
                    Namespace = @class.Key,
                    NullHandling = NullHandling.Swagger,
                    ArrayType = "List"
                },
                AdditionalNamespaceUsages = settings.Namespaces.ToArray(),
                ControllerBaseClass = settings.BaseClass,
                GenerateClientClasses = true,
                GenerateClientInterfaces = settings.GenerateInterfaces
            };
            var gen =
                new SwaggerToCSharpWebApiControllerGenerator(
                    Swag.SwaggerService.FromJson(FileSystem.ReadContent(Source)), genSettings);
            var api = gen.GenerateFile();
            FileSystem.WriteContent(outputFile, api);
            return this;
        }
    }
}