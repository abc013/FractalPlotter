using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;

namespace ComplexNumberGrapher.Graphics
{
	public class Point
	{
		public Color4 Color = Color4.White;
		public float Size = 6f;
		public Vector3d Position = Vector3d.Zero;

		public Point() { }

		public void Render()
		{
			GL.PointSize(Size);
			GL.Begin(PrimitiveType.Points);
			GL.Color4(Color);
			GL.Vertex3(Position);
			GL.End();

			GL.Begin(PrimitiveType.Lines);
			GL.Color4(Color);
			GL.Vertex3(Position + new Vector3d(0.05d / Camera.Scale.X, 0, 0));
			GL.Color4(Color);
			GL.Vertex3(Position - new Vector3d(0.05d / Camera.Scale.X, 0, 0));
			GL.Color4(Color);
			GL.Vertex3(Position + new Vector3d(0, 0.05d / Camera.Scale.X, 0));
			GL.Color4(Color);
			GL.Vertex3(Position - new Vector3d(0, 0.05d / Camera.Scale.X, 0));
			GL.End();

			Utils.CheckError("Render");
		}
	}
}
