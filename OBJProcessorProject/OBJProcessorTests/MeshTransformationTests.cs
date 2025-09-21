using OBJProcessor;
using OBJProcessor.MeshOperations;
using System.Numerics;

namespace OBJProcessorTests;

public class MeshTransformationTests
{
    private List<Vector3> _vertexPositions = new() {
        new Vector3(0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
    };

    private MeshData CreateMesh()
    {
        var mesh = new MeshData();
        foreach (var vertexPosition in _vertexPositions)
        {
            mesh.Vertices.Add(new MeshVertex(vertexPosition));
        }
        return mesh;
    }

    private void AssertAreEqual(Vector3 expected, Vector4 actualVector4)
    {
        var W = actualVector4.W;
        var actual = new Vector3(actualVector4.X / W, actualVector4.Y / W, actualVector4.Z / W);
        float distance = Vector3.Distance(expected, actual);
        float tolerance = 0.01f;

        string outputMessage = $" Expected: <{expected.X}, {expected.Y}, {expected.Z}>\n" +
                               $" Actual:   <{actual.X}, {actual.Y}, {actual.Z}>";
        Assert.True(distance < tolerance, outputMessage);
    }

    [Fact]
    public void NoTransformation()
    {
        var mesh = CreateMesh();
        var transformationMatrix = Matrix4x4.Identity;
        mesh.ApplyTransformation(transformationMatrix);

        for (int i = 0; i < mesh.Vertices.Count; i++)
        {
            AssertAreEqual(_vertexPositions[i], mesh.Vertices[i]!.Position);
        }
    }

    [Fact]
    public void Translation_01()
    {
        var mesh = CreateMesh();
        var translation = new Vector3(0.0f, 0.0f, 0.0f);
        var transformationMatrix = Matrix4x4.CreateTranslation(translation);
        mesh.ApplyTransformation(transformationMatrix);

        for (int i = 0; i < mesh.Vertices.Count; i++)
        {
            AssertAreEqual(_vertexPositions[i] + translation, mesh.Vertices[i]!.Position);
        }
    }

    [Fact]
    public void Translation_02()
    {
        var mesh = CreateMesh();
        var translation = new Vector3(1.0f, 0.0f, 0.0f);
        var transformationMatrix = Matrix4x4.CreateTranslation(translation);
        mesh.ApplyTransformation(transformationMatrix);

        for (int i = 0; i < mesh.Vertices.Count; i++)
        {
            AssertAreEqual(_vertexPositions[i] + translation, mesh.Vertices[i]!.Position);
        }
    }

    [Fact]
    public void Translation_03()
    {
        var mesh = CreateMesh();
        var translation = new Vector3(100.0f, 540.0f, 33210.0f);
        var transformationMatrix = Matrix4x4.CreateTranslation(translation);
        mesh.ApplyTransformation(transformationMatrix);

        for (int i = 0; i < mesh.Vertices.Count; i++)
        {
            AssertAreEqual(_vertexPositions[i] + translation, mesh.Vertices[i]!.Position);
        }
    }

    [Fact]
    public void Translation_04()
    {
        var mesh = CreateMesh();
        var translation = new Vector3(100.11111f, 0.424f, -332.424f);
        var transformationMatrix = Matrix4x4.CreateTranslation(translation);
        mesh.ApplyTransformation(transformationMatrix);

        for (int i = 0; i < mesh.Vertices.Count; i++)
        {
            AssertAreEqual(_vertexPositions[i] + translation, mesh.Vertices[i]!.Position);
        }
    }

    [Fact]
    public void Rotation_01()
    {
        var mesh = CreateMesh();
        var transformationMatrix = Matrix4x4.CreateRotationX(MathF.PI / 2.0f); // 90
        mesh.ApplyTransformation(transformationMatrix);

        List<Vector3> expectedVertexPositions = new(){
        new Vector3(0.0f),
        new Vector3(0.0f, -1.0f, 0.0f),
        new Vector3(1.0f, -1.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        };

        for (int i = 0; i < mesh.Vertices.Count; i++)
        {
            AssertAreEqual(expectedVertexPositions[i], mesh.Vertices[i]!.Position);
        }
    }

    //[Fact]
    //public void Rotation_02()
    //{
    //    var mesh = CreateMesh();
    //    var rotationMatrixByX = Matrix4x4.CreateRotationX(5 * MathF.PI / 4.0f); // 225
    //    var rotationMatrixByY = Matrix4x4.CreateRotationY(MathF.PI / 2.0f); // 90

    //    var transformationMatrix = rotationMatrixByX * rotationMatrixByY;

    //    mesh.ApplyTransformation(transformationMatrix);

    //    List<Vector3> expectedVertexPositions = new(){
    //    new Vector3(0.0f),
    //    new Vector3(0.0f, -1.0f, 0.0f),
    //    new Vector3(-0.7f, 1.0f, 7.0f),
    //    new Vector3(-0.7f, 0.0f, 7.0f),
    //    }; 

    //    for (int i = 0; i < mesh.Vertices.Count; i++)
    //    {
    //        AssertAreEqual(expectedVertexPositions[i], mesh.Vertices[i].Position);
    //    }
    //}
}
