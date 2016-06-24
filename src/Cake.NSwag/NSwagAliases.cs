using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.NSwag
{
    /// <summary>
    ///     Alias to access the code generation functionality from the NSwag toolchain
    /// </summary>
    [CakeAliasCategory("Swagger")]
    public static class NSwagAliases
    {
        /// <summary>
        ///     Gets a runner to invoke NSwag operations
        /// </summary>
        /// <param name="ctx">The Cake context</param>
        /// <example>
        ///     <para> Generate a Swagger spec from a generic .NET assembly</para>
        ///     <code>
        /// <![CDATA[ NSwag.FromAssembly("./assembly.dll").ToSwaggerSpecification("./typespec.json"); ]]>
        /// </code>
        ///     <para>Generate a Swagger spec from an ASP.NET Web API assembly</para>
        ///     <code><![CDATA[
        /// NSwag.FromWebApiAssembly("./web.assembly.dll").ToSwaggerSpecification("./swagger.json");
        /// ]]></code>
        ///     <para>Generate a Typescript client from a JSON Schema file</para>
        ///     <code><![CDATA[NSwag.FromJsonSchema("./schema.json").ToTypeScriptClient("./client.ts");]]></code>
        ///     <para>Generate a C# client from a Swagger specification</para>
        ///     <code><![CDATA[NSwag.FromSwaggerSpecification("./swagger.json").ToCSharpClient("./client.cs", "Swagger.Client");]]></code>
        /// </example>
        [CakePropertyAlias]
        [CakeNamespaceImport("Cake.NSwag.Settings")]
        public static NSwagRunner NSwag(this ICakeContext ctx)
        {
            return new NSwagRunner(ctx.FileSystem, ctx.Environment, ctx.Log);
        }
    }
}