using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;

namespace ComplexNumberGrapher.Graphics
{
	public class ShaderProgram
	{
		public readonly int ID;

		readonly List<int> shaders = new List<int>();
		readonly string name;

		public ShaderProgram(string name)
		{
			this.name = name;
			ID = GL.CreateProgram();
		}

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
