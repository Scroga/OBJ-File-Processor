using System.Numerics;
using OBJProcessor;
using OBJProcessor.DataBuilders;
using OBJProcessor.DataProcessors;
using OBJProcessor.DataWriters;
using OBJProcessor.MeshOperations;

namespace OBJProcessorTests;

public class RemovingIsolatedVerticesTest
{
    private MeshData CreateMeshData(string data)
    {
        var meshData = new MeshData();
        var fileWriter = new OBJDataWriter();

        var fileReader = new OBJDataReader();
        fileReader.SetBuilder(new VertexDataBuilder())
            .SetBuilder(new UVDataBuilder())
            .SetBuilder(new NormalDataBuilder())
            .SetBuilder(new FaceDataBuilder());

        using var reader = new StringReader(data);
        {
            meshData = fileReader.ReadMeshData(reader);
        }
        return meshData;
    }

    [Fact]
    public void OnlyVerticesIsolatedVertices()
    {
        string data =
            """
            v 1 2 3
            v 1 2 3

            """;

        var meshData = CreateMeshData(data);
        Assert.Equal(2, meshData.Vertices.Count);
        meshData.RemoveIsolatedVertices();
        Assert.Equal(2, meshData.Vertices.Count);
        Assert.Null(meshData.Vertices[0]);
        Assert.Null(meshData.Vertices[1]);
    }


    [Fact]
    public void OneIsolatedThreeAccessibleVertices()
    {
        string data =
            """
            v 1 2 3
            v 3 2 3
            v 3 2 3
            v 3 2 3

            f 2 3 4
            """;

        var meshData = CreateMeshData(data);
        Assert.Equal(4, meshData.Vertices.Count);
        meshData.RemoveIsolatedVertices();
        Assert.Equal(4, meshData.Vertices.Count);
        Assert.Null(meshData.Vertices[0]);
        Assert.NotNull(meshData.Vertices[1]);
        Assert.NotNull(meshData.Vertices[2]);
        Assert.NotNull(meshData.Vertices[3]);
    }

    [Fact]
    public void MoresolatedVertices()
    {
        string data =
            """
            v -1 -1 -1
            v 1 1 0
            v 1 2 0
            v 1 3 0
            v -2 -2 -2
            v 2 1 0
            v 2 2 0
            v 2 3 0
            v -3 -3 -3
            v 3 1 0
            v 3 2 0
            v 3 3 0

            f 2 3 4
            f 6 7 8
            f 10 11 12
            """;

        var meshData = CreateMeshData(data);
        Assert.Equal(12, meshData.Vertices.Count);
        meshData.RemoveIsolatedVertices();

        for (int i = 0; i < meshData.Vertices.Count; i++)
        {
            if ((i % 4) == 0)
            {
                Assert.Null(meshData.Vertices[i]);
            }
            else
            {
                Assert.NotNull(meshData.Vertices[i]);
            }
        }
    }
}

public class RemovingZeroAreaFacesTest
{
    private MeshData CreateMeshData(string data)
    {
        var meshData = new MeshData();
        var fileWriter = new OBJDataWriter();

        var fileReader = new OBJDataReader();
        fileReader.SetBuilder(new VertexDataBuilder())
            .SetBuilder(new UVDataBuilder())
            .SetBuilder(new NormalDataBuilder())
            .SetBuilder(new FaceDataBuilder());

        using var reader = new StringReader(data);
        {
            meshData = fileReader.ReadMeshData(reader);
        }
        return meshData;
    }

    [Fact]
    public void HasZeroAreaMethodTest_01()
    {
        string data =
            """
            v 1 1 1
            v 1 2 1
            v 1 3 1
            f 1 2 3
            """;

        var meshData = CreateMeshData(data);
        Assert.Single(meshData.Faces);
        Assert.Equal(3, meshData.Faces[0]!.Vertices.Count);
        Assert.False(TopologyCleanupExtention.HasZeroArea(meshData, 0));
    }

    [Fact]
    public void HasZeroAreaMethodTest_02()
    {
        string data =
            """
            v 0 0 0
            v 1 1 1
            v 2 2 2
            f 1 2 3
            """;

        var meshData = CreateMeshData(data);
        Assert.Single(meshData.Faces);
        Assert.Equal(3, meshData.Faces[0]!.Vertices.Count);
        Assert.True(TopologyCleanupExtention.HasZeroArea(meshData, 0));
    }

    [Fact]
    public void HasZeroAreaMethodTest_03()
    {
        string data =
            """
            v 5 -2 0
            v 15 -6 0
            v -10 4 0
            v 1 2 3
            f 1 4 3 2
            """;

        var meshData = CreateMeshData(data);
        Assert.Single(meshData.Faces);
        Assert.Equal(4, meshData.Faces[0]!.Vertices.Count);
        Assert.False(TopologyCleanupExtention.HasZeroArea(meshData, 0));
    }

    [Fact]
    public void ZeroAreaTriangle()
    {
        string data =
        """
            v 2 -4 4
            v 1 -2 2
            v -1 2 -2
            f 1 2 3
            """;

        var meshData = CreateMeshData(data);
        Assert.Single(meshData.Faces);
        Assert.Equal(3, meshData.Faces[0]!.Vertices.Count);
        meshData.RemoveFacesWithZeroArea();
        Assert.Null(meshData.Faces[0]);
    }

    [Fact]
    public void NormalTriangle()
    {
        string data =
        """
            v 2 -4 10
            v 1 -2 2
            v -1 2 -2
            f 1 2 3
            """;

        var meshData = CreateMeshData(data);
        Assert.Single(meshData.Faces);
        Assert.Equal(3, meshData.Faces[0]!.Vertices.Count);
        meshData.RemoveFacesWithZeroArea();
        Assert.NotNull(meshData.Faces[0]);
    }

    [Fact]
    public void NormalPolygon()
    {
        string data =
            """
            v 5 -2 1
            v 5 -2 2
            v 5 -2 3
            v 5 -2 4
            v 5 -2 0
            v 5 -2 0
            f 1 2 3 4 5 6
            """;

        var meshData = CreateMeshData(data);
        Assert.Single(meshData.Faces);
        Assert.Equal(6, meshData.Faces[0]!.Vertices.Count);
        meshData.RemoveFacesWithZeroArea();
        Assert.NotNull(meshData.Faces[0]);
    }

    [Fact]
    public void ZeroAreaPolygon_01()
    {
        string data =
            """
            v 5 -2 0
            v 5 -2 0
            v 5 -2 0
            v 5 -2 0
            v 5 -2 0
            v 5 -2 0
            f 1 2 3 4 5 6
            """;

        var meshData = CreateMeshData(data);
        Assert.Single(meshData.Faces);
        Assert.Equal(6, meshData.Faces[0]!.Vertices.Count);
        meshData.RemoveFacesWithZeroArea();
        Assert.Null(meshData.Faces[0]);
    }

    [Fact]
    public void ZeroAreaPolygon_02()
    {
        string data =
            """
            v 5 -2 0
            v 15 -6 0
            v -10 4 0
            v 10 -4 0
            v -5 2 0
            f 1 2 3 4 5
            """;

        var meshData = CreateMeshData(data);
        Assert.Single(meshData.Faces);
        Assert.Equal(5, meshData.Faces[0]!.Vertices.Count);
        meshData.RemoveFacesWithZeroArea();
        Assert.Null(meshData.Faces[0]);
    }

    [Fact]
    public void MoreZeroArePolygons()
    {
        string data =
            """
            v 5 -2 0
            v 15 -6 0
            v -10 4 0
            v 10 -4 0
            v -5 2 0
            v 1 0 4
            f 1 2 3 4 5
            f 1 6 2 3 4
            f 2 2 2 2
            """;

        var meshData = CreateMeshData(data);
        meshData.RemoveFacesWithZeroArea();
        Assert.Equal(3, meshData.Faces.Count);
        Assert.Null(meshData.Faces[0]!);
        Assert.NotNull(meshData.Faces[1]!);
        Assert.Null(meshData.Faces[2]!);
    }
}
