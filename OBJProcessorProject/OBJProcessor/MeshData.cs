using System;
using System.Collections.Concurrent;
using System.Numerics;

namespace OBJProcessor;

public record class MeshVertex // TODO: make it thread safe
{
    private object _lock = new();
    private Vector4 _position;
    private ThreadSafeList<Face> _faces;


    /// <summary>
    /// 
    ///     TODO: Lock vertices that are incident with faces?
    /// 
    /// </summary>

    public Vector4 Position
    {
        get
        {
            lock (_lock)
            {
                return _position;
            }
        }
        set
        {
            lock (_lock)
            {
                _position = value;
            }
        }

    }
    public ThreadSafeList<Face> Faces => _faces;

    public MeshVertex(Vector3 position) : this(new Vector4(position, 1.0f)) { }
    public MeshVertex(Vector4 position)
    {
        _position = position;
        _faces = new();
    }
}

public record class VertexData
{
    public int VertexIndex { get; init; }
    public int UVIndex { get; init; }
    public int NormalIndex { get; init; }

    public VertexData(int vertex, int uv, int normal)
    {
        VertexIndex = vertex;
        UVIndex = uv;
        NormalIndex = normal;
    }
}

public record class Face // TODO: Make it thread safe
{
    /// <summary>
    /// 
    ///     TODO: Lock vertices that are incident with faces?
    /// 
    /// </summary>
    public ThreadSafeList<VertexData> Vertices { get; } = new();
}

public record class PtrWrapper<T> where T : struct
{
    private object _lock = new();
    private T _data;
    public T Data
    {
        get
        {
            lock (_lock)
            {
                return _data;
            }
        }
        set
        {
            lock (_lock)
            {
                _data = value;
            }
        }
    }
    public PtrWrapper(T data)
    {
        _data = data;
    }
}

public record class MeshData
{
    public ThreadSafeList<MeshVertex> Vertices = new();
    public ThreadSafeList<Face> Faces = new();
    public ThreadSafeList<PtrWrapper<Vector3>> Normals = new();
    public ThreadSafeList<PtrWrapper<Vector2>> UVCoords = new();
}
