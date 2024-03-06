namespace GLB_TXT
{
    public class Attributes
    {
        public int POSITION { get; set; }
        public int? NORMAL { get; set; }
        public int? TEXCOORD_0 { get; set; }
        public int? TANGENT { get; set; }
        public int? COLOR_01 { get; set; }
        public int? JOINTS_0 { get; set;}
        public int? WEIGHTS_0 { get; set;}

        public Attributes(int position)
        {
            POSITION = position;
        }

        public Attributes(int position, int normal)
        {
            POSITION = position;
            NORMAL = normal;
        }

        public Attributes(int position, int normal, int texcoord)
        {
            POSITION = position;
            NORMAL = normal;
            TEXCOORD_0 = texcoord;
        }

        public Attributes(int position, int? normal, int? texcoord, int? tangent, int? color, 
            int? joints, int? weights)
        {
            POSITION = position;
            NORMAL = normal;
            TEXCOORD_0 = texcoord;
            TANGENT = tangent;
            COLOR_01 = color;
            JOINTS_0 = joints;
            WEIGHTS_0 = weights;
        }
    }
}
