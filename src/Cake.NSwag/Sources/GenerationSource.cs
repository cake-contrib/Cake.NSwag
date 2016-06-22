using Cake.Core;
using Cake.Core.IO;

namespace Cake.NSwag.Sources
{
    /// <summary>
    ///     Base class for metadata sources
    /// </summary>
    public abstract class GenerationSource
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GenerationSource" /> class.
        /// </summary>
        /// <param name="sourceFilePath">Source file for the generation process</param>
        /// <param name="environment">The Cake evironment</param>
        /// <param name="fileSystem">The file system</param>
        protected GenerationSource(FilePath sourceFilePath, ICakeEnvironment environment, IFileSystem fileSystem)
        {
            Environment = environment;
            FileSystem = fileSystem;
            Source = sourceFilePath;
        }

        /// <summary>
        ///     The file system
        /// </summary>
        protected IFileSystem FileSystem { get; set; }

        /// <summary>
        ///     The Cake environment
        /// </summary>
        protected ICakeEnvironment Environment { get; set; }

        /// <summary>
        ///     Source file for API metadata.
        /// </summary>
        protected FilePath Source { get; set; }
    }
}