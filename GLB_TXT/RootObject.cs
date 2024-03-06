namespace GLB_TXT
{
    public class RootObject
    {
        public Asset asset { get; set; }
        public int scene { get; set; }
        public List<Scenes> scenes { get; set; }
        public List<Scenes> nodes { get; set; }
        public List<Materials> materials { get; set; }
        public List<Meshes> meshes { get; set; }
        public List<Accessors> accessors { get; set; }
        public List<BufferViews> bufferViews { get; set; }
        public List<Buffers> buffers { get; set; }

        //public JsonAttribute(Asset asset, int scene, Scenes[] scenes, Nodes[] nodes, 
        //    Materials[]? materials, Meshes[] meshes, Accessors[] accessors, 
        //    BufferViews[] bufferViews, Buffers[] buffers)
        //{
        //    this.asset = asset;
        //    this.scene = scene;
        //    this.scenes = scenes;
        //    this.nodes = nodes;
        //    this.materials = materials;
        //    this.meshes = meshes;
        //    this.accessors = accessors;
        //    this.bufferViews = bufferViews;
        //    this.buffers = buffers;
        //}

        //public JsonAttribute(Asset asset, int scene, Scenes[] scenes, Nodes[] nodes, 
        //    Meshes[] meshes, Accessors[] accessors, BufferViews[] bufferViews, Buffers[] buffers)
        //{
        //    this.asset = asset;
        //    this.scene = scene;
        //    this.scenes = scenes;
        //    this.nodes = nodes;
        //    this.meshes = meshes;
        //    this.accessors = accessors;
        //    this.bufferViews = bufferViews;
        //    this.buffers = buffers;
        //}

        public RootObject() { }
    }
}
