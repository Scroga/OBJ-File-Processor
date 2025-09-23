using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using CommandLine;

namespace OBJProcessor;

public class OBJProcessorArgsParser
{
    [Option('i', "input", Required = true, HelpText = "specifies the path to the input `.obj` file")]
    public string InputFilePath { get; set; } = "input.obj";

    [Option('o', "output", HelpText = "specifies the name of the output `.obj` file")]
    public string OutputFileName { get; set; } = "output.obj";

    [Option('b', "blender", HelpText = "specifies the path to the blender.exe file")]
    public string? BlenderExePath { get; set; } = null;

    [Option('t', "translate", HelpText = "defines the translation vector along each axis")]
    public string _translationRaw { get; set; } = "0,0,0";

    [Option('s', "scale", HelpText = "defines the scaling vector along each axis")]
    public string _scalingRaw { get; set; } = "1,1,1";

    [Option('r', "rotate", HelpText = "defines a rotation around the axis (angles are given in degrees)")]
    public string _rotationRaw { get; set; } = "0,0,0";

    [Option('n', "normalize", HelpText = "normalizes the model to fit into a unit cube")]
    public bool Normalize { get; set; } = false;

    public Vector3 ConvertStringToVector3(string rawVector, string typeOfVector)
    {
        var parts = rawVector
                    .Split(',')
                    .Select(p => p.Trim())
                    .ToArray();

        if (parts.Length != 3)
            throw new ArgumentException(typeOfVector + " vector must have 3 components given in the following format x,y,z (without spaces)");

        return new Vector3(
            float.Parse(parts[0]),
            float.Parse(parts[1]),
            float.Parse(parts[2]));
    }

    public Vector3 Translation => ConvertStringToVector3(_translationRaw, "Translation");
    public Vector3 Scaling     => ConvertStringToVector3(_scalingRaw,     "Scaling");
    public Vector3 Rotation    => ConvertStringToVector3(_rotationRaw,    "Rotation");
}
