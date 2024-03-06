namespace GLB_TXT
{
    public class MetallicRoughness
    {
        public List<double>? baseColorFactor { get; set; } 
        public double metallicFactor { get; set; }
        public double roughnessFactor { get; set; }

        public MetallicRoughness(double metallicFactor, double roughnessFactor)
        {
            this.metallicFactor = metallicFactor;
            this.roughnessFactor = roughnessFactor;
        }

        public MetallicRoughness() { }
    }
}
