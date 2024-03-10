namespace GLB_TXT
{
    /// <summary>
    /// Represents a node in a GLB (Binary GLTF) file, containing information about the node's name and associated mesh.
    /// </summary>
    public class Nodes
    {
        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// Gets or sets the index of the mesh associated with the node.
        /// </summary>
        public int mesh { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Nodes"/> class with the specified name and mesh index.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="mesh">The index of the mesh associated with the node.</param>
        public Nodes(string name, int mesh)
        {
            this.name = name;
            this.mesh = mesh;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Nodes"/> class with the specified mesh index.
        /// </summary>
        /// <param name="mesh">The index of the mesh associated with the node.</param>
        public Nodes(int mesh)
        {
            this.mesh = mesh;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Nodes"/> class.
        /// </summary>
        public Nodes() { }
    }
}

