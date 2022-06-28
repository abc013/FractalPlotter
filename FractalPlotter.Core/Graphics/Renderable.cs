using OpenTK.Graphics.OpenGL;
using System;

namespace FractalPlotter.Graphics
{
	/// <summary>
	/// Generate a renderable that can be loaded into GPU memory and then rendered.
	/// </summary>
	public class Renderable
	{
		readonly int bufferID;
		readonly int arrayID;
		readonly int length;
		bool disposed;

		/// <summary>
		/// generate a GL Buffer with the given vectors and load it into GPU storage.
		/// </summary>
		/// <param name="array"></param>
		public Renderable(Vector[] array)
		{
			bufferID = GL.GenBuffer();
			arrayID = GL.GenVertexArray();
			length = array.Length;

			GL.BindVertexArray(arrayID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);
			GL.BufferData(BufferTarget.ArrayBuffer, Vector.Size * length, IntPtr.Zero, BufferUsageHint.DynamicRead);
			ChangeBuffer(array);

			GL.EnableVertexAttribArray(UniformManager.PositionID);
			GL.VertexAttribPointer(UniformManager.PositionID, 4, VertexAttribPointerType.Float, true, Vector.Size, 0);

			GL.EnableVertexAttribArray(UniformManager.ColorID);
			GL.VertexAttribPointer(UniformManager.ColorID, 4, VertexAttribPointerType.Float, true, Vector.Size, 16);

			GL.EnableVertexAttribArray(UniformManager.TexCoordID);
			GL.VertexAttribPointer(UniformManager.TexCoordID, 2, VertexAttribPointerType.Float, true, Vector.Size, 32);
		}

		/// <summary>
		/// Change the vertices stored in the buffer.
		/// </summary>
		/// <param name="array"></param>
		public void ChangeBuffer(Vector[] array)
		{
			if (disposed || array.Length != length)
				return;

			GL.BindVertexArray(arrayID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);
			GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, Vector.Size * length, array);
		}

		/// <summary>
		/// Renders the vertices given.
		/// </summary>
		/// <param name="type">Tells GL in what way to render the vertices</param>
		/// <param name="start">Parameter that determines from which index the vertices should be rendered</param>
		/// <param name="customLength">Determines how many vertices should be rendered. If set to 0, the full count is rendered.</param>
		public void Render(PrimitiveType type = PrimitiveType.Triangles, int start = 0, int customLength = 0)
		{
			if (disposed)
				return;

			customLength = customLength == 0 ? length : customLength;

			GL.BindVertexArray(arrayID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);
			GL.DrawArrays(type, start, customLength);
		}

		/// <summary>
		/// Free the buffer properly.
		/// </summary>
		public void Dispose()
		{
			disposed = true;

			GL.DeleteBuffer(bufferID);
			GL.DeleteVertexArray(arrayID);
		}
	}
}
