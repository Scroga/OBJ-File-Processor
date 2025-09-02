using System;

namespace OBJProcessor.DataWriters;

public class NormalDataWriter : MeshDataWriter
{
    public NormalDataWriter(string normalTag) : base(normalTag) { }

    public override void WriteMeshData(TextWriter writer, MeshData mesh)
    {
        foreach (var normal in mesh.Normals)
        {
            writer.WriteLine(Tag + WriteVector(normal.Data));
        }
    }
}
