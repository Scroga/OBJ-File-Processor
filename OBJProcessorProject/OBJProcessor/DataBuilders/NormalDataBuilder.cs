using System.Numerics;

namespace OBJProcessor.DataBuilders;

public class NormalDataBuilder : MeshDataBuilder
{
    public const string NORMAL_TAG = "vn";

    private Vector3 ParseLine(string[] data)
    {
        if (data.Length != 3)
            throw new InvalidDataException(ERROR_MESSAGE + " - invalid normal vector data");

        float x = ParseToFloat(data[0]);
        float y = ParseToFloat(data[1]);
        float z = ParseToFloat(data[2]);

        return new Vector3(x, y, z);
    }

    public override bool CanProcess(string tag)
    {
        return tag == NORMAL_TAG;
    }

    public override void BuildMeshData(MeshData meshData, string[] line)
    {
        meshData.Normals.Add(new PtrWrapper<Vector3>(ParseLine(line.Skip(1).ToArray())));
    }
}
