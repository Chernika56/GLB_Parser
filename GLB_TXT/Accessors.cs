using System.Security.AccessControl;
using System.Text.Json.Serialization;

namespace GLB_TXT
{
    public class Accessors
    {
        //public enum AccessorsType
        //{
        //    SCALAR,
        //    VEC2,
        //    VEC3,
        //    MAT4
        //}

        public int bufferView { get; set; }
        //public int byteOffset { get; set; }
        public int componentType { get; set; }
        public int count { get; set; }
        public List<double>? max { get; set; }
        public List<double>? min { get; set; }

        [JsonPropertyName("type")]
        public string type { get; set; }

        public Accessors(int bufferView, int componentType, int count, string type)
        {
            this.bufferView = bufferView;
            this.componentType = componentType; 
            this.count = count;
            this.type = type;
        }

        public Accessors(int bufferView, int componentType, int count, List<double> min, List<double> max, string type)
        {
            this.bufferView = bufferView;
            this.componentType = componentType;
            this.count = count;
            this.min = min;
            this.max = max;
            this.type = type;
        }

        public Accessors() { }
    }
}
