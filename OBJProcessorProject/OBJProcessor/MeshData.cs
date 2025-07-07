using System.Numerics;

namespace OBJProcessor;

public record class MeshVertex
{
    public Vector4 Position { get; set; }
    public List<Face> Faces { get; } = new();

    public MeshVertex(Vector4 position) {
        Position = position;
    }
    public MeshVertex(Vector3 position) {
        Position = new(position, 1.0f);
    }
}

public record class VertexData {
    public int VertexIndex { get; }
    public int NormalIndex { get; }
    public int UVIndex { get; }

    public VertexData(int vertex, int normal, int uv) {
        VertexIndex = vertex;
        NormalIndex = normal;
        UVIndex = uv;
    }
}

public record class Face {
    public List<VertexData> Vertices { get; } = new();
}


public record class MeshData {
    public List<MeshVertex> Vertices { get; } = new();
    public List<Vector3> Normals { get; } = new();
    public List<Vector2> UVs { get; } = new();
    public List<Face> Faces { get; } = new();
}
