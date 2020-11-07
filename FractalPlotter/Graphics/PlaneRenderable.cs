using OpenTK.Mathematics;

namespace FractalPlotter.Graphics
{
	/// <summary>
	/// Class designed to render the viewport plane, on which the fractals get displayed.
	/// Since there only needs to be , only one renderable is necessary.
	/// </summary>
	public static class PlaneRenderable
	{
		static Renderable renderable;

		public static void Load()
		{
			renderable = new Renderable(getVectors());
		}

		public static void Render()
		{
			renderable?.Render();
		}

		public static void UpdateBuffer()
		{
			renderable?.ChangeBuffer(getVectors());
		}

		public static void Dispose()
		{
			renderable?.Dispose();
		}

		static Vector[] getVectors()
		{
			return new[]
			{
				new Vector(new Vector4(-1, -1, 0, 1), Color4.White, new Vector2(-Camera.Ratio * 2f, -1 * 2f)),
				new Vector(new Vector4(-1, 1, 0, 1), Color4.White, new Vector2(-Camera.Ratio * 2f, 1 * 2f)),
				new Vector(new Vector4(1, -1, 0, 1), Color4.White, new Vector2(Camera.Ratio * 2f, -1 * 2f)),
				new Vector(new Vector4(1, -1, 0, 1), Color4.White, new Vector2(Camera.Ratio * 2f, -1 * 2f)),
				new Vector(new Vector4(-1, 1, 0, 1), Color4.White, new Vector2(-Camera.Ratio * 2f, 1 * 2f)),
				new Vector(new Vector4(1, 1, 0, 1), Color4.White, new Vector2(Camera.Ratio * 2f, 1 * 2f)),
			};
		}
	}
}
