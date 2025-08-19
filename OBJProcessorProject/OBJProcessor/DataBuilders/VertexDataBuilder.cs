using System.Globalization;
using System.IO;
using System.Numerics;

namespace OBJProcessor.DataBuilders;

public class VertexDataBuilder : MeshDataBuilder
{
    public const string VERTEX_TAG = "v";

    private Vector3 ParseLine(string[] data) {
        if (data.Length != 3 && data.Length != 4)
            throw new InvalidDataException(ERROR_MESSAGE + ": invalid vertex data");

        float x = ParseToFloat(data[0]);
        float y = ParseToFloat(data[1]);
        float z = ParseToFloat(data[2]);
        float w = data.Length == 4 ? ParseToFloat(data[3]) : 1.0f;

        return new Vector3(x / w, y / w, z / w);
    }

    public override bool CanProcess(string tag)
    {
        return tag == VERTEX_TAG;
    }

    public override void BuildMeshData(MeshData meshData, string[] line)
    {
        var vertex = new MeshVertex(ParseLine(line.Skip(1).ToArray()));
        meshData.Vertices.Add(vertex);
    }
}
