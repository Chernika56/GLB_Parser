namespace GLB_TXT
{
    internal class MetallicRoughness
    {
        public float[] baseColorFactor { get; set; } 
        public float metallicFactor { get; set; }
        public float roughnessFactor { get; set; }

        public MetallicRoughness(float[] baseColorFactor, float metallicFactor, float roughnessFactor)
        {
            this.baseColorFactor = baseColorFactor;
            this.metallicFactor = metallicFactor;
            this.roughnessFactor = roughnessFactor;
        }
    }
}
