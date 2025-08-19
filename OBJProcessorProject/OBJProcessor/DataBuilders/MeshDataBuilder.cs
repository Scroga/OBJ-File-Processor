using System.Globalization;

namespace OBJProcessor.DataBuilders;

public abstract class MeshDataBuilder
{
    protected const string ERROR_MESSAGE = "The file format is invalid or corrupted";

    protected float ParseToFloat(string number)
    {
        if (float.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
        {
            return value;
        }
        throw new InvalidDataException(ERROR_MESSAGE);
    }

    public abstract bool CanProcess(string tag);
    public abstract void BuildMeshData(MeshData meshData, string[] line);
}

// TODO: PARAMETER_SPACE= "vp"
