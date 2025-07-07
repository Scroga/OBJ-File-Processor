using System.Security.Cryptography.X509Certificates;

namespace OBJProcessor;

class OBJFileReader : IDisposable {
    TextReader _reader;

    public OBJFileReader(TextReader reader) {
        _reader = reader;
    }

    public MeshData ReadMeshData() {
        var line = _reader.ReadLine();
        return new MeshData();
    }

    public void Dispose() {
        _reader?.Dispose();
    }
}
