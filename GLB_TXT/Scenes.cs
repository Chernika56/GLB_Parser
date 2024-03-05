namespace GLB_TXT
{
    internal class Scenes
    {
        public string? name { get; set; }
        public int[] nodes { get; set; }

        public Scenes(string? name, int[] nodes)
        {
            this.name = name;
            this.nodes = nodes;
        }

        public Scenes(int[] nodes)
        {
            this.nodes = nodes;
        }
    }
}
