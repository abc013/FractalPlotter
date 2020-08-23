using FractalPlotter.FractalPlotter;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;
using System;
using System.Diagnostics;

namespace ComplexNumberGrapher
{
	public static class Utils
	{
		static readonly Random random = new Random();

		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		public static void CheckError(string position)
		{
			var error = GL.GetError();

			if (error != ErrorCode.NoError)
				throw new GraphicsException(position, error);
		}

		public static float ToFloat(string text, float @default = 1f)
		{
			if (string.IsNullOrWhiteSpace(text) || text.Equals(".") || text.Equals("-"))
				return @default;

			if (float.TryParse(text, out var result))
				return result;
			
			return @default;
		}

		public static double ToDouble(string text)
		{
			if (string.IsNullOrWhiteSpace(text) || text.Equals(".") || text.Equals("-"))
				return 0;

			if (double.TryParse(text, out var result))
				return result;

			return 0;
		}

		public static Color4 RandomColor(float a = 1f)
		{
			var array = new[] {
				(float)random.NextDouble() * 0.5f + 0.25f,
				(float)random.NextDouble() * 0.5f + 0.25f,
				(float)random.NextDouble() * 0.5f + 0.25f,
			};

			var r = array[0];
			if (random.NextDouble() > 0.5f)
				r = 1 - array[1];

			var g = array[1];
			if (random.NextDouble() > 0.5f)
				g = 1 - array[2];

			var b = array[2];
			if (random.NextDouble() > 0.5f)
				b = 1 - array[0];

			return new Color4(r, g, b, a);
		}
	}
}
