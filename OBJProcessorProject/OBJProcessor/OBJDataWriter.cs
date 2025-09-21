using OBJProcessor.DataWriters;
namespace OBJProcessor;

public class OBJDataWriter
{
    private List<MeshDataWriter> _meshWriters = new();

    public OBJDataWriter SetWriter(MeshDataWriter meshWriter)
    {
        _meshWriters.Add(meshWriter);
        return this;
    }

    public void WriteMeshData(TextWriter textWriter, MeshData meshData)
    {
        foreach (var meshWriter in _meshWriters)
        {
            meshWriter.WriteMeshData(textWriter, meshData);
        }
    }
}
