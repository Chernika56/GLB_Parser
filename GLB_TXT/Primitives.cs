namespace GLB_TXT
{
    /// <summary>
    /// Represents the primitives of a mesh in a GLB (Binary GLTF) file, containing information about
    /// attributes, indices, material, and rendering mode.
    /// </summary>
    public class Primitives
    {
        /// <summary>
        /// Gets or sets a dictionary of attributes associated with their corresponding indices.
        /// </summary>
        public Dictionary<string, int> attributes { get; set; }

        /// <summary>
        /// Gets or sets the index of the indices accessor.
        /// </summary>
        public int indices { get; set; }

        /// <summary>
        /// Gets or sets the index of the material used by the primitives.
        /// </summary>
        public int material { get; set; }

        /// <summary>
        /// Gets or sets the rendering mode of the primitives.
        /// </summary>
        public int mode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Primitives"/> class with specified rendering mode, position index,
        /// normal index, indices index, and material index.
        /// </summary>
        /// <param name="mode">The rendering mode of the primitives.</param>
        /// <param name="position">The index of the POSITION attribute accessor.</param>
        /// <param name="normal">The index of the NORMAL attribute accessor.</param>
        /// <param name="indices">The index of the indices accessor.</param>
        /// <param name="material">The index of the material used by the primitives.</param>
        public Primitives(int mode, int position, int normal, int indices, int material)
        {
            attributes = new Dictionary<string, int>();
            attributes.Add("POSITION", position);
            attributes.Add("NORMAL", normal);

            this.indices = indices;
            this.mode = mode;
            this.material = material;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Primitives"/> class with specified rendering mode, position index,
        /// normal index, and indices index.
        /// </summary>
        /// <param name="mode">The rendering mode of the primitives.</param>
        /// <param name="position">The index of the POSITION attribute accessor.</param>
        /// <param name="normal">The index of the NORMAL attribute accessor.</param>
        /// <param name="indices">The index of the indices accessor.</param>
        public Primitives(int mode, int position, int normal, int indices)
        {
            attributes = new Dictionary<string, int>();
            attributes.Add("POSITION", position);
            attributes.Add("NORMAL", normal);

            this.indices = indices;
            this.mode = mode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Primitives"/> class with specified rendering mode, position index,
        /// and normal index.
        /// </summary>
        /// <param name="mode">The rendering mode of the primitives.</param>
        /// <param name="position">The index of the POSITION attribute accessor.</param>
        /// <param name="normal">The index of the NORMAL attribute accessor.</param>
        public Primitives(int mode, int position, int normal)
        {
            attributes = new Dictionary<string, int>();
            attributes.Add("POSITION", position);
            attributes.Add("NORMAL", normal);

            this.mode = mode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Primitives"/> class.
        /// </summary>
        public Primitives() { }
    }
}
