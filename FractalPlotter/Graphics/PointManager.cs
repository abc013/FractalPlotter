using OpenTK.Mathematics;
using System.Collections.Generic;

namespace ComplexNumberGrapher.Graphics
{
	/// <summary>
	/// Class that keeps track of all points.
	/// </summary>
	public static class PointManager
	{
		static readonly List<Point> points = new List<Point>();

		public static Point Add(Vector3 position, Color4 color)
		{
			var point = new Point(position, color);

			points.Add(point);
			return point;
		}

		public static void Remove(Point point)
		{
			points.Remove(point);
		}

		public static void Render()
		{
			foreach (var point in points)
				point.Render();
		}

		public static void Dispose()
		{
			points.Clear();
		}
	}
}
