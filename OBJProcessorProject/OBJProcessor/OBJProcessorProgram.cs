using System.Globalization;
using CommandLine;

namespace OBJProcessor;

class OBJProcessorProgram : IProgram
{
    private OBJProcessorArgsParser _parsedArgs;
    private MeshData _meshData;

    public OBJProcessorProgram(string[] args) {
        Parser.Default.ParseArguments<OBJProcessorArgsParser>(args)
        .WithParsed(options => { _parsedArgs = options; });
        if (_parsedArgs == null) throw new InvalidOperationException("Arguments were not parsed");

        var fileReader = new OBJFileReader(new StreamReader(_parsedArgs.InputFilePath));
        _meshData = fileReader.ReadMeshData();
    }

    public void Run() {
        ProcessMeshData();
        CreateOutputFile();
        PreviewOutputMesh();
    }

    public void CreateOutputFile() {
        var fileWriter = new OBJFileWriter(new StreamWriter(_parsedArgs.OutputFileName));
        fileWriter.WriteMeshData(_meshData);
    }

    private void ProcessMeshData() {
        // TODO:
    }

    private void PreviewOutputMesh() {
        // TODO:
    }
}
