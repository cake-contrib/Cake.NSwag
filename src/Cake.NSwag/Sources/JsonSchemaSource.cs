using System.IO;
using Cake.Core;
using Cake.Core.IO;
using NJsonSchema;

namespace Cake.NSwag.Sources
{
    public class JsonSchemaSource : GenerationSource
    {
        public JsonSchemaSource(FilePath sourceFilePath, ICakeEnvironment env, IFileSystem fs) : base(sourceFilePath, env, fs)
        {
        }

        public void ToTypeScriptClient(FilePath outputFile)
        {
            var schema = JsonSchema4.FromJson(new StreamReader(FileSystem.GetFile(Source).OpenRead()).ReadToEnd());
            var generator = new NJsonSchema.CodeGeneration.TypeScript.TypeScriptGenerator(schema);
            var code = generator.GenerateFile();
            FileSystem.WriteContent(outputFile, code);
        }

        public void ToCSharpClient(FilePath outputFile)
        {
            var schema = JsonSchema4.FromJson(new StreamReader(FileSystem.GetFile(Source).OpenRead()).ReadToEnd());
            var generator = new NJsonSchema.CodeGeneration.CSharp.CSharpGenerator(schema);
            var code = generator.GenerateFile();
            FileSystem.WriteContent(outputFile, code);
        }
    }
}