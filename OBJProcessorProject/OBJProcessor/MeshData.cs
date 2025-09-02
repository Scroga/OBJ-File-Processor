using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OBJProcessor;

public record class MeshVertex
{
    public Vector4 Position { get; set; }
    public List<Face> Faces { get; } = new();

    public MeshVertex(Vector4 position)
    {
        Position = position;
    }
    public MeshVertex(Vector3 position)
    {
        Position = new(position, 1.0f);
    }
}

public record class VertexData
{
    public int VertexIndex { get; }
    public int UVIndex { get; }
    public int NormalIndex { get; }

    public VertexData(int vertex, int uv, int normal)
    {
        VertexIndex = vertex;
        UVIndex = uv;
        NormalIndex = normal;
    }
}

public record class Face
{
    public List<VertexData> VerticesData { get; } = new();

    public Face AddVertex(int vertexIndex, int uvIndex = -1, int normalIndex = -1)
    {
        return AddVertex(new VertexData(vertexIndex, uvIndex, normalIndex));
    }

    public Face AddVertex(VertexData vertexData)
    {
        VerticesData.Add(vertexData);
        return this;
    }
}

public record class MeshData
{
    public List<MeshVertex> Vertices { get; } = new();
    public List<Vector3> Normals { get; } = new();
    public List<Vector2> UVs { get; } = new();
    public List<Face> Faces { get; } = new();

    public MeshData AddFace(Face face) 
    {
        foreach (var vertex in face.VerticesData)
            Vertices[vertex.VertexIndex].Faces.Add(face);

        return this;
    }
}

//public static class MeshDataExtention {
//    public static MeshData RemoveSomethig(this MeshData meshData, int i) {
//        // Do something
//        return meshData;
//    }
    
//}
