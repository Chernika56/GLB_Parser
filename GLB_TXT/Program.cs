using GLB_TXT.Object;
using System.Numerics;
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
        Console.WriteLine("3.stl->txt");
        Console.WriteLine("4.txt->stl");

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

            if (tmp == 3)
            {
                if (filePath.EndsWith(".stl"))
                {
                    ParseSTL(filePath);
                }
            }

            if (tmp == 4)
            {
                if (filePath.EndsWith(".txt"))
                {
                    CreateSTL(filePath);
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

        List<List<Vector3>> vertices = new List<List<Vector3>>();
        List<List<Vector3>> normals = new List<List<Vector3>>();
        List<List<short>> faces = new List<List<short>>();

        vertices.Add(new List<Vector3>());
        normals.Add(new List<Vector3>());
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
    private static void ParseTxt(string[] lines, ref List<List<Vector3>> vertices, ref List<List<Vector3>> normals, ref List<List<short>> faces)
    {
        int tmp = 0;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                tmp++;

                if (tmp % 3 == 0)
                {
                    vertices.Add(new List<Vector3>());
                    normals.Add(new List<Vector3>());
                    faces.Add(new List<short>());
                }
            }
            else
            {
                string cleanedLine = line.Remove(0, 1 + line.IndexOf('(')).Replace(")", "");
                string[] values = cleanedLine.Split(';');

                switch (tmp % 3)
                {
                    case 0:
                        vertices[tmp / 3].Add(new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2])));
                        break;
                    case 1:
                        normals[tmp / 3].Add(new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2])));
                        break;
                    case 2:
                        faces[tmp / 3].Add(short.Parse(values[0]));
                        faces[tmp / 3].Add(short.Parse(values[1]));
                        faces[tmp / 3].Add(short.Parse(values[2]));
                        break;
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
    private static RootObject CreateRootObject(ref List<List<Vector3>> vertices, ref List<List<Vector3>> normals, ref List<List<short>> faces)
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
            bufferViews.Add(new BufferViews(0, (uint)(vertices[i].Count * 3 * sizeof(float)), binLength, 34962));
            binLength += (uint)(vertices[i].Count * 3 * sizeof(float));
            bufferViews.Add(new BufferViews(0, (uint)(normals[i].Count * 3 * sizeof(float)), binLength, 34962));
            binLength += (uint)(normals[i].Count * 3 * sizeof(float));
            bufferViews.Add(new BufferViews(0, (uint)(faces[i].Count * sizeof(short)), binLength, 34963));
            binLength += (uint)(faces[i].Count * sizeof(short));

            min.Add(vertices[i].Min(v => v.X));
            min.Add(vertices[i].Min(v => v.Y));
            min.Add(vertices[i].Min(v => v.Z));

            max.Add(vertices[i].Max(v => v.X));
            max.Add(vertices[i].Max(v => v.Y));
            max.Add(vertices[i].Max(v => v.Z));

            meshes.Add(new Meshes($"aboba{i}", 0, i * 3 + 0, i * 3 + 1, i * 3 + 2, 0));
            nodes.Add(new Nodes($"aboba{i}", i));
            node.Add(i);

            accessors.Add(new Accessors(i * 3 + 0, 5126, vertices[i].Count, min, max, "VEC3"));
            accessors.Add(new Accessors(i * 3 + 1, 5126, normals[i].Count, "VEC3"));
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
    private static byte[] CreateBinBuffer(RootObject jsonData, ref List<List<Vector3>> vertices, ref List<List<Vector3>> normals, ref List<List<short>> faces)
    {
        byte[] binBuffer = new byte[jsonData.buffers[0].byteLength];

        for (int i = 0; i < jsonData.meshes.Count; i++)
        {
            for (int j = 0; j < vertices[i].Count; j++)
                Array.Copy(Vector3ToByteArray(vertices[i][j]), 0, binBuffer, jsonData.bufferViews[i * 3 + 0].byteOffset + j * 3 * sizeof(float), sizeof(float));

            for (int j = 0; j < normals[i].Count; j++)
                Array.Copy(Vector3ToByteArray(vertices[i][j]), 0, binBuffer, jsonData.bufferViews[i * 3 + 1].byteOffset + j * 3 * sizeof(float), sizeof(float));

            for (int j = 0; j < faces[i].Count; j++)
                Array.Copy(BitConverter.GetBytes(faces[i][j]), 0, binBuffer, jsonData.bufferViews[i * 3 + 2].byteOffset + j * sizeof(short), sizeof(short));

        }

        return binBuffer;
    }

    /// <summary>
    /// Converts a Vector3 object to a byte array by concatenating the byte representations of its X, Y, and Z components.
    /// </summary>
    /// <param name="vector">The Vector3 object to convert.</param>
    /// <returns>A byte array representing the Vector3 object.</returns>
    private static byte[] Vector3ToByteArray(Vector3 vector)
    {
        // Convert X, Y, Z components of the Vector3 to byte arrays
        byte[] xBytes = BitConverter.GetBytes(vector.X);
        byte[] yBytes = BitConverter.GetBytes(vector.Y);
        byte[] zBytes = BitConverter.GetBytes(vector.Z);

        // Concatenate the byte arrays to form the final result
        byte[] result = xBytes.Concat(yBytes).Concat(zBytes).ToArray();

        return result;
    }

    /// <summary>
    /// Parses a GLB file and creates a corresponding TXT file.
    /// </summary>
    /// <param name="filePath">The path to the GLB file.</param>
    private static void ParseGLB(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(fileStream))
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
                    List<Vector3> vertices = ParseParams(jsonAttribute, verticesBuffer, "POSITION", mesh);

                    // Extract and parse normals data.
                    byte[] normalBuffer = GetParamsBuffer(jsonAttribute, binaryData, "NORMAL", mesh);
                    List<Vector3> normals = ParseParams(jsonAttribute, normalBuffer, "NORMAL", mesh);

                    // Initialize a list to store face indices.
                    List<ushort> faces = new List<ushort>();

                    // Check the rendering mode to determine how faces are constructed.
                    if (jsonAttribute.meshes[mesh].primitives[0].mode == 4)
                    {
                        // Mode where every 3 consecutive points form a polygon
                        for (ushort i = 0; i < vertices.Count * 3; i++)
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
                        int facesCount = jsonAttribute.accessors[facesPosition].count;

                        byte[] facesBuffer = new byte[facesLength];
                        Array.Copy(binaryData, facesOffset, facesBuffer, 0, facesLength);

                        for (int i = 0; i < facesCount; i++)
                        {
                            faces.Add(BitConverter.ToUInt16(facesBuffer, i * sizeof(ushort)));
                        }
                    }

                    CreateTxt(filePath, vertices, normals, faces);
                }
            }
        }
    }

    /// <summary>
    /// Creates a text file (.txt) from the STL file data, including vertices, normals, and faces.
    /// </summary>
    /// <param name="filePath">The path to the STL file.</param>
    /// <param name="vertices">List of Vector3 representing vertices.</param>
    /// <param name="normals">List of Vector3 representing normals.</param>
    /// <param name="faces">List of ushort representing faces.</param>
    private static void CreateTxt(string filePath, List<Vector3> vertices, List<Vector3> normals, List<ushort> faces)
    {
        using (StreamWriter writer = new StreamWriter(filePath.Replace("stl", "txt")))
        {
            // Write vertices to the text file
            writer.WriteLine();
            for (int i = 0; i < vertices.Count; i++)
            {
                writer.WriteLine($"{i}:({vertices[i].X};{vertices[i].Y};{vertices[i].Z})");
            }

            // Write normals to the text file
            writer.WriteLine();
            for (int i = 0; i < normals.Count; i++)
            {
                writer.WriteLine($"{i}:({normals[i].X};{normals[i].Y};{normals[i].Z})");
            }

            // Write faces to the text file
            writer.WriteLine();
            for (int i = 0; i < faces.Count; i += 3)
            {
                writer.WriteLine($"{i}:({faces[i]};{faces[i + 1]};{faces[i + 2]})");
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
    /// Parses a parameter buffer and returns a List of Vector3 values.
    /// </summary>
    /// <param name="jsonAttribute">The root JSON data representing the GLB file structure.</param>
    /// <param name="paramsBuffer">The buffer containing parameter data.</param>
    /// <param name="param">The parameter to parse (e.g., POSITION, NORMAL).</param>
    /// <param name="mesh">The index of the mesh in the GLB file.</param>
    /// <returns>An array of floating-point values representing the parsed parameter data.</returns>
    private static List<Vector3> ParseParams(RootObject jsonAttribute, byte[] paramsBuffer, string param, int mesh)
    {
        int paramsPosition;
        jsonAttribute.meshes[mesh].primitives[0].attributes.TryGetValue(param, out paramsPosition);

        int paramsCount = jsonAttribute.accessors[paramsPosition].count;

        List<Vector3> data = new List<Vector3>();

        int offset = 0;

        for (int i = 0; i < paramsCount * 3; i += 3)
        {
            float x = BitConverter.ToSingle(paramsBuffer, offset);
            float y = BitConverter.ToSingle(paramsBuffer, offset + 4);
            float z = BitConverter.ToSingle(paramsBuffer, offset + 8);
            data.Add(new Vector3(x, y, z));

            offset += 12;
        }
        return data;
    }

    /// <summary>
    /// Parses an STL file, extracting vertices, normals, and faces, and creates a corresponding text file.
    /// </summary>
    /// <param name="filePath">The path to the STL file.</param>
    private static void ParseSTL(string filePath)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<ushort> faces = new List<ushort>();

        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        using (BinaryReader br = new BinaryReader(fs))
        {
            // Read the STL file header (first 80 bytes)
            byte[] header = br.ReadBytes(80);

            // Read the total number of triangles in the STL file
            uint triangleCount = br.ReadUInt32();

            // Loop through each triangle in the STL file
            for (int i = 0; i < triangleCount; i++)
            {
                // Read the normal vector of the triangle
                float nx = br.ReadSingle();
                float ny = br.ReadSingle();
                float nz = br.ReadSingle();

                // Add the normal vector for each vertex of the triangle
                normals.Add(new Vector3(nx, ny, nz));
                normals.Add(new Vector3(nx, ny, nz));
                normals.Add(new Vector3(nx, ny, nz));

                // Loop through each vertex of the triangle
                for (int j = 0; j < 3; j++)
                {
                    // Read the coordinates of the vertex
                    float x = br.ReadSingle();
                    float y = br.ReadSingle();
                    float z = br.ReadSingle();

                    // Add the vertex to the list
                    vertices.Add(new Vector3(x, y, z));

                    // Add the face index (based on the current triangle and vertex)
                    faces.Add((ushort)(i * 3 + j));
                }

                // Skip the attribute byte count (2 bytes) for each triangle
                br.BaseStream.Seek(2, SeekOrigin.Current);
            }
        }

        // Create a corresponding text file with the extracted data
        CreateTxt(filePath, vertices, normals, faces);
    }

    /// <summary>
    /// Creates an STL file from the specified text file, containing vertices, normals, and faces information.
    /// </summary>
    /// <param name="filePath">The path to the input text file.</param>
    private static void CreateSTL(string filePath)
    {
        // Read the content of the text file
        string[] lines = ReadTxt(filePath);

        // Lists to store vertices, normals, and faces
        List<List<Vector3>> vertices = new List<List<Vector3>>();
        List<List<Vector3>> normals = new List<List<Vector3>>();
        List<List<short>> faces = new List<List<short>>();

        // Initialize lists for the first object
        vertices.Add(new List<Vector3>());
        normals.Add(new List<Vector3>());
        faces.Add(new List<short>());

        // Check if the file is empty
        if (lines.Length == 0)
        {
            throw new Exception("The file is empty");
        }

        // Parse the text file to extract vertices, normals, and faces
        ParseTxt(lines, ref vertices, ref normals, ref faces);

        // Create a header for the binary STL file
        byte[] header = new byte[80];
        byte[] text = Encoding.UTF8.GetBytes("binary stl file                                                                ");
        Array.Copy(text, 0, header, 0, text.Length);

        // Calculate the total number of triangles in all objects
        int trianglesCount = GetTotalElements(faces) / 3;

        // Convert the number of triangles to bytes
        byte[] numOfTriangles = BitConverter.GetBytes((uint)trianglesCount);

        // Write the header and the number of triangles to the binary STL file
        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath.Replace(".txt", "new.stl"), FileMode.Create)))
        {
            writer.Write(header);
            writer.Write(numOfTriangles);

            // Loop through each object and its faces
            for (int i = 0; i < faces.Count; i++)
            {
                // Loop through each face's vertices
                for (int j = 0; j < faces[i].Count;)
                {
                    // Write the normal vector and vertices to the binary STL file
                    writer.Write(Vector3ToByteArray(normals[i][faces[i][j]]));
                    writer.Write(Vector3ToByteArray(vertices[i][faces[i][j++]]));
                    writer.Write(Vector3ToByteArray(vertices[i][faces[i][j++]]));
                    writer.Write(Vector3ToByteArray(vertices[i][faces[i][j++]]));

                    // Write two bytes of attribute data (currently set to 0)
                    writer.Write((byte)0);
                    writer.Write((byte)0);
                }
            }
        }
    }

    /// <summary>
    /// Calculates the total number of elements in a list of lists.
    /// </summary>
    /// <param name="listOfLists">The list of lists containing elements.</param>
    /// <returns>The total number of elements in all lists.</returns>
    private static int GetTotalElements(List<List<short>> listOfLists)
    {
        return listOfLists.SelectMany(list => list).Count();
    }
}