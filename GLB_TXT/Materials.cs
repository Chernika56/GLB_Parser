namespace GLB_TXT
{
    internal class Materials
    {
        public string name { get; set; }
        public bool doubleSided { get; set; }
        public MetallicRoughness pbrMetallicRoughness;

        public Materials(string name, bool doubleSided, MetallicRoughness pbrMetallicRoughness)
        {
            this.name = name;
            this.doubleSided = doubleSided;
            this.pbrMetallicRoughness = pbrMetallicRoughness;
        }
    }
}
