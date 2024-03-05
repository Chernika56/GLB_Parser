namespace GLB_TXT
{
    internal class Primitives
    {
        public int? mode {  get; set; }
        public int? material { get; set; }
        public int indices { get; set; }
        public int? weights { get; set; }
        public Attributes attributes;

        public Primitives(Attributes attributes, int indices)
        {
            this.attributes = attributes;
            this.indices = indices;
        }

        public Primitives(Attributes attributes, int indices, int? material)
        {
            this.attributes = attributes;
            this.indices = indices;
            this.material = material;
        }

        public Primitives(Attributes attributes, int indices, int? material, int? mode)
        {
            this.attributes = attributes;
            this.indices = indices;
            this.material = material;
            this.mode = mode;
        }

        public Primitives(Attributes attributes, int indices, int? material, int? mode, int? weights)
        {
            this.attributes = attributes;
            this.indices = indices;
            this.material = material;
            this.mode = mode;
            this.weights = weights;
        }
    }
}
