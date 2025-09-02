using OBJProcessor;
using OBJProcessor.DataWriters;
using System.Numerics;

namespace OBJProcessorTests;

public class MeshDataWriterBaseTests
{
    [Fact]
    public void ConvertFloatToString_Zero()
    {
        var dataWriter = new VertexDataWriter("some tag");
        Assert.Equal("0.0", dataWriter.ConvertFloatToString(0));
    }

    [Fact]
    public void ConvertFloatToString_PositiveValue_01()
    {
        var dataWriter = new VertexDataWriter("some tag");
        Assert.Equal("2.0", dataWriter.ConvertFloatToString(2));
    }

    [Fact]
    public void ConvertFloatToString_PositiveValue_03()
    {
        var dataWriter = new VertexDataWriter("some tag");
        Assert.Equal("2.0003", dataWriter.ConvertFloatToString(2.0003f));
    }

    [Fact]
    public void ConvertFloatToString_NegativeValue_01()
    {
        var dataWriter = new VertexDataWriter("some tag");
        Assert.Equal("-9.0", dataWriter.ConvertFloatToString(-9));
    }

    [Fact]
    public void ConvertFloatToString_NegativeValue_03()
    {
        var dataWriter = new VertexDataWriter("some tag");
        Assert.Equal("-43.00032", dataWriter.ConvertFloatToString(-43.00032f));
    }

    [Fact]
    public void WriteVector2D_01()
    {
        var dataWriter = new VertexDataWriter("some tag");
        Assert.Equal(" 1.0 1.0", dataWriter.WriteVector(new Vector2(1.0f)));
    }

    [Fact]
    public void WriteVector2D_2()
    {
        var dataWriter = new VertexDataWriter("some tag");
        Assert.Equal(" 1.0321 0.0", dataWriter.WriteVector(new Vector2(1.0321f, 0.0f)));
    }

    [Fact]
    public void WriteVector3D()
    {
        var dataWriter = new VertexDataWriter("some tag");
        Assert.Equal(" 1.0321 0.0 -0.21", dataWriter.WriteVector(new Vector3(1.0321f, 0.0f, -0.21f)));
    }

    [Fact]
    public void WriteVector4D()
    {
        var dataWriter = new VertexDataWriter("some tag");
        Assert.Equal(" -4.021 2.022 -0.023 1.0", dataWriter.WriteVector(new Vector4(8.042f, -4.044f, 0.046f, -2.0f)));
    }

}

public class VertexDataWriterTests
{
    [Fact]
    public void EmptyMesh()
    {
        var meshData = new MeshData();
        var vertexWriter = new VertexDataWriter("Tag");

        var stringWriter = new StringWriter();
        vertexWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """

            """;

        Assert.Equal(expected, stringWriter.ToString());
    }

    [Fact]
    public void OneVertex_Vector3D()
    {
        var meshData = new MeshData();
        meshData.Vertices.Add(new MeshVertex(new Vector3(0.1f)));

        var vertexWriter = new VertexDataWriter("v");

        var stringWriter = new StringWriter();
        vertexWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """
            v 0.1 0.1 0.1 1.0

            """;

        Assert.Equal(expected, stringWriter.ToString());
    }

    [Fact]
    public void OneVertex_Vector4D()
    {
        var meshData = new MeshData();
        meshData.Vertices.Add(new MeshVertex(new Vector4(2.2f, 4.04f, 2.0022f, -2.0f)));

        var vertexWriter = new VertexDataWriter("v");

        var stringWriter = new StringWriter();
        vertexWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """
            v -1.1 -2.02 -1.0011 1.0

            """;

        Assert.Equal(expected, stringWriter.ToString());
    }

    [Fact]
    public void MultipleVertices_DifferentTypes()
    {
        var meshData = new MeshData();
        meshData.Vertices.Add(new MeshVertex(new Vector4(2.2f, 4.04f, 2.0022f, -2.0f)));
        meshData.Vertices.Add(new MeshVertex(new Vector3(2.2f)));
        meshData.Vertices.Add(new MeshVertex(new Vector3(-32.2f)));
        meshData.Vertices.Add(new MeshVertex(new Vector4(1.2f, 4.04f, -2.0022f, 1.0f)));

        var vertexWriter = new VertexDataWriter("v");

        var stringWriter = new StringWriter();
        vertexWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """
            v -1.1 -2.02 -1.0011 1.0
            v 2.2 2.2 2.2 1.0
            v -32.2 -32.2 -32.2 1.0
            v 1.2 4.04 -2.0022 1.0

            """;

        Assert.Equal(expected, stringWriter.ToString());
    }
}

public class UVDataWriterTests
{
    [Fact]
    public void EmptyUvCoords()
    {
        var meshData = new MeshData();

        var uvCoordsWriter = new UVDataWriter("vt");

        var stringWriter = new StringWriter();
        uvCoordsWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """

            """;

        Assert.Equal(expected, stringWriter.ToString());
    }


    [Fact]
    public void OneUvCoord()
    {
        var meshData = new MeshData();
        meshData.UVCoords.Add(new PtrWrapper<Vector2>(new Vector2(0.1f, -0.2f)));

        var uvCoordsWriter = new UVDataWriter("vt");

        var stringWriter = new StringWriter();
        uvCoordsWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """
            vt 0.1 -0.2

            """;

        Assert.Equal(expected, stringWriter.ToString());
    }

    [Fact]
    public void MultipleUvCoords()
    {
        var meshData = new MeshData();
        meshData.UVCoords.Add(new PtrWrapper<Vector2>(new Vector2(0.1f)));
        meshData.UVCoords.Add(new PtrWrapper<Vector2>(new Vector2(0.1f, -0.2f)));
        meshData.UVCoords.Add(new PtrWrapper<Vector2>(new Vector2(0.3f)));
        meshData.UVCoords.Add(new PtrWrapper<Vector2>(new Vector2(0.1f, -0.3223f)));
        meshData.UVCoords.Add(new PtrWrapper<Vector2>(new Vector2(0.123456f)));

        var uvCoordsWriter = new UVDataWriter("vt");

        var stringWriter = new StringWriter();
        uvCoordsWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """
            vt 0.1 0.1
            vt 0.1 -0.2
            vt 0.3 0.3
            vt 0.1 -0.3223
            vt 0.123456 0.123456

            """;

        Assert.Equal(expected, stringWriter.ToString());
    }
}

public class NormalDataWriterTests
{
    [Fact]
    public void EmptyNormals()
    {
        var meshData = new MeshData();

        var NormalsWriter = new NormalDataWriter("vn");

        var stringWriter = new StringWriter();
        NormalsWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """

            """;

        Assert.Equal(expected, stringWriter.ToString());
    }


    [Fact]
    public void OneNormal()
    {
        var meshData = new MeshData();
        meshData.Normals.Add(new PtrWrapper<Vector3>(new Vector3(0.1f, -0.2f, 0.0f)));

        var normalsWriter = new NormalDataWriter("vn");

        var stringWriter = new StringWriter();
        normalsWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """
            vn 0.1 -0.2 0.0

            """;

        Assert.Equal(expected, stringWriter.ToString());
    }

    [Fact]
    public void MultipleNormals()
    {
        var meshData = new MeshData();
        meshData.Normals.Add(new PtrWrapper<Vector3>(new Vector3(0.1f)));
        meshData.Normals.Add(new PtrWrapper<Vector3>(new Vector3(0.1f, -0.2f, 0.01f)));
        meshData.Normals.Add(new PtrWrapper<Vector3>(new Vector3(0.3f)));
        meshData.Normals.Add(new PtrWrapper<Vector3>(new Vector3(0.1f, -0.3223f, 0.032f)));
        meshData.Normals.Add(new PtrWrapper<Vector3>(new Vector3(0.123456f)));

        var normalsWriter = new NormalDataWriter("vn");

        var stringWriter = new StringWriter();
        normalsWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """
            vn 0.1 0.1 0.1
            vn 0.1 -0.2 0.01
            vn 0.3 0.3 0.3
            vn 0.1 -0.3223 0.032
            vn 0.123456 0.123456 0.123456

            """;

        Assert.Equal(expected, stringWriter.ToString());
    }

}

public class FaceDataWriterTests
{
    [Fact]
    public void EmptyFaces()
    {
        var meshData = new MeshData();

        var facesWriter = new FaceDataWriter("f");

        var stringWriter = new StringWriter();
        facesWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """

            """;

        Assert.Equal(expected, stringWriter.ToString());
    }

    [Fact]
    public void OneFace_OnlyVertexIndex()
    {
        var meshData = new MeshData();

        var face = new Face();
        face.Vertices.Add(new VertexData(1, 0, 0));
        face.Vertices.Add(new VertexData(2, 0, 0));
        face.Vertices.Add(new VertexData(3, 0, 0));
        face.Vertices.Add(new VertexData(4, 0, 0));

        meshData.Faces.Add(face);

        var facesWriter = new FaceDataWriter("f");

        var stringWriter = new StringWriter();
        facesWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """
            f 1 2 3 4

            """;

        Assert.Equal(expected, stringWriter.ToString());
    }

    [Fact]
    public void OneFace_VertexIndexAndUVCoord()
    {
        var meshData = new MeshData();

        var face = new Face();
        face.Vertices.Add(new VertexData(1, 4, 0));
        face.Vertices.Add(new VertexData(2, 5, 0));
        face.Vertices.Add(new VertexData(3, 6, 0));

        meshData.Faces.Add(face);

        var facesWriter = new FaceDataWriter("f");

        var stringWriter = new StringWriter();
        facesWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """
            f 1/4 2/5 3/6

            """;

        Assert.Equal(expected, stringWriter.ToString());
    }

    [Fact]
    public void OneFace_VertexIndexAndNormal()
    {
        var meshData = new MeshData();

        var face = new Face();
        face.Vertices.Add(new VertexData(1, 0, 4));
        face.Vertices.Add(new VertexData(2, 0, 5));
        face.Vertices.Add(new VertexData(3, 0, 6));

        meshData.Faces.Add(face);

        var facesWriter = new FaceDataWriter("f");

        var stringWriter = new StringWriter();
        facesWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """
            f 1//4 2//5 3//6

            """;

        Assert.Equal(expected, stringWriter.ToString());
    }

    [Fact]
    public void OneFace_AllVertexData()
    {
        var meshData = new MeshData();

        var face = new Face();
        face.Vertices.Add(new VertexData(1, 7, 4));
        face.Vertices.Add(new VertexData(2, 8, 5));
        face.Vertices.Add(new VertexData(3, 9, 6));

        meshData.Faces.Add(face);

        var facesWriter = new FaceDataWriter("f");

        var stringWriter = new StringWriter();
        facesWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """
            f 1/7/4 2/8/5 3/9/6

            """;

        Assert.Equal(expected, stringWriter.ToString());
    }

    [Fact]
    public void MultipleFaces_01()
    {
        var meshData = new MeshData();

        var face1 = new Face();
        face1.Vertices.Add(new VertexData(1, 0, 0));
        face1.Vertices.Add(new VertexData(2, 0, 0));
        face1.Vertices.Add(new VertexData(3, 0, 0));
        meshData.Faces.Add(face1);

        var face2 = new Face();
        face2.Vertices.Add(new VertexData(3, 1, 0));
        face2.Vertices.Add(new VertexData(4, 2, 0));
        face2.Vertices.Add(new VertexData(5, 3, 0));
        meshData.Faces.Add(face2);

        var face3 = new Face();
        face3.Vertices.Add(new VertexData(6, 4, 1));
        face3.Vertices.Add(new VertexData(3, 5, 3));
        face3.Vertices.Add(new VertexData(7, 6, 5));
        meshData.Faces.Add(face3);

        var face4 = new Face();
        face4.Vertices.Add(new VertexData(6, 0, 1));
        face4.Vertices.Add(new VertexData(3, 0, 3));
        face4.Vertices.Add(new VertexData(7, 0, 5));
        meshData.Faces.Add(face4);

        var facesWriter = new FaceDataWriter("f");

        var stringWriter = new StringWriter();
        facesWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """
            f 1 2 3
            f 3/1 4/2 5/3
            f 6/4/1 3/5/3 7/6/5
            f 6//1 3//3 7//5

            """;

        Assert.Equal(expected, stringWriter.ToString());

    }

    [Fact]
    public void MultipleFaces_02()
    {
        var meshData = new MeshData();

        var face1 = new Face();
        face1.Vertices.Add(new VertexData(1, 7, 4));
        face1.Vertices.Add(new VertexData(2, 8, 5));
        face1.Vertices.Add(new VertexData(3, 9, 6));
        meshData.Faces.Add(face1);

        var face2 = new Face();
        face2.Vertices.Add(new VertexData(10, 70, 40));
        face2.Vertices.Add(new VertexData(20, 80, 50));
        face2.Vertices.Add(new VertexData(30, 90, 60));
        meshData.Faces.Add(face2);

        var face3 = new Face();
        face3.Vertices.Add(new VertexData(11, 74, 47));
        face3.Vertices.Add(new VertexData(22, 85, 58));
        face3.Vertices.Add(new VertexData(33, 96, 69));
        meshData.Faces.Add(face3);

        var facesWriter = new FaceDataWriter("f");

        var stringWriter = new StringWriter();
        facesWriter.WriteMeshData(stringWriter, meshData);
        var expected =
            """
            f 1/7/4 2/8/5 3/9/6
            f 10/70/40 20/80/50 30/90/60
            f 11/74/47 22/85/58 33/96/69

            """;

        Assert.Equal(expected, stringWriter.ToString());

    }
}