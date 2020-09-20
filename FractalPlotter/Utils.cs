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

		/// <summary>
		/// Method used to catch any GL errors and throw them.
		/// </summary>
		/// <param name="position">Argument to use in order to see where the error happened.</param>
		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		public static void CheckError(string position)
		{
			var error = GL.GetError();

			if (error != ErrorCode.NoError)
				throw new GraphicsException(position, error);
		}

		/// <summary>
		/// <c>String</c> to <c>float</c> conversion method. If empty, the <c>default</c> parameter will be used.
		/// </summary>
		/// <param name="text">Input to convert.</param>
		/// <param name="default">Default value if input is null, empty, <c>.</c> or <c>-</c>.</param>
		public static float ToFloat(string text, float @default = 1f)
		{
			if (string.IsNullOrWhiteSpace(text) || text.Equals(".") || text.Equals("-"))
				return @default;

			if (float.TryParse(text, out var result))
				return result;
			
			return @default;
		}

		/// <summary>
		/// <c>String</c> to <c>double</c> conversion method. If empty, the <c>default</c> parameter will be used.
		/// </summary>
		/// <param name="text">Input to convert.</param>
		/// <param name="default">Default value if input is null, empty, <c>.</c> or <c>-</c>.</param>
		public static double ToDouble(string text, double @default = 0)
		{
			if (string.IsNullOrWhiteSpace(text) || text.Equals(".") || text.Equals("-"))
				return @default;

			if (double.TryParse(text, out var result))
				return result;

			return @default;
		}

		/// <summary>
		/// Self-written random color generator.
		/// </summary>
		/// <param name="a">Alpha value to use.</param>
		public static Color4 RandomColor(float a = 1f)
		{
			// In order to not get any grey, black or white values, which are bad to see, multiple randoms need to be calculated.
			// First of all, generate three values in the range of [0.25;0.75]. black and white are now omitted.
			var array = new[] {
				(float)random.NextDouble() * 0.5f + 0.25f,
				(float)random.NextDouble() * 0.5f + 0.25f,
				(float)random.NextDouble() * 0.5f + 0.25f,
			};

			// For each color: set the default value.
			// Then: 50:50 chance to use the inverted color from the previous value. This ensures that the values are truly different from each other, thus making grey almost impossible.
			// The inverted one is still in the range of [0.25;0.75].

			var r = array[0];
			if (random.Next(2) > 0)
				r = 1 - array[1];

			var g = array[1];
			if (random.Next(2) > 0)
				g = 1 - array[2];

			var b = array[2];
			if (random.Next(2) > 0)
				b = 1 - array[0];

			// This gives us enough randomization to always get an interesting color.
			return new Color4(r, g, b, a);
		}
	}
}
