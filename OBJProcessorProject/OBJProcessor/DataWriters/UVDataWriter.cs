using System;

namespace OBJProcessor.DataWriters;

public class UVDataWriter : MeshDataWriter
{
    public UVDataWriter(string uvTag, DeletionSynchronization? deletionSynchronization = null)
        : base(uvTag, deletionSynchronization) { }

    public override void WriteMeshData(TextWriter writer, MeshData mesh)
    {
        foreach (var uvCoord in mesh.UVCoords)
        {
            if (uvCoord is not null)
            {
                writer.WriteLine(Tag + WriteVector(uvCoord.Data));
            }
        }
    }
}
