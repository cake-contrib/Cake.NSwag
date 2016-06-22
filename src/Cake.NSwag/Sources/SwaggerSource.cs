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
    /// <summary>
    ///     Represents a Swagger (Open API) specification to gather metadata from
    /// </summary>
    public class SwaggerSource : GenerationSource
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SwaggerSource" /> class.
        /// </summary>
        /// <param name="sourceFilePath">Path to the assembly from which to extract metadata</param>
        /// <param name="environment">The Cake environment</param>
        /// <param name="fileSystem">The file system</param>
        /// <param name="log">The log</param>
        internal SwaggerSource(FilePath sourceFilePath, ICakeEnvironment environment, IFileSystem fileSystem,
            ICakeLog log)
            : base(sourceFilePath, environment, fileSystem)
        {
            Log = log;
        }

        private ICakeLog Log { get; set; }

        /// <summary>
        ///     Generates a C# API client at the given path with the specified settings
        /// </summary>
        /// <param name="outputFile">File path for the generated client code</param>
        /// <param name="fullClientPath">The fully qualified class name (including namespace) for the client</param>
        /// <param name="configure">Optional settings to further control the code generation process</param>
        /// <returns>The metadata source</returns>
        /// <example>
        ///     <code><![CDATA[NSwag.FromSwaggerSpec("./swagger.json").ToCSharpClient("./client.cs", "Swagger.Client");]]></code>
        /// </example>
        public SwaggerSource ToCSharpClient(FilePath outputFile, string fullClientPath,
            Action<CSharpGeneratorSettings> configure = null)
        {
            var settings = new CSharpGeneratorSettings();
            configure?.Invoke(settings);
            var @class = fullClientPath.SplitClassPath();
            var genSettings = new SwaggerToCSharpClientGeneratorSettings
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
            var gen = new SwaggerToCSharpClientGenerator(Swag.SwaggerService.FromJson(FileSystem.ReadContent(Source)),
                genSettings);
            var cs = gen.GenerateFile();
            FileSystem.WriteContent(outputFile, cs);
            return this;
        }

        /// <summary>
        ///     Generates a TypeScript API client at the given path with the specified settings
        /// </summary>
        /// <param name="outputFile">File path for the generated client code</param>
        /// <param name="configure">Optional settings to further control the code generation process</param>
        /// <returns>The metadata source</returns>
        /// <example>
        ///     <code><![CDATA[NSwag.FromSwaggerSpec("./swagger.json").ToTypeScriptClient("./client.ts");]]></code>
        /// </example>
        public SwaggerSource ToTypeScriptClient(FilePath outputFile,
            Action<TypeScriptGeneratorSettings> configure = null)
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
                    TypeScriptGeneratorSettings
                {
                    ModuleName = settings.ModuleName
                };
            }
            var service = Swag.SwaggerService.FromJson(FileSystem.ReadContent(Source));
            var gen = new SwaggerToTypeScriptClientGenerator(service, genSettings);
            var ts = gen.GenerateFile();
            FileSystem.WriteContent(outputFile, ts);
            return this;
        }

        /// <summary>
        ///     Generates a Web API controller class at the given path with the specified settings
        /// </summary>
        /// <param name="outputFile">File path for the generated client code</param>
        /// <param name="classPath">The fully qualified class name (including namespace) for the client</param>
        /// <param name="configure">Optional settings to further control the code generation process</param>
        /// <returns>The metadata source</returns>
        /// <example>
        ///     <code><![CDATA[
        /// NSwag
        /// .FromSwaggerSpec("./swagger.json")
        /// .ToWebApiController("./controller.cs", "Generated.Api.ValuesController")
        /// ]]></code>
        /// </example>
        public SwaggerSource ToWebApiController(FilePath outputFile, string classPath,
            Action<CSharpGeneratorSettings> configure = null)
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