using GLB_TXT;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Entry point for the GLB to TXT and TXT to GLB conversion program.
/// </summary>
class Program
{
    /// <summary>
    /// Main method where the program execution starts.
    /// </summary>
    static void Main()
    {
        Console.WriteLine("1.glb->txt");
        Console.WriteLine("2.txt->glb");
        
        int tmp = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter path to folder");
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

    /// <summary>
    /// Retrieves the full paths of files in the specified folder.
    /// </summary>
    /// <param name="folderPath">The path to the folder containing the files.</param>
    /// <returns>An array of file addresses.</returns>
    private static string[] GetFileAddresses(string folderPath)
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

    /// <summary>
    /// Creates a GLB file from the content of a text file.
    /// </summary>
    /// <param name="filePath">The path to the text file.</param>
    private static void CreateGLB(string filePath)
    {
        string[] lines = ReadTxt(filePath);

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

        ParseTxt(lines, ref vertices, ref normals, ref faces);

        RootObject jsonData = CreateRootObject(ref vertices, ref normals, ref faces);

        byte[] binBuffer = CreateBinBuffer(jsonData, ref vertices, ref normals, ref faces);

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
            writer.Write(BitConverter.GetBytes((uint)jsonData.buffers[0].byteLength));
            writer.Write(binType);
            writer.Write(binBuffer);
        }
    }

    /// <summary>
    /// Reads the contents of a text file and returns an array of lines.
    /// </summary>
    /// <param name="filePath">The path to the text file.</param>
    /// <returns>An array of lines from the text file.</returns>
    private static string[] ReadTxt(string filePath)
    {
        string[] linesOld = File.ReadAllLines(filePath);

        string[] lines = new string[linesOld.Length - 1];
        Array.Copy(linesOld, 1, lines, 0, lines.Length);

        return lines;
    }

    /// <summary>
    /// Parses the content of a text file and populates lists for vertices, normals, and faces.
    /// </summary>
    /// <param name="lines">An array of lines from the text file.</param>
    /// <param name="vertices">List to store vertex data.</param>
    /// <param name="normals">List to store normal data.</param>
    /// <param name="faces">List to store face data.</param>
    private static void ParseTxt(string[] lines, ref List<List<float>> vertices, ref List<List<float>> normals, ref List<List<short>> faces)
    {
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
    }

    /// <summary>
    /// Creates the root JSON object representing the structure of a GLB file.
    /// </summary>
    /// <param name="vertices">List of vertex data.</param>
    /// <param name="normals">List of normal data.</param>
    /// <param name="faces">List of face data.</param>
    /// <returns>The root JSON object representing the GLB file structure.</returns>
    private static RootObject CreateRootObject(ref List<List<float>> vertices, ref List<List<float>> normals, ref List<List<short>> faces)
    {
        List<Meshes> meshes = new List<Meshes>();
        List<Accessors> accessors = new List<Accessors>();
        List<BufferViews> bufferViews = new List<BufferViews>();
        List<int> node = new List<int>();
        List<Nodes> nodes = new List<Nodes>();
        List<double> min = new List<double>();
        List<double> max = new List<double>();

        uint binLength = 0;

        for (int i = 0; i < vertices.Count; i++)
        {
            bufferViews.Add(new BufferViews(0, (uint)(vertices[i].Count * sizeof(float)), binLength, 34962));
            binLength += (uint)(vertices[i].Count * sizeof(float));
            bufferViews.Add(new BufferViews(0, (uint)(normals[i].Count * sizeof(float)), binLength, 34962));
            binLength += (uint)(normals[i].Count * sizeof(float));
            bufferViews.Add(new BufferViews(0, (uint)(faces[i].Count * sizeof(short)), binLength, 34963));
            binLength += (uint)(faces[i].Count * sizeof(short));

            min.Add(vertices[i].Min());
            min.Add(vertices[i].Min());
            min.Add(vertices[i].Min());

            max.Add(vertices[i].Max());
            max.Add(vertices[i].Max());
            max.Add(vertices[i].Max());

            meshes.Add(new Meshes($"aboba{i}", 0, i * 3 + 0, i * 3 + 1, i * 3 + 2, 0));
            nodes.Add(new Nodes($"aboba{i}", i));
            node.Add(i);

            accessors.Add(new Accessors(i * 3 + 0, 5126, vertices[i].Count / 3, min, max, "VEC3"));
            accessors.Add(new Accessors(i * 3 + 1, 5126, normals[i].Count / 3, "VEC3"));
            accessors.Add(new Accessors(i * 3 + 2, 5123, faces[i].Count, "SCALAR"));
        }

        List<Scenes> scenes = new List<Scenes>();
        scenes.Add(new Scenes("Scene", node));

        return new RootObject(scenes, nodes, meshes, accessors, bufferViews, binLength);
    }

    /// <summary>
    /// Creates the binary buffer for the GLB file using the provided JSON data and vertex information.
    /// </summary>
    /// <param name="jsonData">The root JSON data representing the GLB file structure.</param>
    /// <param name="vertices">List of vertex data.</param>
    /// <param name="normals">List of normal data.</param>
    /// <param name="faces">List of face data.</param>
    /// <returns>The binary buffer for the GLB file.</returns>
    private static byte[] CreateBinBuffer(RootObject jsonData, ref List<List<float>> vertices, ref List<List<float>> normals, ref List<List<short>> faces)
    {
        byte[] binBuffer = new byte[jsonData.buffers[0].byteLength];

        for (int i = 0; i < jsonData.meshes.Count; i++)
        {
            for (int j = 0; j < vertices[i].Count; j++)
                Array.Copy(BitConverter.GetBytes(vertices[i][j]), 0, binBuffer, jsonData.bufferViews[i * 3 + 0].byteOffset + j * sizeof(float), sizeof(float));

            for (int j = 0; j < normals[i].Count; j++)
                Array.Copy(BitConverter.GetBytes(vertices[i][j]), 0, binBuffer, jsonData.bufferViews[i * 3 + 1].byteOffset + j * sizeof(float), sizeof(float));

            for (int j = 0; j < faces[i].Count; j++)
                Array.Copy(BitConverter.GetBytes(faces[i][j]), 0, binBuffer, jsonData.bufferViews[i * 3 + 2].byteOffset + j * sizeof(short), sizeof(short));

        }

        return binBuffer;
    }

    /// <summary>
    /// Parses a GLB file and creates a corresponding TXT file.
    /// </summary>
    /// <param name="filePath">The path to the GLB file.</param>
    private static void ParseGLB(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(fileStream))
        using (StreamWriter writer = new StreamWriter(filePath.Replace("glb", "txt")))
        {
            // Read the magic header to verify the GLB file format.
            string magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
            if (magic != "glTF")
            {
                Console.WriteLine("Неверный формат GLB файла.");
                return;
            }

            // Read and ignore the version and length fields of the GLB header.
            reader.ReadBytes(8);

            uint jsonLength = reader.ReadUInt32();
            reader.ReadBytes(4);
            string jsonStr = Encoding.ASCII.GetString(reader.ReadBytes((int)jsonLength));

            RootObject jsonAttribute = JsonSerializer.Deserialize<RootObject>(jsonStr);

            uint binChunkLength = reader.ReadUInt32();
            if (binChunkLength > 0)
            {
                reader.ReadUInt32();
                byte[] binaryData = reader.ReadBytes((int)binChunkLength);

                // Iterate through each mesh in the GLB file.
                for (int mesh = 0; mesh < jsonAttribute.scenes[0].nodes.Count; mesh++)
                {
                    // Extract and parse vertex data.
                    byte[] verticesBuffer = GetParamsBuffer(jsonAttribute, binaryData, "POSITION", mesh);
                    float[] vertices = ParseParams(jsonAttribute, verticesBuffer, "POSITION", mesh);

                    // Extract and parse normal data.
                    byte[] normalBuffer = GetParamsBuffer(jsonAttribute, binaryData, "NORMAL", mesh);
                    float[] normal = ParseParams(jsonAttribute, normalBuffer, "NORMAL", mesh);

                    // Initialize an array to store face indices.
                    ushort[] faces = new ushort[vertices.Length / 3];

                    // Check the rendering mode to determine how faces are constructed.
                    if (jsonAttribute.meshes[mesh].primitives[0].mode == 4)
                    {
                        // Mode where every 3 consecutive points form a polygon
                        for (ushort i = 0; i < faces.Length; i++)
                        {
                            faces[i] = i;
                        }
                    }
                    else
                    {
                        // Other rendering modes, read face indices from the binary data.
                        int facesPosition = jsonAttribute.meshes[mesh].primitives[0].indices;
                        int bufferPosition = jsonAttribute.accessors[facesPosition].bufferView;
                        uint facesOffset = jsonAttribute.bufferViews[bufferPosition].byteOffset;
                        uint facesLength = jsonAttribute.bufferViews[bufferPosition].byteLength;

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

    /// <summary>
    /// Parses a GLB file, extracts parameter buffers, and returns the buffer for a specific parameter.
    /// </summary>
    /// <param name="jsonAttribute">The root JSON data representing the GLB file structure.</param>
    /// <param name="binaryData">The binary data of the GLB file.</param>
    /// <param name="param">The parameter to extract (e.g., POSITION, NORMAL).</param>
    /// <param name="mesh">The index of the mesh in the GLB file.</param>
    /// <returns>The buffer for the specified parameter.</returns>
    private static byte[] GetParamsBuffer(RootObject jsonAttribute, byte[] binaryData, string param, int mesh)
    {
        int paramsPosition;
        jsonAttribute.meshes[mesh].primitives[0].attributes.TryGetValue(param, out paramsPosition);

        int bufferPosition = jsonAttribute.accessors[paramsPosition].bufferView;
        uint paramsOffset = jsonAttribute.bufferViews[bufferPosition].byteOffset;
        uint paramsLength = jsonAttribute.bufferViews[bufferPosition].byteLength + 4;

        byte[] paramsBuffer = new byte[paramsLength];
        Array.Copy(binaryData, paramsOffset, paramsBuffer, 0, paramsLength);

        return paramsBuffer;
    }

    /// <summary>
    /// Parses a parameter buffer and returns an array of floating-point values.
    /// </summary>
    /// <param name="jsonAttribute">The root JSON data representing the GLB file structure.</param>
    /// <param name="paramsBuffer">The buffer containing parameter data.</param>
    /// <param name="param">The parameter to parse (e.g., POSITION, NORMAL).</param>
    /// <param name="mesh">The index of the mesh in the GLB file.</param>
    /// <returns>An array of floating-point values representing the parsed parameter data.</returns>
    private static float[] ParseParams(RootObject jsonAttribute, byte[] paramsBuffer, string param, int mesh)
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
}