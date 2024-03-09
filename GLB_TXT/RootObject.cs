using static GLB_TXT.Accessors;

namespace GLB_TXT
{
    public class RootObject
    {
        public Asset asset { get; set; }
        public int scene { get; set; }
        public List<Scenes> scenes { get; set; }
        public List<Nodes> nodes { get; set; }
        public List<Materials> materials { get; set; }
        public List<Meshes> meshes { get; set; }
        public List<Accessors> accessors { get; set; }
        public List<BufferViews> bufferViews { get; set; }
        public List<Buffers> buffers { get; set; }

        public RootObject(int position, int normal, int indices, int positonCount, int normalCount, int indicesCount,
            List<BufferViews> bufferViews, int buffersLength)
        {
            asset = new Asset("2.0");
            scene = 0;

            scenes = new List<Scenes>();
            scenes.Add(new Scenes("Scene", 0));

            nodes = new List<Nodes>();
            nodes.Add(new Nodes("aboba", 0));

            materials = new List<Materials>();
            materials.Add(new Materials("Material", true, 0.5, 0.5));

            meshes = new List<Meshes>();
            meshes.Add(new Meshes(position, normal, indices));

            accessors = new List<Accessors>();
            accessors.Add(new Accessors(position, 5126, positonCount, "VEC3"));
            accessors.Add(new Accessors(normal, 5126, normalCount, "VEC3"));
            accessors.Add(new Accessors(indices, 5123, indicesCount, "SCALAR"));

            this.bufferViews = bufferViews;

            this.buffers = new List<Buffers>();
            this.buffers.Add(new Buffers(buffersLength));
        }

        public RootObject(List<Meshes> meshes, List<Accessors> accessors, List<BufferViews> bufferViews, int buffersLength)
        {
            asset = new Asset("2.0");
            scene = 0;

            scenes = new List<Scenes>();
            scenes.Add(new Scenes("Scene", 0));

            nodes = new List<Nodes>();
            nodes.Add(new Nodes("aboba", 0));

            materials = new List<Materials>();
            materials.Add(new Materials("Material", true, 0.5, 0.5));

            this.meshes = meshes;

            this.accessors = accessors;

            this.bufferViews = bufferViews;

            this.buffers = new List<Buffers>();
            this.buffers.Add(new Buffers(buffersLength));
        }

        public RootObject(List<Materials>  materials, List<Meshes> meshes, List<Accessors> accessors, List<BufferViews> bufferViews, int buffersLength)
        {
            asset = new Asset("2.0");
            scene = 0;

            scenes = new List<Scenes>();
            scenes.Add(new Scenes("Scene", 0));

            nodes = new List<Nodes>();
            nodes.Add(new Nodes("aboba", 0));

            this.materials = materials;

            this.meshes = meshes;

            this.accessors = accessors;

            this.bufferViews = bufferViews;

            this.buffers = new List<Buffers>();
            this.buffers.Add(new Buffers(buffersLength));
        }

        public RootObject() { }
    }
}
