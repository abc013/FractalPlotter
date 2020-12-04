using OpenTK.Mathematics;
using System.Collections.Generic;

namespace FractalPlotter.Graphics
{
	/// <summary>
	/// Class that keeps track of all points.
	/// </summary>
	public static class PointManager
	{
		public static readonly List<Point> Points = new List<Point>();

		public static Point Add(Vector3 position, Color4 color)
		{
			var point = new Point(position, color);

			Points.Add(point);
			return point;
		}

		public static void Remove(Point point)
		{
			Points.Remove(point);
		}

		public static void Render()
		{
			foreach (var point in Points)
				point.Render();
		}

		public static void Dispose()
		{
			Points.Clear();
		}
	}
}
