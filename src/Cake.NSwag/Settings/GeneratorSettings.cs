using NJsonSchema.Generation;
using NSwag.CodeGeneration;

namespace Cake.NSwag.Settings
{
    /// <summary>
    /// Base class for other generator settings objects to derive from
    /// </summary>
    /// <remarks>Provides operation-specific settings</remarks>
    public abstract class GeneratorSettings
    {
        /// <summary>
        /// Container for operation-specific settings used when generating client libraries.
        /// </summary>
        public ClientGeneratorBaseSettings ClientSettings { get; set; }

        /// <summary>
        /// Container for operation-specific settings used when generating controller code.
        /// </summary>
        public ControllerGeneratorBaseSettings ControllerSettings { get; set; }

        /// <summary>
        /// Container for operation-specific settings used when generating JSON schemas, including Swagger specifications.
        /// </summary>
        public JsonSchemaGeneratorSettings JsonSettings { get; set; }

        /// <summary>
        /// Specifies operation-specific settings for generating client code.
        /// </summary>
        /// <param name="settings">The NSwag settings to use</param>
        /// <returns>Updated settings object</returns>
        public GeneratorSettings WithSettings(ClientGeneratorBaseSettings settings)
        {
            ClientSettings = settings;
            return this;
        }

        /// <summary>
        /// Specifies operation-specific settings for generating JSON schemas, including Swagger specifications.
        /// </summary>
        /// <param name="settings">The NSwag settings to use</param>
        /// <returns>Updated settings object</returns>
        public GeneratorSettings WithSettings(JsonSchemaGeneratorSettings settings)
        {
            JsonSettings = settings;
            return this;
        }

        /// <summary>
        /// Specificies operation-specific settings for generating controller code
        /// </summary>
        /// <param name="settings">The NSwag settings to use</param>
        /// <returns>Updated settings object</returns>
        public GeneratorSettings WithSettings(ControllerGeneratorBaseSettings settings)
        {
            ControllerSettings = settings;
            return this;
        }
    }
}