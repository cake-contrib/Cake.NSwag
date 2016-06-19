using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;

namespace Cake.NSwag
{
    class Samples
    {
        void Run(ICakeContext ctx)
        {
            ctx.NSwag().FromAssembly("./assembly.dll").ToSwaggerDefinition("./swagger.json");
            ctx.NSwag().FromWebApiAssembly("./apicontroller.dll").ToSwaggerDefinition("./api.json");
            ctx.NSwag().FromJsonSchema("./schema.json").ToCSharpClient("./client.cs");
            ctx.NSwag().FromJsonSchema("./schema.json").ToTypeScriptClient("./client.ts");
            ctx.NSwag().FromSwaggerSpec("./swagger.json").ToCSharpClient("./client.cs", "Swagger.Client");
            ctx.NSwag().FromSwaggerSpec("./swagger.json").ToTypeScriptClient("./client.ts");
        }
    }
}
