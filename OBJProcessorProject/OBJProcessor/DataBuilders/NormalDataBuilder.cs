using System.Numerics;

namespace OBJProcessor.DataBuilders;

public class NormalDataBuilder : MeshDataBuilder
{
    public const string NORMAL_TAG = "vn";

    private Vector3 ParseLine(string[] data)
    {
        if (data.Length != 3)
            throw new InvalidDataException(ERROR_MESSAGE + ": invalid normal vector data");

        float x = ParseToFloat(data[0]);
        float y = ParseToFloat(data[1]);
        float z = ParseToFloat(data[2]);

        if(Math.Abs(x) > 1 || Math.Abs(y) > 1 || Math.Abs(z) > 1)
            throw new InvalidDataException(ERROR_MESSAGE + ": normal vector is not normalized");

        return new Vector3(x, y, z);
    }

    public override bool CanProcess(string tag)
    {
        return tag == NORMAL_TAG;
    }

    public override void BuildMeshData(MeshData meshData, string[] line)
    {
        meshData.Normals.Add(ParseLine(line.Skip(1).ToArray()));
    }
}
