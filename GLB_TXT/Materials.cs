namespace GLB_TXT
{
    public class Materials
    {
        public bool doubleSided { get; set; }
        public string name { get; set; }
        public MetallicRoughness pbrMetallicRoughness { get; set; }

        public Materials(string name, bool doubleSided, double metallicFactor, double roughnessFactor)
        {
            this.name = name;
            this.doubleSided = doubleSided;
            pbrMetallicRoughness = new MetallicRoughness(metallicFactor, roughnessFactor);
        }

        public Materials() { }
    }
}
