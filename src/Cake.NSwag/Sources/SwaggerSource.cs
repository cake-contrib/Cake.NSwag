using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.NSwag.Settings;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.TypeScript;
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
        ///     <code><![CDATA[NSwag.FromSwaggerSpecification("./swagger.json").ToCSharpClient("./client.cs", "Swagger.Client");]]></code>
        /// </example>
        public SwaggerSource ToCSharpClient(FilePath outputFile, string fullClientPath,
            Action<CSharpGeneratorSettings> configure = null)
        {
            var settings = new CSharpGeneratorSettings();
            configure?.Invoke(settings);
            var @class = fullClientPath.SplitClassPath();
            var genSettings = settings.ClientSettings as SwaggerToCSharpClientGeneratorSettings ?? SettingsFactory.GetSwaggerToCSharpSettings();
            genSettings.ClassName = @class.Value;
            genSettings.CSharpGeneratorSettings.Namespace = @class.Key;
            genSettings.AdditionalNamespaceUsages = settings.Namespaces.ToArray();
            genSettings.ClientBaseClass = settings.BaseClass;
            genSettings.GenerateClientInterfaces = settings.GenerateInterfaces;
            genSettings.ExceptionClass = settings.ExceptionClass;
            var gen = new SwaggerToCSharpClientGenerator(Swag.SwaggerDocument.FromJsonAsync(FileSystem.ReadContent(Source)).Result,
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
        ///     <code><![CDATA[NSwag.FromSwaggerSpecification("./swagger.json").ToTypeScriptClient("./client.ts");]]></code>
        /// </example>
        public SwaggerSource ToTypeScriptClient(FilePath outputFile,
            Action<TypeScriptGeneratorSettings> configure = null)
        {
            var settings = new TypeScriptGeneratorSettings();
            configure?.Invoke(settings);
            var genSettings = settings.ClientSettings as SwaggerToTypeScriptClientGeneratorSettings ??
                SettingsFactory.GetSwaggerToTypeScriptSettings();
            genSettings.ClassName = settings.ClassName;
            if (!string.IsNullOrWhiteSpace(settings.ModuleName))
            {
                genSettings.TypeScriptGeneratorSettings.ModuleName = settings.ModuleName;
            }
            var service = Swag.SwaggerDocument.FromJsonAsync(FileSystem.ReadContent(Source)).Result;
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
        /// .FromSwaggerSpecification("./swagger.json")
        /// .ToWebApiController("./controller.cs", "Generated.Api.ValuesController")
        /// ]]></code>
        /// </example>
        public SwaggerSource ToWebApiController(FilePath outputFile, string classPath,
            Action<CSharpGeneratorSettings> configure = null)
        {
            var settings = new CSharpGeneratorSettings();
            configure?.Invoke(settings);
            var @class = classPath.SplitClassPath();
            var genSettings = settings.ClientSettings as SwaggerToCSharpWebApiControllerGeneratorSettings ??
                              SettingsFactory.GetSwaggerToControllerSettings();
            genSettings.ClassName = @class.Value;
            genSettings.CSharpGeneratorSettings.Namespace = @class.Key;
            genSettings.AdditionalNamespaceUsages = settings.Namespaces.ToArray();
            genSettings.ControllerBaseClass = settings.BaseClass;
            genSettings.GenerateClientClasses = true;
            genSettings.GenerateClientInterfaces = settings.GenerateInterfaces;
            var gen =
                new SwaggerToCSharpWebApiControllerGenerator(
                    Swag.SwaggerDocument.FromJsonAsync(FileSystem.ReadContent(Source)).Result, genSettings);
            var api = gen.GenerateFile();
            FileSystem.WriteContent(outputFile, api);
            return this;
        }
    }
}