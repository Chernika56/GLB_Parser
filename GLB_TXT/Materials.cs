namespace GLB_TXT
{
    /// <summary>
    /// Represents a material in a GLB (Binary GLTF) file, containing information about the material's name,
    /// double-sided property, and metallic-roughness properties.
    /// </summary>
    public class Materials
    {
        /// <summary>
        /// Gets or sets a value indicating whether the material is double-sided.
        /// </summary>
        public bool doubleSided { get; set; }

        /// <summary>
        /// Gets or sets the name of the material.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the metallic-roughness properties of the material.
        /// </summary>
        public MetallicRoughness pbrMetallicRoughness { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Materials"/> class with specified name, double-sided property,
        /// and metallic-roughness factors.
        /// </summary>
        /// <param name="name">The name of the material.</param>
        /// <param name="doubleSided">A value indicating whether the material is double-sided.</param>
        /// <param name="metallicFactor">The metallic factor indicating the degree of metallicity of the material.</param>
        /// <param name="roughnessFactor">The roughness factor indicating the microfacet roughness of the material.</param>
        public Materials(string name, bool doubleSided, double metallicFactor, double roughnessFactor)
        {
            this.name = name;
            this.doubleSided = doubleSided;
            pbrMetallicRoughness = new MetallicRoughness(metallicFactor, roughnessFactor);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Materials"/> class.
        /// </summary>
        public Materials() { }
    }
}