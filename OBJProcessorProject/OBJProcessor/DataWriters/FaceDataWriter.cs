using System;

namespace OBJProcessor.DataWriters;

public class FaceDataWriter : MeshDataWriter
{
    public FaceDataWriter(string faceTag) : base(faceTag) { }

    private string WriteVertexData(VertexData vertexData) {
        string output = "";
        int vertexIndex = vertexData.VertexIndex;
        int uvCoordIndex = vertexData.UVIndex;
        int normalIndex = vertexData.NormalIndex;

        if (vertexIndex != 0)
        {
            output += vertexIndex.ToString();
            if (uvCoordIndex != 0 || normalIndex != 0)
            {
                output += '/';
                if (uvCoordIndex != 0)
                {
                    output += uvCoordIndex.ToString();
                }
                if (normalIndex != 0)
                {
                    output += ('/' + normalIndex.ToString());
                }
            }
        }
        return output;
    }

    private string WriteFace(Face face)
    {
        string output = "";
        foreach (VertexData vertexData in face.Vertices)
        {
            output += (' ' + WriteVertexData(vertexData));
        }

        return output;
    }

    public override void WriteMeshData(TextWriter writer, MeshData mesh)
    {
        foreach (var face in mesh.Faces)
        {
            writer.WriteLine(Tag + WriteFace(face));
        }
    }
}
