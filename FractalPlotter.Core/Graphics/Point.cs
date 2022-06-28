using OpenTK.Mathematics;

namespace FractalPlotter.Graphics
{
	/// <summary>
	/// Class storing a point in 3D space.
	/// </summary>
	public class Point
	{
		public readonly Vector3 Position;
		public Color4 Color;

		readonly Matrix4 objectMatrix;

		public Point(Vector3 position, Color4 color, float size = 0.01f)
		{
			Position = position;
			Color = color;

			objectMatrix = Matrix4.CreateScale(size) * Matrix4.CreateTranslation(position);
		}

		/// <summary>
		/// Renders the point: The stored data of this point gets piped into the shader via uniforms.
		/// Then, the static PointRenderable renders the vertice buffer.
		/// </summary>
		public void Render()
		{
			MasterRenderer.DefaultManager.UniformModelView(Camera.InverseScaleMatrix * objectMatrix);
			MasterRenderer.DefaultManager.UniformColor(Color);
			PointRenderable.Render();
		}
	}
}
