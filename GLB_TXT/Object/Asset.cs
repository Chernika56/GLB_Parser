namespace GLB_TXT.Object
{
    /// <summary>
    /// Represents the asset information of a GLB (Binary GLTF) file, including the generator and version.
    /// </summary>
    public class Asset
    {
        /// <summary>
        /// Gets or sets the name or identifier of the software that generated the GLB file.
        /// </summary>
        public string generator { get; set; }

        /// <summary>
        /// Gets or sets the version of the GLTF (GL Transmission Format) used in the GLB file.
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Asset"/> class with specified generator and version.
        /// </summary>
        /// <param name="generator">The name or identifier of the software that generated the GLB file.</param>
        /// <param name="version">The version of the GLTF used in the GLB file.</param>
        public Asset(string generator, string version)
        {
            this.generator = generator;
            this.version = version;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Asset"/> class with specified version.
        /// </summary>
        /// <param name="version">The version of the GLTF used in the GLB file.</param>
        public Asset(string version)
        {
            this.version = version;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Asset"/> class.
        /// </summary>
        public Asset() { }
    }
}
