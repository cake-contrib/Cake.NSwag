namespace Cake.NSwag.Sources
{
    /// <summary>
    ///     Specifies how an assembly should be treated when loading types
    /// </summary>
    internal enum AssemblyMode
    {
        /// <summary>
        ///     Assembly should be parsed for "normal" (POCO) objects
        /// </summary>
        Normal,

        /// <summary>
        ///     Assembly should be parsed for Web API Controllers
        /// </summary>
        WebApi
    }
}