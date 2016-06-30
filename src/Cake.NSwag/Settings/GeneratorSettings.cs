using NJsonSchema;
using NSwag.CodeGeneration.CodeGenerators.CSharp;
using NSwag.CodeGeneration.CodeGenerators.TypeScript;
using NSwag.CodeGeneration.SwaggerGenerators;
using NSwag.CodeGeneration.SwaggerGenerators.WebApi;

namespace Cake.NSwag.Settings
{
    /// <summary>
    /// Base class for other generator settings objects to derive from
    /// </summary>
    /// <remarks>Provides operation-specific settings</remarks>
    public abstract class GeneratorSettings
    {
        /// <summary>
        /// Container for operation-specific settings
        /// </summary>
        public SettingsContainer Settings { get; set; } = new SettingsContainer();

        /// <summary>
        /// Specifies operation-specific settings for generating C# from Swagger specifications
        /// </summary>
        /// <param name="settings">The NSwag settings to use</param>
        /// <returns>Updated settings object</returns>
        public GeneratorSettings WithSettings(SwaggerToCSharpClientGeneratorSettings settings)
        {
            Settings.SwaggerToCSharpClientGeneratorSettings = settings;
            return this;
        }

        /// <summary>
        /// Specificies operation-specific settings for generating Web API controllers from Swagger specifications
        /// </summary>
        /// <param name="settings">The NSwag settings to use</param>
        /// <returns>Updated settings object</returns>
        public GeneratorSettings WithSettings(SwaggerToCSharpWebApiControllerGeneratorSettings settings)
        {
            Settings.SwaggerToCSharpWebApiControllerGeneratorSettings = settings;
            return this;
        }

        /// <summary>
        /// Specifies operation-specific settings for generating TypeScript clients from Swagger specifications
        /// </summary>
        /// <param name="settings">The NSwag settings to use</param>
        /// <returns>Updated settings object</returns>
        public GeneratorSettings WithSettings(SwaggerToTypeScriptClientGeneratorSettings settings)
        {
            Settings.SwaggerToTypeScriptClientGeneratorSettings = settings;
            return this;
        }

        /// <summary>
        /// Specifies operation-specific settings for generating Swagger specifications from a Web API assembly
        /// </summary>
        /// <param name="settings">The NSwag settings to use</param>
        /// <returns>Updated settings object</returns>
        public GeneratorSettings WithSettings(WebApiAssemblyToSwaggerGeneratorSettings settings)
        {
            Settings.WebApiAssemblyToSwaggerGeneratorSettings = settings;
            return this;
        }

        /// <summary>
        /// Specifies operation-specific settings for generating Swagger specifications from a .NET assembly.
        /// </summary>
        /// <param name="settings">The NSwag settings to use</param>
        /// <returns>Updated settings object</returns>
        public GeneratorSettings WithSettings(AssemblyTypeToSwaggerGeneratorSettings settings)
        {
            Settings.AssemblyTypeToSwaggerGeneratorSettings = settings;
            return this;
        }
    }

    /// <summary>
    /// Container class for NSwag's operation settings objects
    /// </summary>
    public class SettingsContainer
    {
        /// <summary>
        /// Specifies operation-specific settings for generating C# from Swagger specifications
        /// </summary>
        public SwaggerToCSharpClientGeneratorSettings SwaggerToCSharpClientGeneratorSettings { get; set; } =
            new SwaggerToCSharpClientGeneratorSettings
            {
                GenerateClientClasses = true,
                CSharpGeneratorSettings =  new NJsonSchema.CodeGeneration.CSharp.CSharpGeneratorSettings
                {
                    ArrayType = "List",
                    NullHandling = NullHandling.Swagger
                }
            };

        /// <summary>
        /// Specificies operation-specific settings for generating Web API controllers from Swagger specifications
        /// </summary>
        public SwaggerToCSharpWebApiControllerGeneratorSettings SwaggerToCSharpWebApiControllerGeneratorSettings { get;
            set; } = new SwaggerToCSharpWebApiControllerGeneratorSettings
            {
                CSharpGeneratorSettings = new NJsonSchema.CodeGeneration.CSharp.CSharpGeneratorSettings
                {
                    ArrayType = "List",
                    NullHandling = NullHandling.Swagger
                }
            };

        /// <summary>
        /// Specifies operation-specific settings for generating TypeScript clients from Swagger specifications
        /// </summary>
        public SwaggerToTypeScriptClientGeneratorSettings SwaggerToTypeScriptClientGeneratorSettings { get; set; } =
            new SwaggerToTypeScriptClientGeneratorSettings
            {
                PromiseType = PromiseType.Promise,
                TypeScriptGeneratorSettings = new NJsonSchema.CodeGeneration.TypeScript.TypeScriptGeneratorSettings()
            };

        /// <summary>
        /// Specifies operation-specific settings for generating Swagger specifications from a Web API assembly
        /// </summary>
        public WebApiAssemblyToSwaggerGeneratorSettings WebApiAssemblyToSwaggerGeneratorSettings { get; set; } =
            new WebApiAssemblyToSwaggerGeneratorSettings();

        /// <summary>
        /// Specifies operation-specific settings for generating Swagger specifications from a .NET assembly.
        /// </summary>
        public AssemblyTypeToSwaggerGeneratorSettings AssemblyTypeToSwaggerGeneratorSettings { get; set; } =
            new AssemblyTypeToSwaggerGeneratorSettings {NullHandling = NullHandling.Swagger};
    }
}