namespace GLB_TXT
{
    public class BufferViews
    {
        public int buffer { get; set; }
        public int byteLength { get; set; }
        public int byteOffset { get; set; }
        public int target { get; set; }
        public int byteStride { get; set; }

        //public BufferViews(int buffer, int byteOffset, int byteLength, int target)
        //{
        //    this.buffer = buffer;
        //    this.byteOffset = byteOffset;
        //    this.byteLength = byteLength;
        //    this.target = target;
        //}

        public BufferViews() { }
    }
}
