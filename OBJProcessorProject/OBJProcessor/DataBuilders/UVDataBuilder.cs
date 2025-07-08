using System.IO;
using System.Numerics;

namespace OBJProcessor.DataBuilders;

public class UVDataBuilder : MeshDataBuilder
{
    public const string UV_TAG = "vt";

    private Vector2 ParseLine(string[] data)
    {
        if (data.Length != 2)
            throw new InvalidDataException(ERROR_MESSAGE + ": invalid uv data");

        float u = ParseToFloat(data[0]);
        float v = ParseToFloat(data[1]);

        if (Math.Abs(u) > 1 || Math.Abs(v) > 1)
            throw new InvalidDataException(ERROR_MESSAGE + ": uv vector is not normalized");

        return new Vector2(u, v);
    }
    public override bool CanProcess(string tag)
    {
        return tag == UV_TAG;
    }

    public override void BuildMeshData(MeshData meshData, string[] line)
    {
        meshData.UVs.Add(ParseLine(line.Skip(1).ToArray()));
    }
}
