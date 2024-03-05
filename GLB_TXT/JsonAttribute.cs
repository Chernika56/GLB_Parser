namespace GLB_TXT
{
    internal class JsonAttribute
    {
        public Asset asset;
        public int scene;
        public Scenes[] scenes;
        public Nodes[] nodes;
        public Materials[]? materials;
        public Meshes[] meshes;
        public Accessors[] accessors;
        public BufferViews[] bufferViews;
        public Buffers[] buffers;

        public JsonAttribute(Asset asset, int scene, Scenes[] scenes, Nodes[] nodes, 
            Materials[]? materials, Meshes[] meshes, Accessors[] accessors, 
            BufferViews[] bufferViews, Buffers[] buffers)
        {
            this.asset = asset;
            this.scene = scene;
            this.scenes = scenes;
            this.nodes = nodes;
            this.materials = materials;
            this.meshes = meshes;
            this.accessors = accessors;
            this.bufferViews = bufferViews;
            this.buffers = buffers;
        }

        public JsonAttribute(Asset asset, int scene, Scenes[] scenes, Nodes[] nodes, 
            Meshes[] meshes, Accessors[] accessors, BufferViews[] bufferViews, Buffers[] buffers)
        {
            this.asset = asset;
            this.scene = scene;
            this.scenes = scenes;
            this.nodes = nodes;
            this.meshes = meshes;
            this.accessors = accessors;
            this.bufferViews = bufferViews;
            this.buffers = buffers;
        }
    }
}
