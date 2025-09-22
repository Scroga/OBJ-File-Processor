using System;
using System.Numerics;

namespace OBJProcessor.MeshOperations;

public static class MeshNormalizationExtention
{
    public static Vector3 GetMinVector(MeshData mesh)
    {
        var min = new Vector3(float.MaxValue);
        foreach (var vertex in mesh.Vertices)
        {
            if (vertex is null) continue;
            var vec = vertex.GetAsVec3();
            min.X = Math.Min(min.X, vec.X);
            min.Y = Math.Min(min.Y, vec.Y);
            min.Z = Math.Min(min.Z, vec.Z);
        }

        return min;
    }
    public static Vector3 GetMaxVector(MeshData mesh)
    {
        var max = new Vector3(float.MinValue);
        foreach (var vertex in mesh.Vertices)
        {
            if (vertex is null) continue;
            var vec = vertex.GetAsVec3();
            max.X = Math.Max(max.X, vec.X);
            max.Y = Math.Max(max.Y, vec.Y);
            max.Z = Math.Max(max.Z, vec.Z);
        }

        return max;
    }

    public static void NormalizeMesh(MeshData mesh)
    {
        var minTask = Task.Run(() => GetMinVector(mesh));
        var maxTask = Task.Run(() => GetMaxVector(mesh));
        Task.WaitAll(minTask, maxTask);

        Vector3 min = minTask.Result;
        Vector3 max = maxTask.Result;

        var size = max - min;
        float longestSide = Math.Max(Math.Max(size.X, size.Y), size.Z);
        if(longestSide <= 0.0f) longestSide = 1.0f;
        float scaling = 1.0f / longestSide;
        Matrix4x4 scalingMatrix = Matrix4x4.CreateScale(scaling);

        Matrix4x4 transformationMatrix = MeshTransformation.CreateTransformationMatrix(
            scaling: new Vector3(scaling));

        mesh.ApplyTransformation(transformationMatrix);
    }

    public static T Normalize<T>(this T mesh) where T : MeshData
    {
        NormalizeMesh(mesh);
        return mesh;
    }
}
