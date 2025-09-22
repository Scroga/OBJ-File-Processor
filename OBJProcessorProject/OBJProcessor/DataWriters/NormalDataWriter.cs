using System;

namespace OBJProcessor.DataWriters;

public class NormalDataWriter : MeshDataWriter
{
    public NormalDataWriter(string normalTag, DeletionSynchronization? deletionSynchronization = null) 
        : base(normalTag, deletionSynchronization) { }

    public override void WriteMeshData(TextWriter writer, MeshData mesh)
    {
        foreach (var normal in mesh.Normals)
        {
            if (normal is not null)
            {
                writer.WriteLine(Tag + WriteVector(normal.Data));
            }
        }
    }
}
