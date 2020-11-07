using OpenTK.Mathematics;

namespace FractalPlotter.Graphics
{
	/// <summary>
	/// Class representing a point in 3D space.
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

		public void Render()
		{
			MasterRenderer.DefaultManager.UniformModelView(Camera.InverseScaleMatrix * objectMatrix);
			MasterRenderer.DefaultManager.UniformColor(color);
			PointRenderable.Render();
		}
	}
}
