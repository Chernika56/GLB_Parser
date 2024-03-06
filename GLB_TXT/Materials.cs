namespace GLB_TXT
{
    public class Materials
    {
        public bool doubleSided { get; set; }
        public string name { get; set; }
        public MetallicRoughness pbrMetallicRoughness { get; set; }

        //public Materials(string name, bool doubleSided, MetallicRoughness pbrMetallicRoughness)
        //{
        //    this.name = name;
        //    this.doubleSided = doubleSided;
        //    this.pbrMetallicRoughness = pbrMetallicRoughness;
        //}

        public Materials() { }
    }
}
