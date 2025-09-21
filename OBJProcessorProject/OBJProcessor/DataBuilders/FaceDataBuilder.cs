using System.Security.AccessControl;

namespace OBJProcessor.DataBuilders;

public class FaceDataBuilder : MeshDataBuilder
{
    public const string FACE_TAG = "f";


    private int GetValidIndex(string data, int maxValue)
    {
        if (int.TryParse(data, out int index))
        {
            if (index > maxValue)
                throw new InvalidDataException(ERROR_MESSAGE + " - invalid face data index");

            return index < 0 ? index : index - 1;

        }
        throw new InvalidDataException(ERROR_MESSAGE + " - invalid face data format");
    }

    private VertexData ParseVertexData(MeshData meshData, string data)
    {
        string[] splitData = data.Split('/');

        if (splitData.Length < 1 || splitData.Length > 3)
            throw new InvalidDataException(ERROR_MESSAGE + " - invalid face data amount");

        int vertexIndex = 0;
        int? uvCoordIndex = null;
        int? normalIndex = null;

        if (splitData.Length >= 1 && !string.IsNullOrWhiteSpace(splitData[0]))
            vertexIndex = GetValidIndex(splitData[0], meshData.Vertices.Count);

        if (splitData.Length >= 2 && !string.IsNullOrWhiteSpace(splitData[1]))
            uvCoordIndex = GetValidIndex(splitData[1], meshData.UVCoords.Count);

        if (splitData.Length == 3 && !string.IsNullOrWhiteSpace(splitData[2]))
            normalIndex = GetValidIndex(splitData[2], meshData.Normals.Count);

        return new VertexData(vertexIndex, uvCoordIndex, normalIndex);
    }

    public override bool CanProcess(string tag)
    {
        return tag == FACE_TAG;
    }

    public override void BuildMeshData(MeshData meshData, string[] line)
    {
        Face face = new();
        foreach (var faceData in line.Skip(1).ToArray())
        {
            var vertexData = ParseVertexData(meshData, faceData);
            meshData.Vertices[vertexData.VertexIndex]!.Faces.Add(face);
            face.Vertices.Add(vertexData);
        }

        meshData.Faces.Add(face);
    }
}