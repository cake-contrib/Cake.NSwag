using NJsonSchema;
using NSwag.CodeGeneration.CodeGenerators.CSharp;
using NSwag.CodeGeneration.CodeGenerators.TypeScript;
using NSwag.CodeGeneration.SwaggerGenerators;
using NSwag.CodeGeneration.SwaggerGenerators.WebApi;

namespace Cake.NSwag.Settings
{
    /// <summary>
    /// Container class for NSwag's operation settings objects
    /// </summary>
    internal class SettingsContainer
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