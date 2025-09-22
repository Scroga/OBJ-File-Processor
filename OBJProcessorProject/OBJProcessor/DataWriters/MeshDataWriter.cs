using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OBJProcessor.DataWriters;

public abstract class MeshDataWriter
{
    public string Tag { get; init; }
    protected DeletionSynchronization? _deletionSynchronization { get; set; }
    protected MeshDataWriter(string tag, DeletionSynchronization? deletionSynchronization = null)
    {
        Tag = tag;
        _deletionSynchronization = deletionSynchronization;
    }

    public string ConvertFloatToString(float number) {
        string numberAsString = number.ToString();
        return numberAsString.Contains('.') ? numberAsString : numberAsString + ".0";
    }

    public string WriteVector(Vector4 vector)
    {
        float W = vector.W;
        return WriteVector(new Vector3(vector.X / W, vector.Y / W, vector.Z / W));
    }

    public string WriteVector(Vector3 vector)
    {
        return 
            $" {ConvertFloatToString(vector.X)}" +
            $" {ConvertFloatToString(vector.Y)}" +
            $" {ConvertFloatToString(vector.Z)}";
    }
    public string WriteVector(Vector2 vector)
    {
        return
            $" {ConvertFloatToString(vector.X)}" +
            $" {ConvertFloatToString(vector.Y)}";
    }

    public abstract void WriteMeshData(TextWriter writer, MeshData mesh);
}
