using GLB_TXT;
using System.Text;
using System.Text.Json;

class Program
{
    static void Main()
    {
        string filePath = "model .glb";
        ParseGLB(filePath);
    }

    static void ParseGLB(string filePath)
    {
        try
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
                // Считываем заголовок GLB файла
                string magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
                if (magic != "glTF")
                {
                    Console.WriteLine("Неверный формат GLB файла.");
                    return;
                }

                reader.ReadBytes(8);

                uint jsonLength = reader.ReadUInt32();
                reader.ReadBytes(4);
                string jsonStr = Encoding.ASCII.GetString(reader.ReadBytes((int)jsonLength));

                RootObject jsonAttribute = JsonSerializer.Deserialize<RootObject>(jsonStr);

                uint binChunkLength = reader.ReadUInt32();
                if (binChunkLength > 0)
                {
                    byte[] binaryData = reader.ReadBytes((int)binChunkLength);
                    byte[] verticesBuffer = GetVertexBuffer(jsonAttribute, binaryData);
                    float[] vertices = ParseVertices(jsonAttribute, verticesBuffer);

                    Console.WriteLine("Координаты вершин:");
                    for (int i = 0; i < vertices.Length; i += 3)
                    {
                        Console.WriteLine($"X: {vertices[i]}, Y: {vertices[i + 1]}, Z: {vertices[i + 2]}");
                    }
                }
                else
                {
                    Console.WriteLine("Бинарные данные отсутствуют.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении GLB файла: {ex.Message}");
        }
    }

    static byte[] GetVertexBuffer(RootObject jsonAttribute, byte[] binaryData)
    {
        int verticesPosition;
        jsonAttribute.meshes[0].primitives[0].attributes.TryGetValue("POSITION", out verticesPosition);

        int bufferPosition = jsonAttribute.accessors[verticesPosition].bufferView;
        int verticesOffset = jsonAttribute.bufferViews[bufferPosition].byteOffset;
        int verticesLength = jsonAttribute.bufferViews[bufferPosition].byteLength + 4;

        // Копируем буфер вершин
        byte[] verticesBuffer = new byte[verticesLength];
        Array.Copy(binaryData, verticesOffset, verticesBuffer, 0, verticesLength);

        return verticesBuffer;
    }

    static float[] ParseVertices(RootObject jsonAttribute, byte[] verticesBuffer)
    {
        int verticesPosition;
        jsonAttribute.meshes[0].primitives[0].attributes.TryGetValue("POSITION", out verticesPosition);

        int verticesCount = jsonAttribute.accessors[verticesPosition].count;

        float[] vertices = new float[verticesCount * 3];

        // Индекс начала координат вершин в буфере
        int offset = 4;

        for (int i = 0; i < vertices.Length; i += 3)
        {
            vertices[i] = BitConverter.ToSingle(verticesBuffer, offset);
            vertices[i + 1] = BitConverter.ToSingle(verticesBuffer, offset + 4);
            vertices[i + 2] = BitConverter.ToSingle(verticesBuffer, offset + 8);

            offset += 12; // Каждая координата занимает 4 байта (float)
        }

        return vertices;
    }
}

