using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using CommandLine;
using OBJProcessor.DataBuilders;
using OBJProcessor.DataWriters;
using OBJProcessor.DataProcessors;

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
        fileReader.SetBuilder(new VertexDataBuilder())
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
        _meshData!.RemoveFacesWithZeroArea();
        _meshData!.RemoveIsolatedVertices();

        var translation = new Vector3(0.0f, 4.0f, 0.0f);
        var scaling = new Vector3(3.0f);
        var rotation = new Vector3(0.0f, 0.0f, 45.0f);

        var transformer = new MeshDataTransformer(translation, scaling, rotation);
        transformer.ProcessMeshData(_meshData!);
    }

    private void PreviewOutputMesh()
    {
        //string pythonScript = "C:\\Users\\illy_\\Desktop\\Prog\\C#Project\\run_blender_with_obj.py";
        //string blenderPath = "C:\\Program Files\\Blender Foundation\\Blender 4.5\\blender.exe";
        //string modelPath = "C:\\Users\\illy_\\Desktop\\Prog\\C#Project\\OBJProcessorProject\\OBJProcessor\\bin\\Debug\\net8.0\\OutputMesh.obj";

        //string args = blenderPath + ' ' + modelPath;

        //var pis = new ProcessStartInfo("python " + pythonScript, args);
        //using var process = Process.Start(pis);

        //process!.WaitForExit();
    }
}