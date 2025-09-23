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
        var translation = new Vector3(0.0f, 4.0f, 0.0f);
        var scaling = new Vector3(5.0f);
        var rotation = new Vector3(0.0f, 0.0f, 45.0f);

        var transformation = MeshTransformation.CreateTransformationMatrix(rotation: rotation);

        _meshData!.RemoveFacesWithZeroArea();
        _meshData!.RemoveIsolatedVertices();
        _meshData!.ApplyTransformation(transformation);
        //_meshData!.Normalize();
    }

    private void PreviewOutputMesh()
    {
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