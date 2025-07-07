using CommandLine;
using System.Numerics;

namespace OBJProcessor;

public class OBJProcessorArgsParser
{
    [Option('i', "input", Required = true, HelpText = "specifies the path to the input `.obj` file")]
    public string InputFilePath { get; set; } = "input.obj";

    [Option('o', "output", HelpText = "specifies the name of the output `.obj` file")]
    public string OutputFileName { get; set; } = "output.obj";

    [Option('b', "blender", HelpText = "specifies the path to the blender.exe file")]
    public string BlenderExePath { get; set; } = "blender.exe";

    [Option('t', "translate", HelpText = "defines the translation vector along each axis")]
    public Vector3 Translation { get; set; } = new Vector3(0, 0, 0);
    [Option('s', "scale", HelpText = "defines the scaling vector along each axis")]
    public Vector3 Scaling { get; set; } = new Vector3(1, 1, 1);

    [Option('r', "rotate", HelpText = "defines a rotation around the axis (angles are given in degrees)")]
    public Vector3 Rotation { get; set; } = new Vector3(0, 0, 0);
}
