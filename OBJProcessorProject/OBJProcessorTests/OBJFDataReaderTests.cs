using System.Numerics;
using OBJProcessor;
using OBJProcessor.DataBuilders;
namespace OBJProcessorTests;

public class OBJDataReaderTests
{

    private MeshData _expectedMeshData;

    public OBJDataReaderTests()
    {
        // TODO:
        _expectedMeshData = new();
        _expectedMeshData.Vertices.Add(new MeshVertex(new Vector3(1.0f)));
        _expectedMeshData.Vertices.Add(new MeshVertex(new Vector3(2.0f)));
        _expectedMeshData.Vertices.Add(new MeshVertex(new Vector3(3.0f)));

        _expectedMeshData.Normals.Add(new Vector3(0.1f));
        _expectedMeshData.Normals.Add(new Vector3(0.2f));
        _expectedMeshData.Normals.Add(new Vector3(0.3f));

        _expectedMeshData.UVs.Add(new Vector2(0.11f));
        _expectedMeshData.UVs.Add(new Vector2(0.22f));
        _expectedMeshData.UVs.Add(new Vector2(0.33f));

        Face face1 = new(); 
        Face face2 = new(); 
        Face face3 = new();

        face1.AddVertex(1).AddVertex(2).AddVertex(3);
        face2.AddVertex(1).AddVertex(2).AddVertex(3);
    }

    [Fact]
    public void SkipCommantsTests()
    {
        var reader = new OBJDataReader();
        reader.SetBuilder(new VertexDataBuilder());
        var actualMeshData = new MeshData();
        actualMeshData = reader.ReadMeshData(new StringReader(
            """
            #Bab bam
            v 1 1 1
            # Bim bim

            """));
        Assert.Single(actualMeshData.Vertices);
        Assert.Empty(actualMeshData.Normals);
        Assert.Empty(actualMeshData.UVs);
        Assert.Empty(actualMeshData.Faces);
    }

    [Fact]
    public void SmallAmountOfData()
    {
        var reader = new OBJDataReader();
        reader.SetBuilder(new VertexDataBuilder()).SetBuilder(new NormalDataBuilder());

    }
}