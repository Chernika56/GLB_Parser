using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace GLB_TXT
{
    /// <summary>
    /// Represents accessor information for accessing bufferView data in a GLB (Binary GLTF) file.
    /// </summary>
    public class Accessors
    {
        /// <summary>
        /// Gets or sets the index of the bufferView.
        /// </summary>
        public int bufferView { get; set; }

        // Uncomment the line below if you decide to use the 'byteOffset' property.
        //public int byteOffset { get; set; }

        /// <summary>
        /// Gets or sets the component type of the accessor data.
        /// </summary>
        public int componentType { get; set; }

        /// <summary>
        /// Gets or sets the number of elements in the accessor.
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// Gets or sets the maximum values for each component in the accessor.
        /// </summary>
        public List<double>? max { get; set; }

        /// <summary>
        /// Gets or sets the minimum values for each component in the accessor.
        /// </summary>
        public List<double>? min { get; set; }

        /// <summary>
        /// Gets or sets the data type of the accessor.
        /// </summary>
        [JsonPropertyName("type")]
        public string type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Accessors"/> class with specified bufferView index,
        /// component type, count, and data type.
        /// </summary>
        /// <param name="bufferView">The index of the bufferView.</param>
        /// <param name="componentType">The component type of the accessor data.</param>
        /// <param name="count">The number of elements in the accessor.</param>
        /// <param name="type">The data type of the accessor.</param>
        public Accessors(int bufferView, int componentType, int count, string type)
        {
            this.bufferView = bufferView;
            this.componentType = componentType;
            this.count = count;
            this.type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Accessors"/> class with specified bufferView index,
        /// component type, count, minimum and maximum values, and data type.
        /// </summary>
        /// <param name="bufferView">The index of the bufferView.</param>
        /// <param name="componentType">The component type of the accessor data.</param>
        /// <param name="count">The number of elements in the accessor.</param>
        /// <param name="min">The minimum values for each component in the accessor.</param>
        /// <param name="max">The maximum values for each component in the accessor.</param>
        /// <param name="type">The data type of the accessor.</param>
        public Accessors(int bufferView, int componentType, int count, List<double> min, List<double> max, string type)
        {
            this.bufferView = bufferView;
            this.componentType = componentType;
            this.count = count;
            this.min = min;
            this.max = max;
            this.type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Accessors"/> class.
        /// </summary>
        public Accessors() { }
    }
}
