using System;
using System.Reflection;

namespace OBJProcessor.DataWriters;

public class FaceDataWriter : MeshDataWriter
{
    public FaceDataWriter(string faceTag) : base(faceTag) { }

    private string WriteVertexData(VertexData vertexData) {
        string output = "";
        int vertexIndex = vertexData.VertexIndex;
        int? uvCoordIndex = vertexData.UVIndex;
        int? normalIndex = vertexData.NormalIndex;

        output += (vertexIndex < 0 ? vertexIndex : vertexIndex + 1).ToString();

        if (uvCoordIndex.HasValue || normalIndex.HasValue)
        {
            output += '/';
            if (uvCoordIndex.HasValue)
            {
                output += (uvCoordIndex < 0 ? uvCoordIndex : uvCoordIndex + 1).ToString();
            }
            if (normalIndex.HasValue)
            {
                output += '/';
                output += (normalIndex < 0 ? normalIndex : normalIndex + 1).ToString();
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
            if(face is not null)
                writer.WriteLine(Tag + WriteFace(face));
        }
    }
}
