using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

class Program
{
    static void Main()
    {
        string filePath = "cub.glb";
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

                uint version = reader.ReadUInt32();
                uint length = reader.ReadUInt32();

                Console.WriteLine($"Версия GLB файла: {version}");
                Console.WriteLine($"Размер GLB файла: {length} байт");

                // Считываем данные JSON
                uint jsonLength = reader.ReadUInt32();
                string jsonStr = Encoding.ASCII.GetString(reader.ReadBytes((int)jsonLength + 4));



                //Console.WriteLine($"JSON данные:");
                //Console.WriteLine(jsonStr);

                // Считываем бинарные данные (если они есть)
                uint binChunkLength = reader.ReadUInt32();
                if (binChunkLength > 0)
                {
                    byte[] binaryData = reader.ReadBytes((int)binChunkLength);

                    // Поиск буфера вершин в бинарных данных
                    int vertexBufferIndex = FindVertexBuffer(binaryData);

                    if (vertexBufferIndex != -1)
                    {
                        // Получаем буфер вершин
                        byte[] vertexBuffer = GetVertexBuffer(binaryData, vertexBufferIndex);

                        // Извлекаем координаты вершин из буфера
                        float[] vertices = ParseVertices(vertexBuffer);

                        // Выводим координаты вершин
                        Console.WriteLine("Координаты вершин:");
                        for (int i = 0; i < vertices.Length; i += 3)
                        {
                            Console.WriteLine($"X: {vertices[i]}, Y: {vertices[i + 1]}, Z: {vertices[i + 2]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Буфер вершин не найден в бинарных данных.");
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

    static int FindVertexBuffer(byte[] binaryData)
    {
        // Идентификатор буфера вершин в формате ASCII
        byte[] vertexBufferIdentifier = Encoding.ASCII.GetBytes("BIN");

        for (int i = 0; i < binaryData.Length - vertexBufferIdentifier.Length; i++)
        {
            bool match = true;

            // Проверяем совпадение идентификатора
            for (int j = 0; j < vertexBufferIdentifier.Length; j++)
            {
                if (binaryData[i + j] != vertexBufferIdentifier[j])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                return i;
            }
        }

        // Буфер вершин не найден
        return -1;
    }

    static byte[] GetVertexBuffer(byte[] binaryData, int startIndex)
    {
        // Находим размер буфера вершин
        int bufferSize = BitConverter.ToInt32(binaryData, startIndex + 4);

        // Копируем буфер вершин
        byte[] vertexBuffer = new byte[bufferSize];
        Array.Copy(binaryData, startIndex, vertexBuffer, 0, bufferSize);

        return vertexBuffer;
    }

    static float[] ParseVertices(byte[] vertexBuffer)
    {
        int vertexCount = BitConverter.ToInt32(vertexBuffer, 0);

        float[] vertices = new float[vertexCount * 3];

        // Индекс начала координат вершин в буфере
        int offset = 4;

        for (int i = 0; i < vertices.Length; i += 3)
        {
            vertices[i] = BitConverter.ToSingle(vertexBuffer, offset);
            vertices[i + 1] = BitConverter.ToSingle(vertexBuffer, offset + 4);
            vertices[i + 2] = BitConverter.ToSingle(vertexBuffer, offset + 8);

            offset += 12; // Каждая координата занимает 4 байта (float)
        }

        return vertices;
    }
}

