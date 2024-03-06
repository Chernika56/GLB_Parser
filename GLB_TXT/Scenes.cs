using System;

namespace GLB_TXT
{
    public class Scenes
    {
        public string name { get; set; }
        public List<int> nodes { get; set; }

        public Scenes(string? name, int node)
        {
            this.name = name;
            nodes = new List<int>();
            nodes.Add(node);
        }

        public Scenes(int node)
        {
            nodes = new List<int>();
            nodes.Add(node);
        }

        public Scenes() { }
    }
}
