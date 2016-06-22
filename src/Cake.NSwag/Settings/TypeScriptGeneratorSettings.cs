namespace Cake.NSwag.Settings
{
    /// <summary>
    ///     Settings to further control the generation of TypeScript code
    /// </summary>
    public class TypeScriptGeneratorSettings
    {
        internal TypeScriptGeneratorSettings()
        {
        }

        /// <summary>
        ///     The class name to use for generated client code
        /// </summary>
        public string ClassName { get; set; } = "Client";

        /// <summary>
        ///     The module name to use when generating a module
        /// </summary>
        public string ModuleName { get; set; }
    }
}