using System.Numerics;

namespace OBJProcessor.MeshOperations;

public static class MeshTransformation
{
    public static T ApplyTransformation<T>(this T mesh, Matrix4x4 transformationMatrix) where T : MeshData {
        Parallel.ForEach(mesh.Vertices, vertex => {
            vertex.Position = Vector4.Transform(vertex.Position, transformationMatrix);
        });
        return mesh;
    }
}
