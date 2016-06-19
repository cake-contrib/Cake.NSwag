using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Tooling;

namespace Cake.NSwag
{
    [CakeAliasCategory("Swagger")]
    public static class NSwagAliases
    {
        [CakePropertyAlias]
        [CakeNamespaceImport("Cake.NSwag.Settings")]
        public static NSwagRunner NSwag(this ICakeContext ctx)
        {
            return new NSwagRunner(ctx.FileSystem, ctx.Environment, ctx.Log);
        }
    }
}
