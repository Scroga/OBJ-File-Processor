using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace OBJProcessor.DataProcessors;

public static class TopologyCleanupExtention
{
    public static bool AreCollinear(List<Vector3> vertices, float epsilon = 0.0f)
    {
        if (vertices.Count < 3) return true;

        Vector3 v1 = vertices[0];
        Vector3 v2 = vertices[1];
        Vector3 dir = v2 - v1;

        for (int i = 2; i < vertices.Count; i++)
        {
            Vector3 p = vertices[i] - v1;
            Vector3 cross = Vector3.Cross(dir, p);
            if (cross.Length() > epsilon) return false;
        }

        return true;
    }

    public static bool HasZeroArea(MeshData meshData, int index)
    {
        var face = meshData.Faces[index];
        var vertices = new List<Vector3>();
        foreach (var vertexData in face!.Vertices)
        {
            vertices.Add(Vector3.Normalize(meshData.Vertices[vertexData.VertexIndex]!.GetAsVec3()));
        }
        return AreCollinear(vertices);
    }

    public static void RemoveFacesWithZeroAreaOnMesh(MeshData meshData)
    {
        Parallel.For(0, meshData.Faces.Count, i =>
        {
            if (meshData.Faces[i] is not null && HasZeroArea(meshData, i))
            {
                meshData.Faces[i] = null;
            }
        });
    }

    public static void RemoveIsolatedVerticesOnMesh(MeshData meshData)
    {
        Parallel.For(0, meshData.Vertices.Count, i =>
        {
            if (meshData.Vertices[i] is not null && meshData.Vertices[i]!.Faces.Count == 0)
            {
                meshData.Vertices[i] = null;
            }
        });
    }

    public static T RemoveIsolatedVertices<T>(this T mesh) where T : MeshData
    {
        RemoveIsolatedVerticesOnMesh(mesh);
        return mesh;
    }

    public static T RemoveFacesWithZeroArea<T>(this T mesh) where T : MeshData
    {
        RemoveFacesWithZeroAreaOnMesh(mesh);
        return mesh;
    }
}