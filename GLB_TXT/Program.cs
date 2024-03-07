using GLB_TXT;
using System.Globalization;
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
        string[] lines = File.ReadAllLines(filePath);

        List<float> vertices = new List<float>();
        List<float> normals = new List<float>();
        List<short> faces = new List<short>();

        if (lines.Length == 0)
        {
            throw new Exception("Файл пуст");
        }

        int tmp = 0;

        foreach (string line in lines)
        {
            if (line == null)
            {
                tmp++;
            }
            else
            {
                string cleanedLine = line.Remove(0, 1 + line.IndexOf('(')).Replace(")", "");
                string[] values = cleanedLine.Split(';');

                switch (tmp)
                {
                    case 0:
                        foreach (string value in values)
                        {
                            if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out float floatValue))
                            {
                                vertices.Add(floatValue);
                            }
                        }
                        break;
                    case 1:
                        foreach (string value in values)
                        {
                            if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out float floatValue))
                            {
                                normals.Add(floatValue);
                            }
                        }

                        break;
                    case 2:
                        foreach (string value in values)
                        {
                            if (short.TryParse(value, out short shortValue))
                            {
                                faces.Add(shortValue);
                            }
                        }
                        break;
                }
            }
        }


    }

    static void ParseGLB(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(fileStream))
        using (StreamWriter writer = new StreamWriter(filePath.Replace("glb", "txt")))

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

                for (int mesh = 0; mesh < jsonAttribute.scenes[0].nodes.Count; mesh++)
                {
                    byte[] verticesBuffer = GetParamsBuffer(jsonAttribute, binaryData, "POSITION", mesh);
                    float[] vertices = ParseParams(jsonAttribute, verticesBuffer, "POSITION", mesh);

                    byte[] normalBuffer = GetParamsBuffer(jsonAttribute, binaryData, "NORMAL", mesh);
                    float[] normal = ParseParams(jsonAttribute, normalBuffer, "NORMAL", mesh);

                    ushort[] faces = new ushort[vertices.Length / 3];

                    if (jsonAttribute.meshes[mesh].primitives[0].mode == 4)
                    {
                        for (ushort i = 0; i < faces.Length; i++)
                        {
                            faces[i] = i;
                        }
                    }
                    else
                    {
                        int facesPosition = jsonAttribute.meshes[mesh].primitives[0].indices;
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
                    writer.WriteLine();

                }
            }
            else
            {
                Console.WriteLine("Бинарные данные отсутствуют.");
            }
        }
    }

    static byte[] GetParamsBuffer(RootObject jsonAttribute, byte[] binaryData, string param, int mesh)
    {
        int paramsPosition;
        jsonAttribute.meshes[mesh].primitives[0].attributes.TryGetValue(param, out paramsPosition);

        int bufferPosition = jsonAttribute.accessors[paramsPosition].bufferView;
        int paramsOffset = jsonAttribute.bufferViews[bufferPosition].byteOffset;
        int paramsLength = jsonAttribute.bufferViews[bufferPosition].byteLength + 4;

        byte[] paramsBuffer = new byte[paramsLength];
        Array.Copy(binaryData, paramsOffset, paramsBuffer, 0, paramsLength);

        return paramsBuffer;
    }

    static float[] ParseParams(RootObject jsonAttribute, byte[] paramsBuffer, string param, int mesh)
    {
        int paramsPosition;
        jsonAttribute.meshes[mesh].primitives[0].attributes.TryGetValue(param, out paramsPosition);

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

