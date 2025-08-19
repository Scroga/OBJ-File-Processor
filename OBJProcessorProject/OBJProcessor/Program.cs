namespace OBJProcessor;

internal class Program {
    static void Main(string[] args) {
        try {
            var program = new OBJProcessorProgram(args);
            program.Run();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }
}
