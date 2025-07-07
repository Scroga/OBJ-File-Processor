using OBJProcessor;
using System.Numerics;
using System.Reflection.PortableExecutable;
namespace OBJProcessorTests;

public class GeneralParsingTests {
    [Fact]
    public void InvalidFileFormat()
    {
        var reader = new OBJFileReader(new StringReader("JO"));
        Assert.Throws<InvalidDataException>(() =>
        {
            var data = reader.ReadMeshData();
        });
    }
}

public class ParseToVec3Tests {
    [Fact]
    public void ParseToVec3_InvalidInput_TooFewArgs()
    {
        var reader = new OBJFileReader(new StringReader(""));
        string[] input = { "1", "2" };
        Assert.Throws<InvalidDataException>(() =>
        {
            var vec = reader.ParseToVec3(input);
        });
    }

    [Fact]
    public void ParseToVec3_InvalidInput_IsNotVec4()
    {
        var reader = new OBJFileReader(new StringReader(""));
        string[] input = { "1", "2", "0", "2" };
        Assert.Throws<InvalidDataException>(() =>
        {
            var vec = reader.ParseToVec3(input, false);
        });
    }

    [Fact]
    public void ParseToVec3_InvalidInput_WrongDataType()
    {
        var reader = new OBJFileReader(new StringReader(""));
        string[] input = { "1", "jojo", "0", "2" };
        Assert.Throws<InvalidDataException>(() =>
        {
            var vec = reader.ParseToVec3(input);
        });
    }

    [Fact]
    public void ParseToVec3_ValidInput_01()
    {
        var reader = new OBJFileReader(new StringReader(""));
        string[] input = { "1", "32", "0", "1" };
        var expected = new Vector3(1, 32, 0);
        Assert.Equal(expected, reader.ParseToVec3(input));
    }

    [Fact]
    public void ParseToVec3_ValidInput_02()
    {
        var reader = new OBJFileReader(new StringReader(""));
        string[] input = { "1", "32", "0", "2" };
        var expected = new Vector3(0.5f, 16, 0);
        Assert.Equal(expected, reader.ParseToVec3(input));
    }


    [Fact]
    public void ParseToVec3_ValidInput_IsNotVec4()
    {
        var reader = new OBJFileReader(new StringReader(""));
        string[] input = { "1", "32", "2"};
        var expected = new Vector3(1, 32, 2);
        Assert.Equal(expected, reader.ParseToVec3(input, false));
    }
}

public class VertexDataParsingTests
{
    [Fact]
    public void InvalidDataFormat_01()
    {
        var reader = new OBJFileReader(new StringReader("v 1.0"));
        Assert.Throws<InvalidDataException>(() =>
        {
            var data = reader.ReadMeshData();
        });
    }

    [Fact]
    public void InvalidDataFormat_02()
    {
        var reader = new OBJFileReader(new StringReader("v 1.0 1.0 1.0 1.0 1.0"));
        Assert.Throws<InvalidDataException>(() =>
        {
            var data = reader.ReadMeshData();
        });
    }


    [Fact]
    public void ValidData_SimpleTest()
    {
        var reader = new OBJFileReader(new StringReader("v 1.0 1.0 1.0 1.0"));
        var data = reader.ReadMeshData();
        var expected = new MeshVertex(new Vector4(1));
        Assert.Equal(expected.Position, data.Vertices[0].Position);
    }


    [Fact]
    public void ValidData_3Coords()
    {
        var reader = new OBJFileReader(new StringReader(
            """
            v 1.0 2.0 3.0
            """));

        var data = reader.ReadMeshData();
        var expected = new MeshVertex(new Vector4(1, 2, 3, 1));
        Assert.Equal(expected.Position, data.Vertices[0].Position);
    }

    [Fact]
    public void ValidData_ExplicitFloat()
    {
        var reader = new OBJFileReader(new StringReader(
            """
            v 1.1 2.2 3.3 3.0
            """));

        var data = reader.ReadMeshData();
        var expected = new MeshVertex(new Vector4(1.1f / 3.0f, 2.2f / 3.0f, 3.3f / 3.0f, 1));
        Assert.Equal(expected.Position, data.Vertices[0].Position);
    }

    [Fact]
    public void MultipleValidData()
    {
        var reader = new OBJFileReader(new StringReader(
            """
            v 1.0 2.0 3.0
            v 1.1 2.2 3.3 1.0
            v 4.0 3.0 2.0 1.0
            """));

        var data = reader.ReadMeshData();
        var expected1 = new MeshVertex(new Vector4(1, 2, 3, 1));
        var expected2 = new MeshVertex(new Vector4(1.1f, 2.2f, 3.3f, 1));
        var expected3 = new MeshVertex(new Vector4(4, 3, 2, 1));
        Assert.Equal(expected1.Position, data.Vertices[0].Position);
        Assert.Equal(expected2.Position, data.Vertices[1].Position);
        Assert.Equal(expected3.Position, data.Vertices[2].Position);
    }

}

public class NormalDataParsingTests {
    [Fact]
    public void InvalidDataFormat()
    {
        var reader = new OBJFileReader(new StringReader(
            """
            vn 1.1 2.2 3.3 3.0
            """));

        Assert.Throws<InvalidDataException>(() =>
        {
            var data = reader.ReadMeshData();
        });
    }

    [Fact]
    public void ValidDataFormat()
    {
        var reader = new OBJFileReader(new StringReader(
            """
            vn 0.1 0.3 0.2
            """));

        var data = reader.ReadMeshData();
        var expected = new Vector3(0.1f, 0.3f, 0.2f);
        Assert.Equal(expected, data.Normals[0]);
    }
}

public class FaceDataParsingTests {


}