namespace GLB_TXT.Object
{
    /// <summary>
    /// Represents the metallic-roughness material properties in a GLB (Binary GLTF) file.
    /// </summary>
    public class MetallicRoughness
    {
        /// <summary>
        /// Gets or sets the base color factor, represented as a list of double values.
        /// </summary>
        public List<double>? baseColorFactor { get; set; }

        /// <summary>
        /// Gets or sets the metallic factor, indicating the degree of metallicity of the material.
        /// </summary>
        public double metallicFactor { get; set; }

        /// <summary>
        /// Gets or sets the roughness factor, indicating the microfacet roughness of the material.
        /// </summary>
        public double roughnessFactor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetallicRoughness"/> class with specified metallic and roughness factors.
        /// </summary>
        /// <param name="metallicFactor">The metallic factor indicating the degree of metallicity of the material.</param>
        /// <param name="roughnessFactor">The roughness factor indicating the microfacet roughness of the material.</param>
        public MetallicRoughness(double metallicFactor, double roughnessFactor)
        {
            this.metallicFactor = metallicFactor;
            this.roughnessFactor = roughnessFactor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetallicRoughness"/> class.
        /// </summary>
        public MetallicRoughness() { }
    }
}
