using GLB_TXT;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

class Program
{
    static void Main()
    {
        Console.WriteLine("1.glb->txt");
        Console.WriteLine("2.txt->glb");

        int tmp = Convert.ToInt32(Console.ReadLine());

        string folderPath = Console.ReadLine();
        string[] fileAddresses = GetFileAddresses(folderPath);
        foreach (string filePath in fileAddresses)
        {
            if (tmp == 1)
            {
                if (filePath.EndsWith(".glb"))
                {
                    ParseGLB(filePath);
                }
            }

            if (tmp == 2)
            {
                if (filePath.EndsWith(".txt"))
                {
                    CreateGLB(filePath);
                }
            }
        }
    }

    static void CreateGLB(string filePath)
    {
        string[] lines = System.IO.File.ReadAllLines(filePath);

        List<float> vertices = new List<float>();

        if (lines.Length == 0)
        {
            throw new Exception("Файл пуст");
        }

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] != null) 
            {
                string line = lines[i].Remove(0, 1 + lines[i].IndexOf('('));
                line = line.Replace(")", "");

                string[] values = line.Split(';');
                float[] data = new float[values.Length];
                for (int j = 0; j < values.Length; j++)
                {
                    vertices.Add(float.Parse(values[j], CultureInfo.InvariantCulture)); 
                }
            }
        }
    }

    static void ParseGLB(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(fileStream))
        {
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

            uint binChunkLength = reader.ReadUInt32() + 4;
            if (binChunkLength > 0)
            {
                byte[] binaryData = reader.ReadBytes((int)binChunkLength);

                byte[] verticesBuffer = GetParamsBuffer(jsonAttribute, binaryData, "POSITION");
                float[] vertices = ParseParams(jsonAttribute, verticesBuffer, "POSITION");

                byte[] normalBuffer = GetParamsBuffer(jsonAttribute, binaryData, "NORMAL");
                float[] normal = ParseParams(jsonAttribute, normalBuffer, "NORMAL");

                ushort[] faces = new ushort[vertices.Length / 3];

                if (jsonAttribute.meshes[0].primitives[0].mode == 4)
                {
                    for (ushort i = 0; i < faces.Length; i++)
                    {
                        faces[i] = i;
                    }
                }
                else
                {
                    int facesPosition = jsonAttribute.meshes[0].primitives[0].indices;
                    int bufferPosition = jsonAttribute.accessors[facesPosition].bufferView;
                    int facesOffset = jsonAttribute.bufferViews[bufferPosition].byteOffset + 4;
                    int facesLength = jsonAttribute.bufferViews[bufferPosition].byteLength;

                    byte[] facesBuffer = new byte[facesLength];
                    Array.Copy(binaryData, facesOffset, facesBuffer, 0, facesLength);

                    int facesCount = jsonAttribute.accessors[facesPosition].count;
                    Array.Resize(ref faces, facesCount);

                    for (int i = 0; i < faces.Length; i++)
                    {
                        faces[i] = BitConverter.ToUInt16(facesBuffer, i * sizeof(ushort));
                    }
                }

                using (StreamWriter writer = new StreamWriter(filePath.Replace("glb", "txt")))
                {
                    for (int i = 0; i < vertices.Length; i += 3)
                    {
                        writer.WriteLine($"{i / 3}:({vertices[i]};{vertices[i + 1]};{vertices[i + 2]})");
                    }

                    writer.WriteLine();
                    for (int i = 0; i < normal.Length; i += 3)
                    {
                        writer.WriteLine($"{i / 3}:({normal[i]};{normal[i + 1]};{normal[i + 2]})");
                    }

                    writer.WriteLine();
                    for (int i = 0; i < faces.Length; i += 3)
                    {
                        writer.WriteLine($"{i / 3}:({faces[i]};{faces[i + 1]};{faces[i + 2]})");
                    }
                }

            }
            else
            {
                Console.WriteLine("Бинарные данные отсутствуют.");
            }
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

    static string[] GetFileAddresses(string folderPath)
    {
        try
        {
            string[] files = Directory.GetFiles(folderPath);

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFullPath(files[i]);
            }

            return files;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return new string[0];
        }
    }
}

