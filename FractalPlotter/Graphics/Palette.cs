using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ComplexNumberGrapher.Graphics
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
		/// Takes an input file (32bit png) and loads it into the GPU for further use in the shaders.
		/// </summary>
		/// <param name="name">path to the specified file.</param>
		public Palette(string name)
		{
			var bmp = new Bitmap(name);
			var rect = new Rectangle(0, 0, bmp.Width, 1);

			var data = loadTexture(bmp, rect);

			ID = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture1D, ID);
			GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
			GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMagFilter, (int)All.Nearest);

			GL.TexImage1D(TextureTarget.Texture1D, 0, PixelInternalFormat.Rgba32f, rect.Width, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Float, data);

			GL.BindTexture(TextureTarget.Texture1D, 0);

			Utils.CheckError($"Palette {name}");
		}

		/// <summary>
		/// Load textues the fastest way possible.
		/// Code taken from https://stackoverflow.com/questions/4747428/getting-rgb-array-from-image-in-c-sharp.
		/// </summary>
		float[] loadTexture(Bitmap bmp, Rectangle rect)
		{
			const int pixelWidth = 4;

			var data = bmp.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			var byteSize = data.Height * data.Stride;
			System.GC.AddMemoryPressure(byteSize);

			var scansize = data.Width * 4;
			var stride = data.Stride;
			var r = new float[data.Height * stride];
			try
			{
				var scan = new byte[stride];
				for (int scanline = 0; scanline < data.Height; scanline++)
				{
					Marshal.Copy(data.Scan0 + scanline * stride, scan, 0, stride);

					for (int px = 0; px < data.Width; px++)
					{
						// little endian
						// B
						r[scanline * scansize + px * pixelWidth + 2] = scan[px * pixelWidth] / 255f;
						// G
						r[scanline * scansize + px * pixelWidth + 1] = scan[px * pixelWidth + 1] / 255f;
						// R
						r[scanline * scansize + px * pixelWidth] = scan[px * pixelWidth + 2] / 255f;
						// A
						r[scanline * scansize + px * pixelWidth + 3] = scan[px * pixelWidth + 3] / 255f;
					}
				}
			}
			finally
			{
				bmp.UnlockBits(data);
				System.GC.RemoveMemoryPressure(byteSize);
			}

			return r;
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
