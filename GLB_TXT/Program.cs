using GLB_TXT;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        string[] linesOld = File.ReadAllLines(filePath);

        string[] lines = new string[linesOld.Length - 1];
        Array.Copy(linesOld, 1, lines, 0, lines.Length);

        List<List<float>> vertices = new List<List<float>>();
        List<List<float>> normals = new List<List<float>>();
        List<List<short>> faces = new List<List<short>>();

        vertices.Add(new List<float>());
        normals.Add(new List<float>());
        faces.Add(new List<short>());

        if (lines.Length == 0)
        {
            throw new Exception("Файл пуст");
        }

        int tmp = 0;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                tmp++;

                if (tmp % 3 == 0)
                {
                    vertices.Add(new List<float>());
                    normals.Add(new List<float>());
                    faces.Add(new List<short>());
                }
            }
            else
            {
                string cleanedLine = line.Remove(0, 1 + line.IndexOf('(')).Replace(")", "");
                string[] values = cleanedLine.Split(';');

                foreach (string value in values)
                {
                    switch (tmp % 3)
                    {
                        case 0:
                            if (float.TryParse(value, out float floatValue1))
                            {
                                vertices[tmp / 3].Add(floatValue1);
                            }
                            break;
                        case 1:
                            if (float.TryParse(value, out float floatValue2))
                            {
                                normals[tmp / 3].Add(floatValue2);
                            }
                            break;
                        case 2:
                            if (short.TryParse(value, out short shortValue))
                            {
                                faces[tmp / 3].Add(shortValue);
                            }
                            break;
                    }
                }
            }
        }

        List<Meshes> meshes = new List<Meshes>();
        List<Accessors> accessors = new List<Accessors>();
        List<BufferViews> bufferViews = new List<BufferViews>();

        int binLength = 0;

        for (int i = 0; i < vertices.Count; i++)
        {
            bufferViews.Add(new BufferViews(0, vertices[i].Count * sizeof(float), binLength, 34962));
            binLength += vertices[i].Count * sizeof(float);
            bufferViews.Add(new BufferViews(0, normals[i].Count * sizeof(float), binLength, 34962));
            binLength += normals[i].Count * sizeof(float);
            bufferViews.Add(new BufferViews(0, faces[i].Count * sizeof(short), binLength, 34963));
            binLength += faces[i].Count * sizeof(short);

            meshes.Add(new Meshes("aboba", 0, i * 3 + 0, i * 3 + 1, i * 3 + 2, 0));

            accessors.Add(new Accessors(i * 3 + 0, 5126, vertices[i].Count / 3, "VEC3"));
            accessors.Add(new Accessors(i * 3 + 1, 5126, normals[i].Count / 3, "VEC3"));
            accessors.Add(new Accessors(i * 3 + 2, 5123, faces[i].Count, "SCALAR"));
        }

        RootObject jsonData = new RootObject(meshes, accessors, bufferViews, binLength);

        byte[] binBuffer = new byte[binLength];

        for (int i = 0; i < meshes.Count; i++)
        {
            for (int j = 0; j < vertices[i].Count; j++)
                Array.Copy(BitConverter.GetBytes(vertices[i][j]), 0, binBuffer, bufferViews[i * 3 + 0].byteOffset + j * sizeof(float), sizeof(float));

            for (int j = 0; j < normals[i].Count; j++)
                Array.Copy(BitConverter.GetBytes(vertices[i][j]), 0, binBuffer, bufferViews[i * 3 + 1].byteOffset + j * sizeof(float), sizeof(float));

            for (int j = 0; j < faces[i].Count; j++)
                Array.Copy(BitConverter.GetBytes(faces[i][j]), 0, binBuffer, bufferViews[i * 3 + 2].byteOffset + j * sizeof(short), sizeof(short));

        }

        string jsonString = JsonSerializer.Serialize(jsonData, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = false });
        byte[] jsonLength = BitConverter.GetBytes((uint)jsonString.Length);
        byte[] jsonChunk = Encoding.UTF8.GetBytes("JSON" + jsonString);

        byte[] magic = Encoding.UTF8.GetBytes("glTF");
        byte[] version = BitConverter.GetBytes((uint)2);
        byte[] length = BitConverter.GetBytes((uint)(binBuffer.Length + 8));

        byte[] binType = Encoding.UTF8.GetBytes("BIN\0");

        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath.Replace(".txt", "new.glb"), FileMode.Create)))
        {
            writer.Write(magic);
            writer.Write(version);
            writer.Write(length);
            writer.Write(jsonLength);
            writer.Write(jsonChunk);
            writer.Write(BitConverter.GetBytes((uint)binLength));
            writer.Write(binType);
            writer.Write(binBuffer);
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

            byte[] asd = reader.ReadBytes(8);

            uint jsonLength = reader.ReadUInt32();
            reader.ReadBytes(4);
            string jsonStr = Encoding.ASCII.GetString(reader.ReadBytes((int)jsonLength));

            RootObject jsonAttribute = JsonSerializer.Deserialize<RootObject>(jsonStr);

            uint binChunkLength = reader.ReadUInt32();
            if (binChunkLength > 0)
            {
                reader.ReadUInt32();
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
                        int facesOffset = jsonAttribute.bufferViews[bufferPosition].byteOffset;
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

                    writer.WriteLine();
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

        int offset = 0;

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