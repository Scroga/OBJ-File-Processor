using System.Globalization;
using System.Numerics;
using OBJProcessor.DataBuilders;

namespace OBJProcessor;

public class OBJDataReader
{
    public char COMMENT_TAG = '#'; 
    private List<MeshDataBuilder> _builders = new();

    public OBJDataReader SetBuilder(MeshDataBuilder builder) {
        _builders.Add(builder);
        return this;
    }

    public MeshData ReadMeshData(TextReader reader)
    {
        var meshData = new MeshData();

        string? line = reader.ReadLine();
        while (line != null)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                line = reader.ReadLine();
                continue;
            }
            string[] splitLine = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (splitLine[0].StartsWith(COMMENT_TAG)) continue;

            foreach (var builder in _builders)
            {
                if (builder.CanProcess(splitLine[0])) {
                    builder.BuildMeshData(meshData, splitLine);
                    line = reader.ReadLine();
                }     
            }
            line = reader.ReadLine();
        }
        return meshData;
    }
}