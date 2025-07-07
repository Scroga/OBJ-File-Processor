using System.Globalization;
using System.IO;
using System.Numerics;

namespace OBJProcessor;

public class OBJFileReader : IDisposable
{
    private const string ERROR_MESSAGE = "The file format is invalid or corrupted";
    public const string COMMENT_FLAG = "#";
    public const string VERTEX_FLAG = "v";
    public const string PLYGONAL_FACE_FLAG = "f";

    public const string TEXTURE_COORD_FLAG = "vt";
    public const string NORMAL_VECTOR_FLAG = "vn";
    public const string PARAMTER_SPACE_FLAG = "vp";

    TextReader _reader;

    public OBJFileReader(TextReader reader)
    {
        _reader = reader;
    }

    private float ParseToFloat(string number)
    {
        if (float.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
        {
            return value;
        }
        throw new InvalidDataException(ERROR_MESSAGE);
    }

    public Vector3 ParseToVec3(string[] parts, bool isVec4 = true)
    {
        if (parts.Length == 4 && !isVec4)
            throw new InvalidDataException(ERROR_MESSAGE);

        if (parts.Length != 3 && parts.Length != 4)
            throw new InvalidDataException(ERROR_MESSAGE + ": invalid vertex data");

        float x = ParseToFloat(parts[0]);
        float y = ParseToFloat(parts[1]);
        float z = ParseToFloat(parts[2]);
        float w = parts.Length == 4 ? ParseToFloat(parts[3]) : 1.0f;

        return new Vector3(x / w, y / w, z / w);
    }

    public Vector2 ParseToVec2(string[] parts)
    {
        if (parts.Length != 2)
            throw new InvalidDataException(ERROR_MESSAGE + ": invalid vertex data");

        float u = ParseToFloat(parts[0]);
        float v = ParseToFloat(parts[1]);

        return new Vector2(u, v);
    }

    public VertexData ParseVertexData(string faceData)
    {
        // TODO:

        //f 1 2 3
        //f 3 / 1 4 / 2 5 / 3
        //f 6 / 4 / 1 3 / 5 / 3 7 / 6 / 5
        //f 7//1 8//2 9//3

        return new VertexData(0, 0, 0);
    }

    private void ProcessVertex(MeshData meshData, string[] line)
    {
        switch (line[0])
        {
            case VERTEX_FLAG:
                meshData.Vertices.Add(new MeshVertex(ParseToVec3(line.Skip(1).ToArray())));
                return;
            case TEXTURE_COORD_FLAG:
                meshData.UVs.Add(ParseToVec2(line.Skip(1).ToArray()));
                break;
            case NORMAL_VECTOR_FLAG:
                meshData.Normals.Add(ParseToVec3(line.Skip(1).ToArray(), false));
                return;
            case PARAMTER_SPACE_FLAG:
                // TODO:
                break;
            default: break;
        }
        throw new InvalidDataException(ERROR_MESSAGE);
    }

    private void ProcessFace(MeshData meshData, string[] line)
    {
        Face face = new();
        foreach (var faceData in line)
        {
            face.Vertices.Add(ParseVertexData(faceData));
        }

        meshData.Faces.Add(face);
    }

    private void ProcessOtherData(MeshData meshData, string[] line)
    {
        // TODO:
        throw new InvalidDataException(ERROR_MESSAGE);
    }

    public MeshData ReadMeshData()
    {
        var meshData = new MeshData();

        string? line = _reader.ReadLine();
        while (line != null)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                line = _reader.ReadLine();
                continue;
            }
            string[] splitLine = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (splitLine[0].StartsWith(COMMENT_FLAG)) continue;

            if (splitLine[0].StartsWith(VERTEX_FLAG))
            {
                ProcessVertex(meshData, splitLine);
            }
            else if (splitLine[0] == PLYGONAL_FACE_FLAG)
            {
                ProcessFace(meshData, splitLine.Skip(1).ToArray());
            }
            else
            {
                ProcessOtherData(meshData, splitLine);
            }
            line = _reader.ReadLine();
        }
        return meshData;
    }

    public void Dispose()
    {
        _reader?.Dispose();
    }
}