namespace GLB_TXT
{
    /// <summary>
    /// Represents bufferView information for accessing a specific portion of a buffer in a GLB (Binary GLTF) file.
    /// </summary>
    public class BufferViews
    {
        /// <summary>
        /// Gets or sets the index of the buffer.
        /// </summary>
        public uint buffer { get; set; }

        /// <summary>
        /// Gets or sets the length of the buffer view in bytes.
        /// </summary>
        public uint byteLength { get; set; }

        /// <summary>
        /// Gets or sets the offset into the buffer in bytes.
        /// </summary>
        public uint byteOffset { get; set; }

        /// <summary>
        /// Gets or sets the target that the bufferView should be bound to (e.g., ARRAY_BUFFER, ELEMENT_ARRAY_BUFFER).
        /// </summary>
        public int target { get; set; }

        /// <summary>
        /// Gets or sets the byte stride between consecutive elements in the bufferView. If not specified, tightly packed.
        /// </summary>
        public int? byteStride { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferViews"/> class with specified buffer index,
        /// buffer view length, offset, target, and byte stride.
        /// </summary>
        /// <param name="buffer">The index of the buffer.</param>
        /// <param name="byteLength">The length of the buffer view in bytes.</param>
        /// <param name="byteOffset">The offset into the buffer in bytes.</param>
        /// <param name="target">The target that the bufferView should be bound to (e.g., ARRAY_BUFFER, ELEMENT_ARRAY_BUFFER).</param>
        /// <param name="byteStride">The byte stride between consecutive elements in the bufferView. If not specified, tightly packed.</param>
        public BufferViews(uint buffer, uint byteLength, uint byteOffset, int target, int byteStride)
        {
            this.buffer = buffer;
            this.byteOffset = byteOffset;
            this.byteLength = byteLength;
            this.target = target;
            this.byteStride = byteStride;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferViews"/> class with specified buffer index,
        /// buffer view length, offset, and target.
        /// </summary>
        /// <param name="buffer">The index of the buffer.</param>
        /// <param name="byteLength">The length of the buffer view in bytes.</param>
        /// <param name="byteOffset">The offset into the buffer in bytes.</param>
        /// <param name="target">The target that the bufferView should be bound to (e.g., ARRAY_BUFFER, ELEMENT_ARRAY_BUFFER).</param>
        public BufferViews(uint buffer, uint byteLength, uint byteOffset, int target)
        {
            this.buffer = buffer;
            this.byteOffset = byteOffset;
            this.byteLength = byteLength;
            this.target = target;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferViews"/> class.
        /// </summary>
        public BufferViews() { }
    }
}
