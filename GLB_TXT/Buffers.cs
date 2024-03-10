namespace GLB_TXT
{
    /// <summary>
    /// Represents the buffer information in a GLB (Binary GLTF) file.
    /// </summary>
    public class Buffers
    {
        /// <summary>
        /// Gets or sets the total length of the buffer in bytes.
        /// </summary>
        public uint byteLength { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Buffers"/> class with the specified total length of the buffer in bytes.
        /// </summary>
        /// <param name="byteLength">The total length of the buffer in bytes.</param>
        public Buffers(uint byteLength)
        {
            this.byteLength = byteLength;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Buffers"/> class.
        /// </summary>
        public Buffers() { }
    }
}
