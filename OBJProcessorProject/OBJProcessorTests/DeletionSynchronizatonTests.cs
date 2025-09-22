using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OBJProcessor;
using OBJProcessor.DataBuilders;
using OBJProcessor.DataWriters;
using OBJProcessor.MeshOperations;

namespace OBJProcessorTests;

public class DeletionSynchronizatonTests
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
    public void OnlyIsolatedVertices_OnlyVertexWriter()
    {
        string inputData =
            """
            v 1 2 3
            v 1 2 3

            """;

        var meshData = CreateMeshData(inputData);
        meshData.RemoveIsolatedVertices();

        var delSync = new DeletionSynchronization();
        var fileWriter = new OBJDataWriter();
        fileWriter
           .SetWriter(new VertexDataWriter(VertexDataBuilder.VERTEX_TAG, delSync));
        var writer = new StringWriter();
        fileWriter.WriteMeshData(writer, meshData!);
        var list = delSync.VertexSubtrahends();
        Assert.Equal(2, list.Count);
        Assert.Equal(1, list[0]);
        Assert.Equal(2, list[1]);
        Assert.Empty(writer.ToString());
    }

    [Fact]
    public void MoreVertices_OnlyVertexWriter()
    {
        string inputData =
            """
            v 1 2 3
            v 2 2 3
            v 3 2 3
            v 4 2 3
            v 5 2 3
            f 1 3 5
            """;

        var meshData = CreateMeshData(inputData);
        meshData.RemoveIsolatedVertices();

        var delSync = new DeletionSynchronization();
        var fileWriter = new OBJDataWriter();
        fileWriter
           .SetWriter(new VertexDataWriter(VertexDataBuilder.VERTEX_TAG, delSync));
        var writer = new StringWriter();
        fileWriter.WriteMeshData(writer, meshData!);
        var list = delSync.VertexSubtrahends();
        Assert.Equal(5, list.Count);
        Assert.Equal(0, list[0]);
        Assert.Equal(1, list[1]);
        Assert.Equal(1, list[2]);
        Assert.Equal(2, list[3]);
        Assert.Equal(2, list[4]);
    }

    [Fact]
    public void NoIsolatedVertex_WithFaceWriter()
    {
        string inputData =
            """
            v 1 2 3
            v 2 2 3
            v 3 2 3
            v 4 2 3
            f 1 3 4
            f 1 2 4
            """;

        string expectedOutput =
            """
            v 1.0 2.0 3.0
            v 2.0 2.0 3.0
            v 3.0 2.0 3.0
            v 4.0 2.0 3.0
            f 1 3 4
            f 1 2 4

            """;

        var meshData = CreateMeshData(inputData);
        meshData.RemoveIsolatedVertices();

        var delSync = new DeletionSynchronization();
        var fileWriter = new OBJDataWriter();
        fileWriter
           .SetWriter(new VertexDataWriter(VertexDataBuilder.VERTEX_TAG, delSync))
           .SetWriter(new FaceDataWriter(FaceDataBuilder.FACE_TAG, delSync));
        var writer = new StringWriter();
        fileWriter.WriteMeshData(writer, meshData!);
        var list = delSync.VertexSubtrahends();
        Assert.Equal(4, list.Count);
        Assert.Equal(0, list[0]);
        Assert.Equal(0, list[1]);
        Assert.Equal(0, list[2]);
        Assert.Equal(0, list[3]);
        Assert.Equal(expectedOutput, writer.ToString());
    }


    [Fact]
    public void MoreIsolatedVertex_WithFaceWriter()
    {
        string inputData =
            """
            v 1 2 3
            v 2 2 3
            v 3 2 3
            v 4 2 3
            f 1 3 4
            """;

        string expectedOutput =
            """
            v 1.0 2.0 3.0
            v 3.0 2.0 3.0
            v 4.0 2.0 3.0
            f 1 2 3

            """;

        var meshData = CreateMeshData(inputData);
        meshData.RemoveIsolatedVertices();

        var delSync = new DeletionSynchronization();
        var fileWriter = new OBJDataWriter();
        fileWriter
           .SetWriter(new VertexDataWriter(VertexDataBuilder.VERTEX_TAG, delSync))
           .SetWriter(new FaceDataWriter(FaceDataBuilder.FACE_TAG, delSync));
        var writer = new StringWriter();
        fileWriter.WriteMeshData(writer, meshData!);
        var list = delSync.VertexSubtrahends();
        Assert.Equal(4, list.Count);
        Assert.Equal(0, list[0]);
        Assert.Equal(1, list[1]);
        Assert.Equal(1, list[2]);
        Assert.Equal(1, list[3]);
        Assert.Equal(expectedOutput, writer.ToString());
    }

    [Fact]
    public void MoreIsolatedVertices_ComplexTestWithFaceWriter()
    {
        string inputData =
            """
            v 1 2 3
            v 2 2 3
            v 3 2 3
            v 4 2 3
            v 5 2 3
            v 6 2 3
            v 7 2 3
            v 8 2 3
            v 9 2 3
            v 10 2 3
            f 1 5 7
            f 1 2 5
            f 5 2 10
            """;

        string expectedOutput =
            """
            v 1.0 2.0 3.0
            v 2.0 2.0 3.0
            v 5.0 2.0 3.0
            v 7.0 2.0 3.0
            v 10.0 2.0 3.0
            f 1 3 4
            f 1 2 3
            f 3 2 5

            """;

        var meshData = CreateMeshData(inputData);
        meshData.RemoveIsolatedVertices();

        var delSync = new DeletionSynchronization();
        var fileWriter = new OBJDataWriter();
        fileWriter
           .SetWriter(new VertexDataWriter(VertexDataBuilder.VERTEX_TAG, delSync))
           .SetWriter(new FaceDataWriter(FaceDataBuilder.FACE_TAG, delSync));
        var writer = new StringWriter();
        fileWriter.WriteMeshData(writer, meshData!);
        var list = delSync.VertexSubtrahends();
        Assert.Equal(10, list.Count);
        Assert.Equal(expectedOutput, writer.ToString());
    }
}
