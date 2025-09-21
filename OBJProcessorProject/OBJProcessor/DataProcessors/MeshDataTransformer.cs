using System;
using System.Numerics;

namespace OBJProcessor.DataProcessors;

public class MeshDataTransformer : MeshDataProcessor
{
    private Vector3 _translation;
    private Vector3 _scaling;
    private Vector3 _rotation;

    public MeshDataTransformer(Vector3 translation, Vector3 scaling, Vector3 rotation)
    {
        _translation = translation;
        _scaling = scaling;
        _rotation = rotation;
    }

    public override void ProcessMeshData(MeshData meshData)
    {
        Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(_translation);
        Matrix4x4 scalingMatrix = Matrix4x4.CreateScale(_scaling);
        Vector3 radians = _rotation * (MathF.PI / 180f);
        Matrix4x4 rotationMatrix =
            Matrix4x4.CreateRotationX(radians.X) * 
            Matrix4x4.CreateRotationY(radians.Y) *
            Matrix4x4.CreateRotationZ(radians.Z);

        Matrix4x4 transformMatrix = scalingMatrix * rotationMatrix * translationMatrix;

        Parallel.For(0, meshData.Vertices.Count, i =>
        {
            if (meshData.Vertices[i] is not null)
            {
                Vector4 newPosition = Vector4.Transform(meshData.Vertices[i]!.Position, transformMatrix);
                meshData.Vertices[i]!.Position = newPosition;
            }
        });
    }
}
