using OpenTK.Mathematics;

namespace FractalPlotter.Graphics
{
	/// <summary>
	/// Class storing a point in 3D space.
	/// </summary>
	public class Point
	{
		readonly Matrix4 objectMatrix;
		readonly Color4 color;

		public Point(Vector3 position, Color4 color, float size = 0.01f)
		{
			objectMatrix = Matrix4.CreateScale(size) * Matrix4.CreateTranslation(position);
			this.color = color;
		}

		/// <summary>
		/// Renders the point: The stored data of this point gets piped into the shader via uniforms.
		/// Then, the static PointRenderable renders the vertice buffer.
		/// </summary>
		public void Render()
		{
			MasterRenderer.DefaultManager.UniformModelView(Camera.InverseScaleMatrix * objectMatrix);
			MasterRenderer.DefaultManager.UniformColor(color);
			PointRenderable.Render();
		}
	}
}
