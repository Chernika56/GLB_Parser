using System;

namespace GLB_TXT.Object
{
    /// <summary>
    /// Represents a scene in a GLB (Binary GLTF) file, containing information about the scene's name and associated nodes.
    /// </summary>
    public class Scenes
    {
        /// <summary>
        /// Gets or sets the name of the scene.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the list of node indices associated with the scene.
        /// </summary>
        public List<int> nodes { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Scenes"/> class with the specified name and node index.
        /// </summary>
        /// <param name="name">The name of the scene.</param>
        /// <param name="node">The index of the node associated with the scene.</param>
        public Scenes(string? name, int node)
        {
            this.name = name;
            nodes = new List<int>();
            nodes.Add(node);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scenes"/> class with the specified node index.
        /// </summary>
        /// <param name="node">The index of the node associated with the scene.</param>
        public Scenes(int node)
        {
            nodes = new List<int>();
            nodes.Add(node);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scenes"/> class with the specified name and list of node indices.
        /// </summary>
        /// <param name="name">The name of the scene.</param>
        /// <param name="nodes">The list of node indices associated with the scene.</param>
        public Scenes(string? name, List<int> nodes)
        {
            this.name = name;
            this.nodes = nodes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scenes"/> class.
        /// </summary>
        public Scenes() { }
    }
}
