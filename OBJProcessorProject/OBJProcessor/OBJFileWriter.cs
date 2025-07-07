namespace OBJProcessor;


class OBJFileWriter: IDisposable {
    private TextWriter _writer;

    public OBJFileWriter(TextWriter writer) {
        _writer = writer;
    }

    public void WriteMeshData(MeshData meshData) { 
        // TODO:
    }

    public void Dispose()
    {
        _writer?.Dispose();
    }
}
