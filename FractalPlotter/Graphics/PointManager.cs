using OpenToolkit.Mathematics;
using System.Collections.Generic;

namespace ComplexNumberGrapher.Graphics
{
	public static class PointManager
	{
		static readonly List<Point> points = new List<Point>();

		public static Point Add(Vector3d position, Color4 color)
		{
			var point = new Point
			{
				Position = position,
				Color = color
			};

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
	}
}
