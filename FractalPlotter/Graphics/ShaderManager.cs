using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace FractalPlotter.Graphics
{
	/// <summary>
	/// Class that keeps track of all shaders with names and their corresponding UniformManagers.
	/// </summary>
	public static class ShaderManager
	{
		static readonly Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
		static readonly Dictionary<int, UniformManager> managers = new Dictionary<int, UniformManager>();

		/// <summary>
		/// Load a shader and check whether it works.
		/// </summary>
		/// <param name="name">Name of the shader.</param>
		public static void Add(string name)
		{
			// Load the shader
			var program = new ShaderProgram(name);
			var files = FileManager.GetGraphShaders(name);

			foreach (var file in files)
				program.AddShader(file.EndsWith(".frag") ? ShaderType.FragmentShader : ShaderType.VertexShader, file);
			program.Link();

			// Check for any GL errors.
			Utils.CheckError("AddShader " + name);

			// After shader creation, create the corresponding manager
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

			// Add both the the lists
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

		/// <summary>
		/// Clean up.
		/// </summary>
		public static void Dispose()
		{
			foreach (var shader in shaders.Values)
				shader.Dispose();
		}
	}
}
