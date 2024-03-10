namespace GLB_TXT
{
    /// <summary>
    /// Represents the root object of a GLB (Binary GLTF) file, containing information about scenes, nodes, meshes, accessors, buffer views, and buffers.
    /// </summary>
    public class RootObject
    {
        /// <summary>
        /// Gets or sets the asset information of the GLB file.
        /// </summary>
        public Asset asset { get; set; }

        /// <summary>
        /// Gets or sets the active scene index.
        /// </summary>
        public int scene { get; set; }

        /// <summary>
        /// Gets or sets the list of scenes in the GLB file.
        /// </summary>
        public List<Scenes> scenes { get; set; }

        /// <summary>
        /// Gets or sets the list of nodes in the GLB file.
        /// </summary>
        public List<Nodes> nodes { get; set; }

        /// <summary>
        /// Gets or sets the list of materials in the GLB file.
        /// </summary>
        public List<Materials> materials { get; set; }

        /// <summary>
        /// Gets or sets the list of meshes in the GLB file.
        /// </summary>
        public List<Meshes> meshes { get; set; }

        /// <summary>
        /// Gets or sets the list of accessors in the GLB file.
        /// </summary>
        public List<Accessors> accessors { get; set; }

        /// <summary>
        /// Gets or sets the list of buffer views in the GLB file.
        /// </summary>
        public List<BufferViews> bufferViews { get; set; }

        /// <summary>
        /// Gets or sets the list of buffers in the GLB file.
        /// </summary>
        public List<Buffers> buffers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootObject"/> class with specified scene, nodes, meshes, accessors, buffer views, and buffer length.
        /// </summary>
        /// <param name="scenes">The list of scenes in the GLB file.</param>
        /// <param name="nodes">The list of nodes in the GLB file.</param>
        /// <param name="meshes">The list of meshes in the GLB file.</param>
        /// <param name="accessors">The list of accessors in the GLB file.</param>
        /// <param name="bufferViews">The list of buffer views in the GLB file.</param>
        /// <param name="buffersLength">The length of the buffer in the GLB file.</param>
        public RootObject(List<Scenes> scenes, List<Nodes> nodes, List<Meshes> meshes,
            List<Accessors> accessors, List<BufferViews> bufferViews, uint buffersLength)
        {
            asset = new Asset("2.0");
            scene = 0;

            this.scenes = scenes;

            this.nodes = nodes;

            materials = new List<Materials>();
            materials.Add(new Materials("Material", true, 0.5, 0.5));

            this.meshes = meshes;

            this.accessors = accessors;

            this.bufferViews = bufferViews;

            this.buffers = new List<Buffers>();
            this.buffers.Add(new Buffers(buffersLength));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootObject"/> class with specified materials, scenes, nodes, meshes, accessors, buffer views, and buffer length.
        /// </summary>
        /// <param name="materials">The list of materials in the GLB file.</param>
        /// <param name="scenes">The list of scenes in the GLB file.</param>
        /// <param name="nodes">The list of nodes in the GLB file.</param>
        /// <param name="meshes">The list of meshes in the GLB file.</param>
        /// <param name="accessors">The list of accessors in the GLB file.</param>
        /// <param name="bufferViews">The list of buffer views in the GLB file.</param>
        /// <param name="buffersLength">The length of the buffer in the GLB file.</param>
        public RootObject(List<Materials> materials, List<Scenes> scenes, List<Nodes> nodes,
            List<Meshes> meshes, List<Accessors> accessors, List<BufferViews> bufferViews, uint buffersLength)
        {
            asset = new Asset("2.0");
            scene = 0;

            this.scenes = scenes;

            this.nodes = nodes;

            this.materials = materials;

            this.meshes = meshes;

            this.accessors = accessors;

            this.bufferViews = bufferViews;

            this.buffers = new List<Buffers>();
            this.buffers.Add(new Buffers(buffersLength));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootObject"/> class.
        /// </summary>
        public RootObject() { }
    }
}
