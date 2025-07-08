## Specification

> OBJ Transformation and Optimization Tool

This program is a command-line C# application designed to process 3D model stored in [.obj format](https://en.wikipedia.org/wiki/Wavefront_.obj_file#:~:text=The%20OBJ%20file%20format%20is,of%20vertices%2C%20and%20texture%20vertices.) and apply several geometric and topological cleanup operations on the model and generate a new, optimized `.obj` file.
To view the resulting 3D model using the open-source 3D modeling application *Blender*, the path to the *Blender* application executable file should be provided as a command-line argument. Otherwise, the program will create the output file but will not preview the result.
To ensure high performance, parallelization techniques will be used. Ensuring a high software quality, parts of the program should be tested where it makes sense. For these unit and integration tests, the *xUnit Test Project* will be used.

### Program execution
The program will get command-line arguments, parse them, and set the execution properties based on the provided arguments. Then, data from the `.obj` file will be read and stored. After this, geometric operations will be applied in the correct order to avoid conflicts (for example, scale first, then normalized). Iteration through the stored mesh data will be performed in parallel where possible to increase the program's performance. Finally, the program will write the data of the modified mesh to the output `.obj` file and run the Python code to open *Blender* and preview the model.

## Operations
- **Mesh transformations** applies user-defined transformations such as scale, translate, rotate. Each vertex will be stored as a vector 4D (to provide homogeneous of matrix multiplication) and then multiplied by proper transformation matrix.
- **Model Normalization** scales the entire model to fit within a unit cube (1,1,1).
- **Topolgy Cleanup** removes isolated vertices, faces with zero-area.

## Optional Operations
The following operations are planned but will be implemented only if they prove feasible in terms of algorithmic and implementation complexity:

- **Triangulation** converts all polygonal faces to triangles.
- **Face Orientation Fix** ensures all faces have consistent normal direction.
- **Merge Vertices by Tolerance** merges vertices that are within a small distance threshold (epsilon).

## Command Line arguments
The user initiates the workflow by providing input parameters through command line arguments. 

- `-i` or `--input <path/to/file.obj>` specifies the path to the input `.obj` file. 

**Optional Arguments**
- `-b` or `--blender <path/to/blender.exe>` sets the path to the blender execution file.

- `-o` or `--output <filename.obj>` specifies the name of the output `.obj` file. 

- `-t` or `--translate <x,y,z>` defines the translation vector along each axis.
- `-s` or `--scale <x,y,z>` defines the scaling vector along each axis. 
- `-r` or `--rotate <x,y,z>` defines a rotation around the axis (angles are given in degrees).

If the optional arguments of transformation operations are not provided, those operations will not be applied.
Additional parameters will be added as necessary.

## Software and Tools Used
This subsection provides an overview of the software, tools, and technologies that will be used throughout the development of the project.

- **Visual Studio 2022** will serve as the primary integrated development environment (IDE) for implementing and debugging the C# code. 

- **Blender** will be used for viewing the resulting 3D model.

 - **Python** will be used as an additional programming language for writing scripts to interact with the *Blender* program.