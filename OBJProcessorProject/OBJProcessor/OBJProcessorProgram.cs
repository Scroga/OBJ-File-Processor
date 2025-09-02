using System.Globalization;
using CommandLine;
using OBJProcessor.DataBuilders;
using OBJProcessor.DataWriters;

namespace OBJProcessor;

class OBJProcessorProgram : IProgram
{
    private OBJProcessorArgsParser _parsedArgs;
    private MeshData? _meshData;

    public OBJProcessorProgram(string[] args)
    {
        Parser.Default.ParseArguments<OBJProcessorArgsParser>(args)
            .WithParsed(options => { _parsedArgs = options; });
        if (_parsedArgs == null) throw new InvalidOperationException("Arguments were not parsed");
    }

    public void LoadMeshData()
    {
        var fileReader = new OBJDataReader();
        fileReader.SetBuilder(new VertexDataBuilder())
            .SetBuilder(new UVDataBuilder())
            .SetBuilder(new NormalDataBuilder())
            .SetBuilder(new FaceDataBuilder());
        _meshData = fileReader.ReadMeshData(new StreamReader(_parsedArgs.InputFilePath));
    }

    public void Run()
    {
        LoadMeshData();
        ProcessMeshData();
        CreateOutputFile();
        PreviewOutputMesh();
    }

    public void CreateOutputFile()
    {
        var fileWriter = new OBJDataWriter();

        fileWriter.SetWriter(new VertexDataWriter(VertexDataBuilder.VERTEX_TAG))
            .SetWriter(new UVDataWriter(UVDataBuilder.UV_TAG))
            .SetWriter(new NormalDataWriter(NormalDataBuilder.NORMAL_TAG))
            .SetWriter(new FaceDataWriter(FaceDataBuilder.FACE_TAG));

        using (var fs = new FileStream(_parsedArgs.OutputFileName, FileMode.Create, FileAccess.Write))
        using (var writer = new StreamWriter(fs))
        {
            fileWriter.WriteMeshData(writer, _meshData!);
        }
    }

    private void ProcessMeshData()
    {
        // TODO:
    }

    private void PreviewOutputMesh()
    {
        // TODO:
    }
}
