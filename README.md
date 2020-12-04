# FractalPlotter
FractalPlotter is a program written in c# to display and examine fractal structures. It supports switching fractals/shaders and color palettes seamlessly as well as adjusting parametes in runtime.
FractalPlotter is using a GL backend and shaders for each fractal structure, such as the Mandelbrot and Julia fractals. 

## Dependencies
### Framework: [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- [OpenTK](https://github.com/opentk/opentk)
- [ImGui.NET](https://github.com/mellinoe/ImGui.NET)
- System.Drawing.Common

## Download
Releases are available [here](https://github.com/abc013/FractalPlotter/releases).
The source code can either be [downloaded as zip](https://github.com/abc013/FractalPlotter/archive/master.zip) or cloned via `git` using:
```sh
git clone https://github.com/abc013/FractalPlotter.git
```

## Compiling
After installing the [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1), make a local copy of this repository.
Open a command line and navigate to the corresponding directory:
```sh
cd C:/example/path/to/downloaded/directory/FractalPlotter/
```
After doing so, just run the following command:
```sh
dotnet build
```
This command should fetch the dependencies via NuGet (make sure you have an internet connection!) and build the binaries for you. Done!

As text editors, Visual Studio 2019 or Visual Studio Code are recommended.

## Settings
Most options can be modified in the program. However, to get access to more settings or to have the same arguments at every startup, you can create a `settings.txt` in the same directory where the executable is. To see what can be modified, please take a look at the [Settings.cs file](https://github.com/abc013/FractalPlotter/blob/master/FractalPlotter/Settings.cs). An example of a `settings.txt` might be:
```
Points=false
DefaultShader=julia
Scale=1.5
Factor1X=-0.5
Factor1Y=0.55
```

## Issues
If you encounter any problems or bugs while compiling or running the program, feel free to [open an issue](https://github.com/abc013/FractalPlotter/issues/new)! Please don't forget to atttach the `exception.log` and `information.log` files.
### After Zooming in a while, the texture gets really pixelated!
Yes, this is a limitation brought with the standard 32-bit floats. They are not precise enough to display such deep zooming, thus the texture becoming pixelated.
However, also a 64-bit version of the implemented shaders also exists, which greatly improves quality at the cost of performance. However, these shaders may not be supported on certain devices.
