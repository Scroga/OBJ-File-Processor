using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using CommandLine;
using OBJProcessor.DataBuilders;
using OBJProcessor.DataWriters;
using OBJProcessor.MeshOperations;

namespace OBJProcessor;

class OBJProcessorProgram : IProgram
{
    private string PYTHON_SCRIPT = "run_blender_with_obj.py";
    private string PROJECT_ROOT_DIR = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));

    private OBJProcessorArgsParser _parsedArgs;
    private MeshData? _meshData;

    public OBJProcessorProgram(string[] args)
    {
        Parser.Default.ParseArguments<OBJProcessorArgsParser>(args)
            .WithParsed(options => { _parsedArgs = options; });
        if (_parsedArgs == null) throw new InvalidOperationException("Arguments were not parsed");
    }

    public void Run()
    {
        LoadMeshData();
        ProcessMeshData();
        CreateOutputFile();
        PreviewOutputMesh();
    }

    public void LoadMeshData()
    {
        var fileReader = new OBJDataReader();
        fileReader
            .SetBuilder(new VertexDataBuilder())
            .SetBuilder(new UVDataBuilder())
            .SetBuilder(new NormalDataBuilder())
            .SetBuilder(new FaceDataBuilder());

        using var reader = new StreamReader(_parsedArgs.InputFilePath);
        {
            _meshData = fileReader.ReadMeshData(reader);
        }
    }

    public void CreateOutputFile()
    {
        var fileWriter = new OBJDataWriter();

        var delSync = new DeletionSynchronization();

        fileWriter
            .SetWriter(new VertexDataWriter(VertexDataBuilder.VERTEX_TAG, delSync))
            .SetWriter(new UVDataWriter(UVDataBuilder.UV_TAG))
            .SetWriter(new NormalDataWriter(NormalDataBuilder.NORMAL_TAG))
            .SetWriter(new FaceDataWriter(FaceDataBuilder.FACE_TAG, delSync));

        using (var fs = new FileStream(_parsedArgs.OutputFileName, FileMode.Create, FileAccess.Write))
        using (var writer = new StreamWriter(fs))
        {
            fileWriter.WriteMeshData(writer, _meshData!);
        }
    }

    private void ProcessMeshData()
    {
        var transformation = MeshTransformation.CreateTransformationMatrix(
            translation: _parsedArgs.Translation,
            scaling:     _parsedArgs.Scaling,
            rotation:    _parsedArgs.Rotation);

        _meshData!.RemoveFacesWithZeroArea().RemoveIsolatedVertices();
        _meshData!.ApplyTransformation(transformation);
        if (_parsedArgs.Normalize)
            _meshData!.Normalize();
    }

    private void PreviewOutputMesh()
    {
        if (_parsedArgs.BlenderExePath is null) return;

        string pythonScriptPath = Path.Combine(PROJECT_ROOT_DIR, PYTHON_SCRIPT);
        string blenderPath = _parsedArgs.BlenderExePath;
        string modelPath = _parsedArgs.OutputFileName;

        string args = string.Format(" \"{0}\" \"{1}\" \"{2}\"", pythonScriptPath, blenderPath, modelPath);

        var psi = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = args,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        using (Process? process = Process.Start(psi))
        {
            string? output = process?.StandardOutput.ReadToEnd();
            string? errors = process?.StandardError.ReadToEnd();
            process?.WaitForExit();

            Console.WriteLine(output);
            Console.WriteLine(errors);
        }
    }
}