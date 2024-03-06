namespace GLB_TXT
{
    public class Nodes
    {
        public string? name { get; set; }
        public int mesh { get; set; }

        public Nodes(string name, int mesh)
        {
            this.name = name;
            this.mesh = mesh;
        }

        public Nodes(int node)
        {
            this.mesh = mesh;
        }

        public Nodes() { }
    }
}
