using System;

namespace OBJProcessor.DataWriters;

public class VertexDataWriter : MeshDataWriter
{
    public VertexDataWriter(string vertexTeg) : base(vertexTeg) { }

    public override void WriteMeshData(TextWriter writer, MeshData mesh)
    {
        foreach (var vertex in mesh.Vertices)
        {
             writer.WriteLine(Tag + WriteVector(vertex.Position));
        }
    }
}
