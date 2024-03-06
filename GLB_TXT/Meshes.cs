namespace GLB_TXT
{
    public class Meshes
    {
        public string? name { get; set; }
        public List<Primitives> primitives { get; set; }

        public Meshes(int mode, int position, int normal, int indices, int material)
        {
            primitives = new List<Primitives>();
            primitives.Add(new Primitives(mode, position, normal, indices, material));
        }

        public Meshes(int mode, int position, int normal, int indices)
        {
            primitives = new List<Primitives>();
            primitives.Add(new Primitives(mode, position, normal, indices));
        }

        public Meshes(int mode, int position, int normal)
        {
            primitives = new List<Primitives>();
            primitives.Add(new Primitives(mode, position, normal));
        }

        public Meshes(string name, int mode, int position, int normal, int indices, int material)
        {
            this.name = name;
            primitives = new List<Primitives>();
            primitives.Add(new Primitives(mode, position, normal, indices, material));
        }

        public Meshes() { }
    }
}
