using GLB_TXT;
using System.Text;
using System.Text.Json;

class Program
{
    static void Main()
    {
        string filePath = "model.glb";
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

                    byte[] verticesBuffer = GetParamsBuffer(jsonAttribute, binaryData, "POSITION");
                    float[] vertices = ParseParams(jsonAttribute, verticesBuffer, "POSITION");

                    Console.WriteLine("Координаты вершин:");
                    for (int i = 0; i < vertices.Length; i += 3)
                    {
                        Console.WriteLine($"X: {vertices[i]}, Y: {vertices[i + 1]}, Z: {vertices[i + 2]}");
                    }

                    byte[] normalBuffer = GetParamsBuffer(jsonAttribute, binaryData, "NORMAL");
                    float[] normal = ParseParams(jsonAttribute, normalBuffer, "NORMAL");

                    Console.WriteLine("Нормали:");
                    for (int i = 0; i < normal.Length; i += 3)
                    {
                        Console.WriteLine($"X: {normal[i]}, Y: {normal[i + 1]}, Z: {normal[i + 2]}");
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

    static byte[] GetParamsBuffer(RootObject jsonAttribute, byte[] binaryData, string param)
    {
        int paramsPosition;
        jsonAttribute.meshes[0].primitives[0].attributes.TryGetValue(param, out paramsPosition);

        int bufferPosition = jsonAttribute.accessors[paramsPosition].bufferView;
        int paramsOffset = jsonAttribute.bufferViews[bufferPosition].byteOffset;
        int paramsLength = jsonAttribute.bufferViews[bufferPosition].byteLength + 4;

        byte[] paramsBuffer = new byte[paramsLength];
        Array.Copy(binaryData, paramsOffset, paramsBuffer, 0, paramsLength);

        return paramsBuffer;
    }

    static float[] ParseParams(RootObject jsonAttribute, byte[] paramsBuffer, string param)
    {
        int paramsPosition;
        jsonAttribute.meshes[0].primitives[0].attributes.TryGetValue(param, out paramsPosition);

        int paramsCount = jsonAttribute.accessors[paramsPosition].count;

        float[] data = new float[paramsCount * 3];

        int offset = 4;

        for (int i = 0; i < data.Length; i += 3)
        {
            data[i] = BitConverter.ToSingle(paramsBuffer, offset);
            data[i + 1] = BitConverter.ToSingle(paramsBuffer, offset + 4);
            data[i + 2] = BitConverter.ToSingle(paramsBuffer, offset + 8);

            offset += 12;  
        }

        return data;
    }
}

