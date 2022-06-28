using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FractalPlotter.Graphics
{
	/// <summary>
	/// Shader used for ImGui.
	/// Taken from https://github.com/NogginBops/ImGui.NET_OpenTK_Sample, thanks to NogginBops!
	/// </summary>
	class ImGuiShader
	{
		public int Program { get; private set; }

		readonly Dictionary<string, int> uniformLocations = new Dictionary<string, int>();

		public ImGuiShader(string vertexShader, string fragmentShader)
		{
			Program = GL.CreateProgram();

			var shaders = new int[2];
			shaders[0] = compileShader(ShaderType.VertexShader, vertexShader);
			shaders[1] = compileShader(ShaderType.FragmentShader, fragmentShader);

			foreach (var shader in shaders)
				GL.AttachShader(Program, shader);

			GL.LinkProgram(Program);

			GL.GetProgram(Program, GetProgramParameterName.LinkStatus, out int Success);
			if (Success == 0)
				Log.WriteInfo($"ImGuiShader generated following log: {GL.GetProgramInfoLog(Program)}");

			foreach (var Shader in shaders)
			{
				GL.DetachShader(Program, Shader);
				GL.DeleteShader(Shader);
			}
		}

		int compileShader(ShaderType type, string source)
		{
			var shader = GL.CreateShader(type);

			GL.ShaderSource(shader, source);
			GL.CompileShader(shader);

			GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
			if (success == 0)
				Log.WriteInfo($"ImGuiShader generated following log: {GL.GetShaderInfoLog(shader)}");

			return shader;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetUniformLocation(string uniform)
		{
			if (uniformLocations.TryGetValue(uniform, out int location) == false)
			{
				location = GL.GetUniformLocation(Program, uniform);
				uniformLocations.Add(uniform, location);

				if (location == -1)
					Log.WriteInfo($"The uniform '{uniform}' does not exist in the ImGui shader!");
			}

			return location;
		}

		public void Dispose()
		{
			GL.DeleteProgram(Program);
		}
	}
}
