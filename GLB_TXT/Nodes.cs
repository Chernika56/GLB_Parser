namespace GLB_TXT
{
    public class Nodes
    {
        public string? name { get; set; }
        public int? mesh { get; set; }
        public int[]? children { get; set; }

        public Nodes(string? name, int? mesh, int[]? children)
        {
            this.name = name;
            this.mesh = mesh;
            this.children = children;
        }

        public Nodes(string name, int mesh) 
        {
            this.name = name;
            this.mesh = mesh;
        }

        public Nodes(int[] children) 
        { 
            this.children = children;
        }

        public Nodes(int mesh)
        {
            this.mesh = mesh;
        }
    }
}
