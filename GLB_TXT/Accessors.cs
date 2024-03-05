﻿namespace GLB_TXT
{
    internal class Accessors
    {
        public enum AccessorsType
        {
            SCALAR,
            VEC2,
            VEC3,
            MAT4
        }

        public int bufferView { get; set; }
        public int? byteOffset { get; set; }
        public int componentType { get; set; }
        public int count { get; set; }
        public AccessorsType type;
        public float[]? max { get; set; }
        public float[]? min { get; set; }

        public Accessors(int bufferView, int? byteOffset, int componentType, int count, 
            AccessorsType type, float[]? max, float[]? min)
        {
            this.bufferView = bufferView;
            this.byteOffset = byteOffset;
            this.componentType = componentType;
            this.count = count;
            this.type = type;
            this.max = max;
            this.min = min;
        }

        public Accessors(AccessorsType type, int bufferView, int componentType, int count)
        {
            this.type = type;
            this.bufferView = bufferView;
            this.componentType = componentType;
            this.count = count;
        }
    }
}
