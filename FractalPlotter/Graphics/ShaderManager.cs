using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace FractalPlotter.Graphics
{
	/// <summary>
	/// Class that keeps track of all shaders and their names.
	/// </summary>
	public static class ShaderManager
	{
		static readonly Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
		static readonly Dictionary<int, UniformManager> managers = new Dictionary<int, UniformManager>();

		public static void Add(string name)
		{
			var program = new ShaderProgram(name);
			var files = FileManager.GetGraphShaders(name);

			foreach (var file in files)
				program.AddShader(file.EndsWith(".frag") ? ShaderType.FragmentShader : ShaderType.VertexShader, file);
			program.Link();

			Utils.CheckError("AddShader " + name);

			UniformManager manager;
			try
			{
				manager = new UniformManager(name, program.ID);
			}
			catch (System.Exception e)
			{
				Log.WriteException(e);
				Log.WriteInfo($"could not initialize shader {name}. See shader info for details.");
				return;
			}

			shaders.Add(name, program);
			managers.Add(program.ID, manager);
		}

		public static int Fetch(string name)
		{
			if (string.IsNullOrEmpty(name) || !shaders.ContainsKey(name))
				return -1;

			return shaders[name].ID;
		}

		public static UniformManager FetchManager(int id)
		{
			return managers[id];
		}

		public static void Dispose()
		{
			foreach (var shader in shaders.Values)
				shader.Dispose();
		}
	}
}
