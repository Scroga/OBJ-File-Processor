using System.Globalization;
using CommandLine;
using OBJProcessor.DataBuilders;

namespace OBJProcessor;

class OBJProcessorProgram : IProgram
{
    private OBJProcessorArgsParser _parsedArgs;
    private MeshData? _meshData;

    public OBJProcessorProgram(string[] args) {
        Parser.Default.ParseArguments<OBJProcessorArgsParser>(args)
        .WithParsed(options => { _parsedArgs = options; });
        if (_parsedArgs == null) throw new InvalidOperationException("Arguments were not parsed");
    }

    public void LoadMeshData() {
        var fileReader = new OBJDataReader();
        fileReader.SetBuilder(new VertexDataBuilder())
            .SetBuilder(new UVDataBuilder())
            .SetBuilder(new NormalDataBuilder())
            //.SetBuilder(new ParametricSpacesDataBuilder())
            .SetBuilder(new FaceDataBuilder());

        _meshData = fileReader.ReadMeshData(new StreamReader(_parsedArgs.InputFilePath));
    }

    public void Run() {
        LoadMeshData();
        ProcessMeshData();
        CreateOutputFile();
        PreviewOutputMesh();
    }

    public void CreateOutputFile() {
        var fileWriter = new OBJFileWriter(new StreamWriter(_parsedArgs.OutputFileName));
        fileWriter.WriteMeshData(_meshData!);
    }

    private void ProcessMeshData() {
        // TODO:
    }

    private void PreviewOutputMesh() {
        // TODO:
    }
}
