using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.NSwag.Settings;
using NSwag.CodeGeneration.TypeScript;

namespace Cake.NSwag
{
    class Samples
    {
        void Run(ICakeContext ctx)
        {
            ctx.NSwag().FromAssembly("./assembly.dll").ToSwaggerSpecification("./swagger.json");
            ctx.NSwag().FromWebApiAssembly("./apicontroller.dll").ToSwaggerSpecification("./api.json");
            ctx.NSwag().FromWebApiAssembly("./apicontroller.dll")
                .ToSwaggerSpecification("./api.json", s =>
                    s.SearchAssemblies("./reference.dll")
                        .UseBasePath("api")
                        .UseStringEnums()
                        .WithTitle("Sample API"));
            ctx.NSwag().FromJsonSchema("./schema.json").ToCSharpClient("./client.cs");
            ctx.NSwag().FromJsonSchema("./schema.json").ToTypeScriptClient("./client.ts");
            ctx.NSwag().FromSwaggerSpecification("./swagger.json").ToCSharpClient("./client.cs", "Swagger.Client");
            ctx.NSwag().FromSwaggerSpecification("./swagger.json").ToTypeScriptClient("./client.ts");
            ctx.NSwag()
                .FromSwaggerSpecification("./swagger.json")
                .ToTypeScriptClient("./client.ts", s =>
                    s.WithSettings(new SwaggerToTypeScriptClientGeneratorSettings
                    {
                        Template = TypeScriptTemplate.Angular2,
                        PromiseType = PromiseType.Promise
                    }));
            ctx.NSwag()
                .FromSwaggerSpecification("./swagger.json")
                .ToTypeScriptClient("./client.ts", s =>
                    s.WithClassName("ApiClient")
                        .WithModuleName("SwaggerApi")
                        .WithSettings(new SwaggerToTypeScriptClientGeneratorSettings
                        {
                            PromiseType = PromiseType.Promise
                        }));
            ctx.NSwag()
                .FromSwaggerSpecification("./swagger.json")
                .ToWebApiController("./controller.cs", "Generated.Api.ValuesController");
        }
    }
}
