using System;
using System.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.NSwag.Sources;

namespace Cake.NSwag
{
    public class NSwagRunner
    {
        internal NSwagRunner(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            Environment = environment;
            FileSystem = fileSystem;
        }

        private ICakeEnvironment Environment { get; set; }
        private IFileSystem FileSystem { get; set; }

        public AssemblySource FromAssembly(FilePath assemblyPath)
        {
            return CreateAssemblySource(assemblyPath, false);
        }

        public AssemblySource FromWebApiAssembly(FilePath assemblyPath)
        {
            return CreateAssemblySource(assemblyPath, true);
        }

        private AssemblySource CreateAssemblySource(FilePath assemblyPath, bool useWebApi)
        {
            if (assemblyPath == null) throw new ArgumentNullException(nameof(assemblyPath));
            if (assemblyPath.HasExtension && !assemblyPath.GetExtension().Contains("dll"))
            {
                throw new ArgumentException($"The '{assemblyPath}' does not appear to be an assembly!", nameof(assemblyPath));
            }
            return new AssemblySource(assemblyPath, Environment, FileSystem, useWebApi);
        }

        public SwaggerSource FromSwaggerSpec(FilePath definitionFilePath)
        {
            if (definitionFilePath == null) throw new ArgumentNullException(nameof(definitionFilePath));
            if (!FileSystem.Exist(definitionFilePath))
            {
                throw new FileNotFoundException($"Could not find file '{definitionFilePath}", nameof(definitionFilePath));
            }

            return new SwaggerSource(definitionFilePath, Environment, FileSystem);
        }

        public JsonSchemaSource FromJsonSchema(FilePath definitionFilePath)
        {
            if (definitionFilePath == null) throw new ArgumentNullException(nameof(definitionFilePath));
            if (!FileSystem.Exist(definitionFilePath))
            {
                throw new FileNotFoundException($"Could not find file '{definitionFilePath}", nameof(definitionFilePath));
            }
            return new JsonSchemaSource(definitionFilePath, Environment, FileSystem);
        }
    }
}