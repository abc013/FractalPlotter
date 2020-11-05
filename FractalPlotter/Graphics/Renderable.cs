using OpenTK.Graphics.OpenGL;
using System;

namespace ComplexNumberGrapher.Graphics
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

		public Renderable(Vector[] array)
		{
			// generate a GL Buffer with a plane in it and load it into GPU storage.
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

		public void ChangeBuffer(Vector[] array)
		{
			if (disposed || array.Length != length)
				return;

			GL.BindVertexArray(arrayID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);
			GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, Vector.Size * length, array);
		}

		public void Render(PrimitiveType type = PrimitiveType.Triangles, int start = 0, int customLength = 0)
		{
			if (disposed)
				return;

			customLength = customLength == 0 ? length : customLength;

			GL.BindVertexArray(arrayID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);
			GL.DrawArrays(type, start, customLength);
		}

		public void Dispose()
		{
			disposed = true;

			GL.DeleteBuffer(bufferID);
			GL.DeleteVertexArray(arrayID);
		}
	}
}
