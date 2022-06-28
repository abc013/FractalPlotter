using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;

namespace FractalPlotter.Graphics
{
	/// <summary>
	/// Class used to load a shader.
	/// </summary>
	public class ShaderProgram
	{
		/// <summary>
		/// Shader ID in GL.
		/// </summary>
		public readonly int ID;

		readonly List<int> shaders = new List<int>();
		readonly string name;

		/// <summary>
		/// Loads a shader based on the given name.
		/// </summary>
		/// <param name="name">shader name.</param>
		public ShaderProgram(string name)
		{
			this.name = name;
			ID = GL.CreateProgram();
		}

		/// <summary>
		/// Add a given shader file to this shader program. The shader is compiled and then loaded into GPU memory.
		/// </summary>
		/// <param name="type">Type of the shaders.</param>
		/// <param name="path">Path to the shader file</param>
		public void AddShader(ShaderType type, string path)
		{
			var shader = GL.CreateShader(type);
			GL.ShaderSource(shader, File.ReadAllText(path));
			GL.CompileShader(shader);

			var info = GL.GetShaderInfoLog(shader);
			if (!string.IsNullOrWhiteSpace(info))
				Log.WriteInfo($"program {name}, shader {shader} information: " + info);

			shaders.Add(shader);
		}

		/// <summary>
		/// Tells GL that this shader is finished and can be used.
		/// </summary>
		public void Link()
		{
			foreach (var shader in shaders)
				GL.AttachShader(ID, shader);

			GL.LinkProgram(ID);

			foreach (var shader in shaders)
			{
				GL.DetachShader(ID, shader);
				GL.DeleteShader(shader);
			}
		}

		/// <summary>
		/// Properly free shader resources again.
		/// </summary>
		public virtual void Dispose()
		{
			dispose(true);
			GC.SuppressFinalize(this);
		}

		void dispose(bool disposing)
		{
			if (disposing)
				GL.DeleteProgram(ID);
		}
	}
}
