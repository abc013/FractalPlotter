using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;

namespace ComplexNumberGrapher.Graphics
{
	/// <summary>
	/// Class representing a point in 3D space.
	/// </summary>
	public class Point
	{
		public Color4 Color = Color4.White;
		public float Size = 6f;
		public Vector3d Position = Vector3d.Zero;

		public Point() { }

		public void Render()
		{
			var posID = UniformManager.PositionID;
			var colorID = UniformManager.ColorID;
			var color = (Vector4)Color;

			// Draw a point.
			GL.PointSize(Size);
			GL.VertexAttrib4(colorID, color);

			GL.Begin(PrimitiveType.Points);
			GL.VertexAttrib4(posID, new Vector4d(Position, 1.0d));
			GL.End();

			// Render lines to give a sense where the point is exactly.
			GL.Begin(PrimitiveType.Lines);
			GL.VertexAttrib4(posID, new Vector4d(Position + new Vector3d(0.05d / Camera.Scale.X, 0, 0), 1.0f));
			GL.VertexAttrib4(posID, new Vector4d(Position - new Vector3d(0.05d / Camera.Scale.X, 0, 0), 1.0f));
			GL.VertexAttrib4(posID, new Vector4d(Position + new Vector3d(0, 0.05d / Camera.Scale.X, 0), 1.0f));
			GL.VertexAttrib4(posID, new Vector4d(Position - new Vector3d(0, 0.05d / Camera.Scale.X, 0), 1.0f));
			GL.End();

			// Check for GL errors.
			Utils.CheckError("Render");
		}
	}
}
