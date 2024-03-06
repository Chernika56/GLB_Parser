namespace GLB_TXT
{
    public class MetallicRoughness
    {
        public List<double> baseColorFactor { get; set; } 
        public double metallicFactor { get; set; }
        public double roughnessFactor { get; set; }

        //public MetallicRoughness(float[] baseColorFactor, float metallicFactor, float roughnessFactor)
        //{
        //    this.baseColorFactor = baseColorFactor;
        //    this.metallicFactor = metallicFactor;
        //    this.roughnessFactor = roughnessFactor;
        //}
    }
}
