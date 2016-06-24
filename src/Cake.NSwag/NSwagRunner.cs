using System;
using System.IO;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.NSwag.Sources;

namespace Cake.NSwag
{
    /// <summary>
    ///     Exposes API metadata operations for generating specifications and client code.
    /// </summary>
    public class NSwagRunner
    {
        internal NSwagRunner(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
        {
            Environment = environment;
            FileSystem = fileSystem;
            Log = log;
        }

        private ICakeLog Log { get; }
        private ICakeEnvironment Environment { get; }
        private IFileSystem FileSystem { get; }

        /// <summary>
        ///     Parses a plain .NET assembly for metadata
        /// </summary>
        /// <param name="assemblyPath">Path to the assembly to load</param>
        /// <returns>A metadata source for the given assembly</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <c>assemblyPath</c> is null</exception>
        /// <exception cref="FileNotFoundException">Thrown when the assembly could not be found on the file system</exception>
        /// <exception cref="ArgumentException">Thrown when the given path doesn't appear to be an assembly.</exception>
        public AssemblySource FromAssembly(FilePath assemblyPath)
        {
            return CreateAssemblySource(assemblyPath, false);
        }

        /// <summary>
        ///     Parses an ASP.NET Web API assembly for API metadata
        /// </summary>
        /// <param name="assemblyPath">Path to the API assembly to load</param>
        /// <returns>A metadata source for the given assembly</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <c>assemblyPath</c> is null</exception>
        /// <exception cref="FileNotFoundException">Thrown when the assembly could not be found on the file system</exception>
        /// <exception cref="ArgumentException">Thrown when the given path doesn't appear to be an assembly.</exception>
        public AssemblySource FromWebApiAssembly(FilePath assemblyPath)
        {
            return CreateAssemblySource(assemblyPath, true);
        }

        private AssemblySource CreateAssemblySource(FilePath assemblyPath, bool useWebApi)
        {
            if (assemblyPath == null) throw new ArgumentNullException(nameof(assemblyPath));
            if (!FileSystem.Exist(assemblyPath))
            {
                throw new FileNotFoundException($"Could not find file '{assemblyPath}", nameof(assemblyPath));
            }
            if (assemblyPath.HasExtension && !assemblyPath.GetExtension().Contains("dll"))
            {
                throw new ArgumentException($"The '{assemblyPath}' does not appear to be an assembly!",
                    nameof(assemblyPath));
            }
            return new AssemblySource(assemblyPath, Environment, FileSystem, useWebApi);
        }

        /// <summary>
        ///     Parses a Swagger (Open API) specification for API metadata
        /// </summary>
        /// <param name="specificationFilePath">Path to the JSON definition file</param>
        /// <returns>A metadata source for the given API spec</returns>
        /// <exception cref="ArgumentNullException">Thrown if the definition file is not provided</exception>
        /// <exception cref="FileNotFoundException">Thrown if the definition file is not found on the file system</exception>
        public SwaggerSource FromSwaggerSpecification(FilePath specificationFilePath)
        {
            if (specificationFilePath == null) throw new ArgumentNullException(nameof(specificationFilePath));
            if (!FileSystem.Exist(specificationFilePath))
            {
                throw new FileNotFoundException($"Could not find file '{specificationFilePath}", nameof(specificationFilePath));
            }

            return new SwaggerSource(specificationFilePath, Environment, FileSystem, Log);
        }

        /// <summary>
        ///     Parses a JSON Schema file for API metadata
        /// </summary>
        /// <param name="definitionFilePath">Path to the JSON Schema file</param>
        /// <returns>A metadata source for the given schema</returns>
        /// <exception cref="ArgumentNullException">Thrown if <c>specificationFilePath</c> is null</exception>
        /// <exception cref="FileNotFoundException">Thrown if the schema file is not found on the file system</exception>
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