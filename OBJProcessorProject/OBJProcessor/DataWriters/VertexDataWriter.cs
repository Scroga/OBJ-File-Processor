using System;

namespace OBJProcessor.DataWriters;

public class VertexDataWriter : MeshDataWriter
{
    public VertexDataWriter(string vertexTeg, DeletionSynchronization? deletionSynchronization = null)
        : base(vertexTeg, deletionSynchronization) { }

    public override void WriteMeshData(TextWriter writer, MeshData mesh)
    {
        for (int i = 0; i < mesh.Vertices.Count; i++)
        {
            if (mesh.Vertices[i] is not null)
            {
                writer.WriteLine(Tag + WriteVector(mesh.Vertices[i]!.Position));
            }
            else
            {
                _deletionSynchronization?.IncrementCurrectVertexSubtrahend();
            }
            _deletionSynchronization?.AddVertexSubtrahend();
        }
    }
}
