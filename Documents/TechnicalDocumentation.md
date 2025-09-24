## Technical Documentation

> OBJ Transformation and Optimization Tool

This program is a command-line C# application designed to process 3D model stored in [.obj format](https://en.wikipedia.org/wiki/Wavefront_.obj_file#:~:text=The%20OBJ%20file%20format%20is,of%20vertices%2C%20and%20texture%20vertices.) and apply several geometric and topological cleanup operations on the model and generate a new, optimized `.obj` file. To view the resulting 3D model using the open-source 3D modeling application *Blender*, the path to the *Blender* application executable file will be provided as a command-line argument. Otherwise, the program will create the output file but will not preview the result.

### External Libraries
To avoid manual parsing logic, the progam uses external `CommandLine` library.

### High-Level Program Architecture

Firstly, the program takes and parses the command-line arguments using the `OBJProcessorArgsParser`, which is implemented with the help of `CommandLine` library. After argument parsing, the core program unit `OBJProcessorProgram` takes arguments and starts reading, processing and then writing the `obj` file.
The program reads the `.obj` file using the `OBJDataReader` class and then stores the data in the `MeshData` class. The `MeshData` object is then processed with `MeshOperations` and written to the output `.obj` file using the `OBJDataWriter` class.

### Data Flow Diagram

<img src="../Data/DataFlowDiagram.svg" alt="Diagram" style="width:100%;">

### Program Components


### Notes and Suggestions

### Conclusion