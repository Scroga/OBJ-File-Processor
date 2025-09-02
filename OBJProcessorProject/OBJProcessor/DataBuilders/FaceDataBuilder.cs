using System.Security.AccessControl;

namespace OBJProcessor.DataBuilders;

public class FaceDataBuilder : MeshDataBuilder
{
    public static string FACE_TAG = "f";

    private int GetValidIndex(string data)
    {
        if (int.TryParse(data, out int index))
        {
            if (index >= 0) index--;
            return index;
        }
        throw new InvalidDataException(ERROR_MESSAGE + ": invalid face data index");
    }

    private VertexData ParseVertexData(string data)
    {
        string[] splitData = data.Split('/');

        if (splitData.Length < 1 || splitData.Length > 3)
            throw new InvalidDataException(ERROR_MESSAGE + ": invalid face data format");

        int vertexIndex = 0;
        int uvCoordIndex = 0;
        int normalIndex = 0;

        if (splitData.Length >= 1 && !string.IsNullOrWhiteSpace(splitData[0]))
            vertexIndex = GetValidIndex(splitData[0]);

        if (splitData.Length >= 2 && !string.IsNullOrWhiteSpace(splitData[1]))
            uvCoordIndex = GetValidIndex(splitData[1]);

        if (splitData.Length == 3 && !string.IsNullOrWhiteSpace(splitData[2]))
            normalIndex = GetValidIndex(splitData[2]);

        return new VertexData(vertexIndex, uvCoordIndex, normalIndex);
    }

    public override bool CanProcess(string tag)
    {
        return tag == FACE_TAG;
    }

    public override void BuildMeshData(MeshData meshData, string[] line)
    {
        Face face = new();
        for (int i = 1; i < line.Length; i++)
        {
            face.Vertices.Add(ParseVertexData(line[i]));
        }

        meshData.Faces.Add(face);
    }
}
