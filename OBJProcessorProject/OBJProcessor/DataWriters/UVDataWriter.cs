using System;

namespace OBJProcessor.DataWriters;

public class UVDataWriter : MeshDataWriter
{
    public UVDataWriter(string uvTag) : base(uvTag) { }

    public override void WriteMeshData(TextWriter writer, MeshData mesh)
    {
        foreach (var uvCoord in mesh.UVCoords)
        {
            writer.WriteLine(Tag + WriteVector(uvCoord.Data));
        }
    }
}
