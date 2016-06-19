using Cake.Core;
using Cake.Core.IO;

namespace Cake.NSwag.Sources
{
    public abstract class GenerationSource
    {
        public GenerationSource(FilePath sourceFilePath)
        {
            Source = sourceFilePath;
        }

        public GenerationSource(FilePath sourceFilePath, ICakeEnvironment environment, IFileSystem fileSystem) : this(sourceFilePath)
        {
            Environment = environment;
            FileSystem = fileSystem;
        }

        protected IFileSystem FileSystem { get; set; }

        protected ICakeEnvironment Environment { get; set; }

        protected FilePath Source { get; set; }
    }
}