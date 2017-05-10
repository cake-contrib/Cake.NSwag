using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.TypeScript;
using NSwag.SwaggerGeneration;
using NSwag.SwaggerGeneration.WebApi;

namespace Cake.NSwag.Settings
{
    internal static class SettingsFactory
    {
        private static SettingsContainer Container { get; } = new SettingsContainer();

        internal static SwaggerToCSharpClientGeneratorSettings GetSwaggerToCSharpSettings()
        {
            return Container.SwaggerToCSharpClientGeneratorSettings;
        }

        internal static SwaggerToTypeScriptClientGeneratorSettings GetSwaggerToTypeScriptSettings()
        {
            return Container.SwaggerToTypeScriptClientGeneratorSettings;
        }

        internal static SwaggerToCSharpWebApiControllerGeneratorSettings GetSwaggerToControllerSettings()
        {
            return Container.SwaggerToCSharpWebApiControllerGeneratorSettings;
        }

        internal static WebApiAssemblyToSwaggerGeneratorSettings GetWebApiToSwaggerSettings()
        {
            return Container.WebApiAssemblyToSwaggerGeneratorSettings;
        }

        internal static AssemblyTypeToSwaggerGeneratorSettings GetAssemblyToSwaggerSettings()
        {
            return Container.AssemblyTypeToSwaggerGeneratorSettings;
        }
    }
}