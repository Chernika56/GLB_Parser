namespace GLB_TXT
{
    public class Meshes
    {
        public string name { get; set; }
        public List<Primitives> primitives { get; set; }

        //public Meshes(Primitives[] primitives)
        //{
        //    this.primitives = primitives;
        //}

        //public Meshes(Primitives[] primitives, string? name)
        //{
        //    this.primitives = primitives;
        //    this.name = name;
        //}

        public Meshes() { }
    }
}
