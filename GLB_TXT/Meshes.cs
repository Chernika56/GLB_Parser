namespace GLB_TXT
{
    /// <summary>
    /// Represents a mesh in a GLB (Binary GLTF) file, containing a name and a list of primitives.
    /// </summary>
    public class Meshes
    {
        /// <summary>
        /// Gets or sets the name of the mesh.
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// Gets or sets the list of primitives that make up the mesh.
        /// </summary>
        public List<Primitives> primitives { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Meshes"/> class with specified rendering mode, position index,
        /// normal index, indices index, and material index.
        /// </summary>
        /// <param name="mode">The rendering mode of the primitives.</param>
        /// <param name="position">The index of the POSITION attribute accessor.</param>
        /// <param name="normal">The index of the NORMAL attribute accessor.</param>
        /// <param name="indices">The index of the indices accessor.</param>
        /// <param name="material">The index of the material used by the primitives.</param>
        public Meshes(int mode, int position, int normal, int indices, int material)
        {
            primitives = new List<Primitives>();
            primitives.Add(new Primitives(mode, position, normal, indices, material));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Meshes"/> class with specified rendering mode, position index,
        /// normal index, and indices index.
        /// </summary>
        /// <param name="mode">The rendering mode of the primitives.</param>
        /// <param name="position">The index of the POSITION attribute accessor.</param>
        /// <param name="normal">The index of the NORMAL attribute accessor.</param>
        /// <param name="indices">The index of the indices accessor.</param>
        public Meshes(int mode, int position, int normal, int indices)
        {
            primitives = new List<Primitives>();
            primitives.Add(new Primitives(mode, position, normal, indices));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Meshes"/> class with specified rendering mode, position index,
        /// and normal index.
        /// </summary>
        /// <param name="mode">The rendering mode of the primitives.</param>
        /// <param name="position">The index of the POSITION attribute accessor.</param>
        /// <param name="normal">The index of the NORMAL attribute accessor.</param>
        public Meshes(int mode, int position, int normal)
        {
            primitives = new List<Primitives>();
            primitives.Add(new Primitives(mode, position, normal));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Meshes"/> class with specified name, rendering mode, position index,
        /// normal index, indices index, and material index.
        /// </summary>
        /// <param name="name">The name of the mesh.</param>
        /// <param name="mode">The rendering mode of the primitives.</param>
        /// <param name="position">The index of the POSITION attribute accessor.</param>
        /// <param name="normal">The index of the NORMAL attribute accessor.</param>
        /// <param name="indices">The index of the indices accessor.</param>
        /// <param name="material">The index of the material used by the primitives.</param>
        public Meshes(string name, int mode, int position, int normal, int indices, int material)
        {
            this.name = name;
            primitives = new List<Primitives>();
            primitives.Add(new Primitives(mode, position, normal, indices, material));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Meshes"/> class.
        /// </summary>
        public Meshes() { }
    }
}
