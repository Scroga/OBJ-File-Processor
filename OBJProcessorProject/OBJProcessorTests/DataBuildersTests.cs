using OBJProcessor;
using OBJProcessor.DataBuilders;
using System.Numerics;

namespace OBJProcessorTests;

public class VertexDataBuilderTests
{
    [Fact]
    public void TagTest_ValidData()
    {
        var builder = new VertexDataBuilder();
        Assert.True(builder.CanProcess("v"));
    }

    [Fact]
    public void TagTest_InvalidData()
    {
        var builder = new VertexDataBuilder();
        Assert.False(builder.CanProcess("vp"));
    }

    [Fact]
    public void TagTest_EmptyInput()
    {
        var builder = new VertexDataBuilder();
        Assert.False(builder.CanProcess(""));
    }

    [Fact]
    public void InvalidData_TooFewArgs()
    {
        var builder = new VertexDataBuilder();
        string[] inputData = { "v", "1" };
        Assert.Throws<InvalidDataException>(() =>
        {
            builder.BuildMeshData(new MeshData(), inputData);
        });
    }

    [Fact]
    public void InvalidData_TooManyArgs()
    {
        var builder = new VertexDataBuilder();
        string[] inputData = { "v", "1.0", "2.0", "3.0", "4.0", "5.0" };
        Assert.Throws<InvalidDataException>(() =>
        {
            builder.BuildMeshData(new MeshData(), inputData);
        });
    }

    [Fact]
    public void InvalidData_CorruptedArg()
    {
        var builder = new VertexDataBuilder();
        string[] inputData = { "v", "1.0", "2.0", "jo", "4.0" };
        Assert.Throws<InvalidDataException>(() =>
        {
            builder.BuildMeshData(new MeshData(), inputData);
        });
    }

    [Fact]
    public void InvalidData_TooManyTags()
    {
        var builder = new VertexDataBuilder();
        string[] inputData = { "v", "vp", "2.0", "1.2", "4.0" };
        Assert.Throws<InvalidDataException>(() =>
        {
            builder.BuildMeshData(new MeshData(), inputData);
        });
    }

    [Fact]
    public void BuildMeshTest_PositiveValue()
    {
        var builder = new VertexDataBuilder();
        var meshData = new MeshData();
        string[] inputData = { "v", "1.0", "1.0", "1.0", "1.0" };
        builder.BuildMeshData(meshData, inputData);
        var expected = new MeshVertex(new Vector4(1));
        Assert.Equal(expected.Position, meshData.Vertices[0].Position);

    }

    [Fact]
    public void BuildMeshTest_NegativeValue()
    {
        var builder = new VertexDataBuilder();
        var meshData = new MeshData();
        string[] inputData = { "v", "-1.0", "1.0", "-1.0", "1.0" };
        builder.BuildMeshData(meshData, inputData);
        var expected = new MeshVertex(new Vector4(-1.0f, 1.0f, -1.0f, 1.0f));
        Assert.Equal(expected.Position, meshData.Vertices[0].Position);

    }

    [Fact]
    public void BuildMeshTest_SimpleData_ExplicitFloat()
    {
        var builder = new VertexDataBuilder();
        var meshData = new MeshData();
        string[] inputData = { "v", "1.0", "1.0", "1.0", "1.0" };
        builder.BuildMeshData(meshData, inputData);
        var expected = new MeshVertex(new Vector4(1));
        Assert.Equal(expected.Position, meshData.Vertices[0].Position);
    }

    [Fact]
    public void BuildMeshTest_Vec3Input()
    {
        var builder = new VertexDataBuilder();
        var meshData = new MeshData();
        string[] inputData = { "v", "1.1", "2.2", "3.3" };
        builder.BuildMeshData(meshData, inputData);
        var expected = new MeshVertex(new Vector4(1.1f, 2.2f, 3.3f, 1));
        Assert.Equal(expected.Position, meshData.Vertices[0].Position);
    }

    [Fact]
    public void BuildMeshTest_Vec4Input()
    {
        var builder = new VertexDataBuilder();
        var meshData = new MeshData();
        string[] inputData = { "v", "1.1", "2.2", "3.3", "4.4" };
        builder.BuildMeshData(meshData, inputData);
        var expected = new MeshVertex(new Vector4(1.1f / 4.4f, 2.2f / 4.4f, 3.3f / 4.4f, 1));
        Assert.Equal(expected.Position, meshData.Vertices[0].Position);
    }

    [Fact]
    public void BuildMeshTest_MultipleData()
    {
        var builder = new VertexDataBuilder();
        var meshData = new MeshData();
        string[] inputData1 = { "v", "1.1", "2.2", "3.3" };
        string[] inputData2 = { "v", "3", "2", "-1" };
        string[] inputData3 = { "v", "1.0", "1.0", "1.0", "1.0" };
        builder.BuildMeshData(meshData, inputData1);
        builder.BuildMeshData(meshData, inputData2);
        builder.BuildMeshData(meshData, inputData3);
        var expected1 = new MeshVertex(new Vector4(1.1f, 2.2f, 3.3f, 1));
        var expected2 = new MeshVertex(new Vector4(3, 2, -1, 1));
        var expected3 = new MeshVertex(new Vector4(1));
        Assert.Equal(expected1.Position, meshData.Vertices[0].Position);
        Assert.Equal(expected2.Position, meshData.Vertices[1].Position);
        Assert.Equal(expected3.Position, meshData.Vertices[2].Position);

    }
}

public class NormalDataBuilderTests
{
    [Fact]
    public void TagTest_ValidData()
    {
        var builder = new NormalDataBuilder();
        Assert.True(builder.CanProcess("vn"));
    }

    [Fact]
    public void TagTest_InvalidData()
    {
        var builder = new NormalDataBuilder();
        Assert.False(builder.CanProcess("v"));
    }

    [Fact]
    public void TagTest_EmptyInput()
    {
        var builder = new NormalDataBuilder();
        Assert.False(builder.CanProcess(""));
    }

    [Fact]
    public void InvalidData_TooFewArgs()
    {
        var builder = new NormalDataBuilder();
        string[] inputData = { "vn", "0.1", "0.1" };
        Assert.Throws<InvalidDataException>(() =>
        {
            builder.BuildMeshData(new MeshData(), inputData);
        });
    }

    [Fact]
    public void InvalidData_TooManyArgs()
    {
        var builder = new NormalDataBuilder();
        string[] inputData = { "vn", "0.1", "0.2", "0.1", "0.1" };
        Assert.Throws<InvalidDataException>(() =>
        {
            builder.BuildMeshData(new MeshData(), inputData);
        });
    }

    [Fact]
    public void InvalidData_CorruptedArg()
    {
        var builder = new NormalDataBuilder();
        string[] inputData = { "vn", "1.0", "0.1s", "1.0" };
        Assert.Throws<InvalidDataException>(() =>
        {
            builder.BuildMeshData(new MeshData(), inputData);
        });
    }

    [Fact]
    public void InvalidData_TooManyTags()
    {
        var builder = new NormalDataBuilder();
        string[] inputData = { "vn", "vp", "0.1", "0.1" };
        Assert.Throws<InvalidDataException>(() =>
        {
            builder.BuildMeshData(new MeshData(), inputData);
        });
    }

    [Fact]
    public void InvalidData_IsNotNormalized()
    {
        var builder = new NormalDataBuilder();
        string[] inputData = { "vn", "2.0", "0.0", "0.1" };
        Assert.Throws<InvalidDataException>(() =>
        {
            builder.BuildMeshData(new MeshData(), inputData);
        });
    }


    [Fact]
    public void BuildMeshTest_PositiveValue()
    {
        var builder = new NormalDataBuilder();
        var meshData = new MeshData();
        string[] inputData = { "vn", "1.0", "1.0", "1.0" };
        builder.BuildMeshData(meshData, inputData);
        var expected = new Vector3(1);
        Assert.Equal(expected, meshData.Normals[0]);

    }

    [Fact]
    public void BuildMeshTest_NegativeValue()
    {
        var builder = new NormalDataBuilder();
        var meshData = new MeshData();
        string[] inputData = { "vn", "1.0", "-0.1", "0.3" };
        builder.BuildMeshData(meshData, inputData);
        var expected = new Vector3(1.0f, -0.1f, 0.3f);
        Assert.Equal(expected, meshData.Normals[0]);

    }

    [Fact]
    public void BuildMeshTest_SimpleData_ExplicitFloat()
    {
        var builder = new NormalDataBuilder();
        var meshData = new MeshData();
        string[] inputData = { "vn", "1.0", "0.5", "0.0" };
        builder.BuildMeshData(meshData, inputData);
        var expected = new Vector3(1.0f, 0.5f, 0.0f);
        Assert.Equal(expected, meshData.Normals[0]);
    }

    [Fact]
    public void BuildMeshTest_MultipleData()
    {
        var builder = new NormalDataBuilder();
        var meshData = new MeshData();
        string[] inputData1 = { "vn", "0.1", "0.2", "0.3" };
        string[] inputData2 = { "vn", "0.3", "0.2", "0.1" };
        string[] inputData3 = { "vn", "0.0", "1.0", "1.0" };
        builder.BuildMeshData(meshData, inputData1);
        builder.BuildMeshData(meshData, inputData2);
        builder.BuildMeshData(meshData, inputData3);
        var expected1 = new Vector3(0.1f, 0.2f, 0.3f);
        var expected2 = new Vector3(0.3f, 0.2f, 0.1f);
        var expected3 = new Vector3(0.0f, 1.0f, 1.0f);
        Assert.Equal(expected1, meshData.Normals[0]);
        Assert.Equal(expected2, meshData.Normals[1]);
        Assert.Equal(expected3, meshData.Normals[2]);

    }
}

public class UVDataBuilderTests
{
    [Fact]
    public void TagTest_ValidData()
    {
        var builder = new UVDataBuilder();
        Assert.True(builder.CanProcess("vt"));
    }

    [Fact]
    public void TagTest_InvalidData()
    {
        var builder = new UVDataBuilder();
        Assert.False(builder.CanProcess("vn"));
    }

    [Fact]
    public void TagTest_EmptyInput()
    {
        var builder = new UVDataBuilder();
        Assert.False(builder.CanProcess(""));
    }

    [Fact]
    public void InvalidData_TooFewArgs()
    {
        var builder = new UVDataBuilder();
        string[] inputData = { "vt", "0.1" };
        Assert.Throws<InvalidDataException>(() =>
        {
            builder.BuildMeshData(new MeshData(), inputData);
        });
    }

    [Fact]
    public void InvalidData_TooManyArgs()
    {
        var builder = new UVDataBuilder();
        string[] inputData = { "vt", "0.1", "0.2", "0.3" };
        Assert.Throws<InvalidDataException>(() =>
        {
            builder.BuildMeshData(new MeshData(), inputData);
        });
    }

    [Fact]
    public void InvalidData_CorruptedArg()
    {
        var builder = new UVDataBuilder();
        string[] inputData = { "vt", "1.0", "0.1f" };
        Assert.Throws<InvalidDataException>(() =>
        {
            builder.BuildMeshData(new MeshData(), inputData);
        });
    }

    [Fact]
    public void InvalidData_TooManyTags()
    {
        var builder = new UVDataBuilder();
        string[] inputData = { "vt", "vp", "0.1" };
        Assert.Throws<InvalidDataException>(() =>
        {
            builder.BuildMeshData(new MeshData(), inputData);
        });
    }

    [Fact]
    public void InvalidData_IsNotNormalized()
    {
        var builder = new UVDataBuilder();
        string[] inputData = { "vt", "1.1", "0.1" };
        Assert.Throws<InvalidDataException>(() =>
        {
            builder.BuildMeshData(new MeshData(), inputData);
        });
    }

    [Fact]
    public void BuildMeshTest_PositiveValue()
    {
        var builder = new UVDataBuilder();
        var meshData = new MeshData();
        string[] inputData = { "vt", "1.0", "1.0"};
        builder.BuildMeshData(meshData, inputData);
        var expected = new Vector2(1);
        Assert.Equal(expected, meshData.UVs[0]);

    }

    [Fact]
    public void BuildMeshTest_NegativeValue()
    {
        var builder = new UVDataBuilder();
        var meshData = new MeshData();
        string[] inputData = { "vt", "1.0", "-0.3"};
        builder.BuildMeshData(meshData, inputData);
        var expected = new Vector2(1.0f, -0.3f);
        Assert.Equal(expected, meshData.UVs[0]);

    }

    [Fact]
    public void BuildMeshTest_SimpleData_ImplicitFloat()
    {
        var builder = new UVDataBuilder();
        var meshData = new MeshData();
        string[] inputData = { "vt", "1", "0.5"};
        builder.BuildMeshData(meshData, inputData);
        var expected = new Vector2(1.0f, 0.5f);
        Assert.Equal(expected, meshData.UVs[0]);
    }

    [Fact]
    public void BuildMeshTest_MultipleData()
    {
        var builder = new UVDataBuilder();
        var meshData = new MeshData();
        string[] inputData1 = { "vt", "0.1", "0.2"};
        string[] inputData2 = { "vt", "0.3", "0.2" };
        string[] inputData3 = { "vt", "0", "1.0" };
        builder.BuildMeshData(meshData, inputData1);
        builder.BuildMeshData(meshData, inputData2);
        builder.BuildMeshData(meshData, inputData3);
        var expected1 = new Vector2(0.1f, 0.2f);
        var expected2 = new Vector2(0.3f, 0.2f);
        var expected3 = new Vector2(0.0f, 1.0f);
        Assert.Equal(expected1, meshData.UVs[0]);
        Assert.Equal(expected2, meshData.UVs[1]);
        Assert.Equal(expected3, meshData.UVs[2]);
    }

}

public class FaceDataBuilderTests
{
    private MeshData _meshData;
    public FaceDataBuilderTests()
    {
        _meshData = new();
    }

    [Fact]
    public void TagTest_ValidData()
    {
        var builder = new FaceDataBuilder();
        Assert.True(builder.CanProcess("f"));
    }

    [Fact]
    public void TagTest_InvalidData()
    {
        var builder = new FaceDataBuilder();
        Assert.False(builder.CanProcess("fs"));
    }

    [Fact]
    public void TagTest_EmptyInput()
    {
        var builder = new FaceDataBuilder();
        Assert.False(builder.CanProcess(""));
    }

}