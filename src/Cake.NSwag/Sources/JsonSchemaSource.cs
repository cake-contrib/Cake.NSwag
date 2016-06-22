using System.IO;
using Cake.Core;
using Cake.Core.IO;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using NJsonSchema.CodeGeneration.TypeScript;

namespace Cake.NSwag.Sources
{
    /// <summary>
    ///     Represents a JSON Schema to gather metadata from
    /// </summary>
    public class JsonSchemaSource : GenerationSource
    {
        internal JsonSchemaSource(FilePath sourceFilePath, ICakeEnvironment env, IFileSystem fs)
            : base(sourceFilePath, env, fs)
        {
        }

        /// <summary>
        ///     Generates a TypeScript client from the JSON Schema metadata
        /// </summary>
        /// <param name="outputFile">File path for the generated client code</param>
        /// <returns>The metadata source</returns>
        /// <example>
        ///     <code><![CDATA[NSwag.FromJsonSchema("./schema.json").ToTypeScriptClient("./client.ts");]]></code>
        /// </example>
        public JsonSchemaSource ToTypeScriptClient(FilePath outputFile)
        {
            var schema = JsonSchema4.FromJson(new StreamReader(FileSystem.GetFile(Source).OpenRead()).ReadToEnd());
            var generator = new TypeScriptGenerator(schema);
            var code = generator.GenerateFile();
            FileSystem.WriteContent(outputFile, code);
            return this;
        }

        /// <summary>
        ///     Genreates a C# client from the JSON Schema metadata
        /// </summary>
        /// <param name="outputFile">File path for the generated client code</param>
        /// <returns>The metadata source</returns>
        /// <example>
        ///     <code><![CDATA[NSwag.FromJsonSchema("./schema.json").ToCSharpClient("./client.cs");]]></code>
        /// </example>
        public JsonSchemaSource ToCSharpClient(FilePath outputFile)
        {
            var schema = JsonSchema4.FromJson(new StreamReader(FileSystem.GetFile(Source).OpenRead()).ReadToEnd());
            var generator = new CSharpGenerator(schema);
            var code = generator.GenerateFile();
            FileSystem.WriteContent(outputFile, code);
            return this;
        }
    }
}