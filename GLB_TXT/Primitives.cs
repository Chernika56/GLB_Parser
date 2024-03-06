namespace GLB_TXT
{
    public class Primitives
    {
        public Dictionary<string, int> attributes { get; set; }
        public int indices { get; set; }
        public int material { get; set; }
        public int mode { get; set; }

        public Primitives(int mode, int position, int normal, int indices, int material)
        {
            attributes = new Dictionary<string, int>();
            attributes.Add("POSITION", position);
            attributes.Add("NOMAL", normal);

            this.indices = indices;
            this.mode = mode;
            this.material = material;
        }

        public Primitives(int mode, int position, int normal, int indices)
        {
            attributes = new Dictionary<string, int>();
            attributes.Add("POSITION", position);
            attributes.Add("NOMAL", normal);

            this.indices = indices;
            this.mode = mode;
        }

        public Primitives(int mode, int position, int normal)
        {
            attributes = new Dictionary<string, int>();
            attributes.Add("POSITION", position);
            attributes.Add("NOMAL", normal);

            this.mode = mode;
        }

        public Primitives() { }
    }
}
