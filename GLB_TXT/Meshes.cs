namespace GLB_TXT
{
    internal class Meshes
    {
        public string? name { get; set; }
        public Primitives[] primitives;

        public Meshes(Primitives[] primitives)
        {
            this.primitives = primitives;
        }

        public Meshes(Primitives[] primitives, string? name)
        {
            this.primitives = primitives;
            this.name = name;
        }
    }
}
