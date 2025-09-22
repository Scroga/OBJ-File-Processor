using System.Numerics;

namespace OBJProcessor.MeshOperations;

public static class MeshTransformation
{
    public static Matrix4x4 CreateTransformationMatrix(
        Vector3? translation = null,
        Vector3? rotation = null,
        Vector3? scaling = null)
    {
        var translationMatrix = Matrix4x4.Identity;
        var rotationMatrix = Matrix4x4.Identity;
        var scalingMatrix = Matrix4x4.Identity;

        if (translation is not null)
            translationMatrix = Matrix4x4.CreateTranslation(translation.Value);

        if (rotation is not null)
        {
            Vector3 radians = rotation.Value * (MathF.PI / 180f);
            rotationMatrix =
                Matrix4x4.CreateRotationX(radians.X) *
                Matrix4x4.CreateRotationY(radians.Y) *
                Matrix4x4.CreateRotationZ(radians.Z);
        }
        if (scaling is not null)
            scalingMatrix = Matrix4x4.CreateScale(scaling.Value);

        return translationMatrix * rotationMatrix * scalingMatrix;
    }

    public static T ApplyTransformation<T>(this T mesh, Matrix4x4 transformationMatrix) where T : MeshData
    {
        Parallel.ForEach(mesh.Vertices, vertex =>
        {
            if (vertex is not null)
            {
                var newPosition = Vector4.Transform(vertex.Position, transformationMatrix);
                vertex!.Position = newPosition;
            }
        });
        return mesh;
    }
}
