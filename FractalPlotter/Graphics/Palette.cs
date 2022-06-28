using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace FractalPlotter.Graphics
{
	/// <summary>
	/// Class representing a palette file.
	/// </summary>
	public class Palette
	{
		/// <summary>
		/// ID that GL uses internally to keep track of textures.
		/// </summary>
		public readonly int ID;

		/// <summary>
		/// Takes an input file and loads it into the GPU for further use in the shaders.
		/// </summary>
		/// <param name="name">path to the specified file.</param>
		public Palette(string name)
		{
			using var img = Image.Load<RgbaVector>(name);
			var rect = new Rectangle(0, 0, img.Width, 1);

			var data = loadTexture(img, rect);

			// Generate a texture, specifiy it only has one dimension.
			ID = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture1D, ID);
			GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
			GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
			GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureWrapS, (int)All.ClampToBorder);
			GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureWrapT, (int)All.ClampToBorder);
			// Load in the data.
			GL.TexImage1D(TextureTarget.Texture1D, 0, PixelInternalFormat.Rgba32f, rect.Width, 0, PixelFormat.Rgba, PixelType.Float, data);

			GL.BindTexture(TextureTarget.Texture1D, 0);

			Utils.CheckError($"Palette {name}");
		}

		/// <summary>
		/// Load textues the fastest way possible.
		/// </summary>
		float[] loadTexture(Image<RgbaVector> img, Rectangle rect)
		{
			var span = new Span<RgbaVector>(new RgbaVector[rect.Width * rect.Height]);
			img.CopyPixelDataTo(span);

			var result = new float[rect.Width * rect.Height * 4];

			for (int scanline = 0; scanline < rect.Height; scanline++)
			{
				for (int pixel = 0; pixel < rect.Width; pixel++)
				{
					var offset = (scanline * rect.Width + pixel) * 4;
					var color = span[(rect.Y + scanline) * img.Size().Width + rect.X + pixel];

					result[offset++] = color.R;
					result[offset++] = color.G;
					result[offset++] = color.B;
					result[offset++] = color.A;
				}
			}

			return result;
		}

		/// <summary>
		/// Clean up.
		/// </summary>
		public void Dispose()
		{
			GL.DeleteTexture(ID);
		}
	}
}
