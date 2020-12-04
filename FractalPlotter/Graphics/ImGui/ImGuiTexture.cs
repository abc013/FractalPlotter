using OpenTK.Graphics.OpenGL;
using System;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace FractalPlotter.Graphics
{
	/// <summary>
	/// Textures used for ImGui.
	/// Taken from https://github.com/NogginBops/ImGui.NET_OpenTK_Sample
	/// </summary>
	class ImGuiTexture : IDisposable
	{
		public readonly int GLTexture;

		public ImGuiTexture(int width, int height, IntPtr data)
		{
			GL.CreateTextures(TextureTarget.Texture2D, 1, out GLTexture);

			GL.TextureStorage2D(GLTexture, 1, SizedInternalFormat.Rgba8, width, height);
			GL.TextureSubImage2D(GLTexture, 0, 0, 0, width, height, PixelFormat.Bgra, PixelType.UnsignedByte, data);

			GL.TextureParameter(GLTexture, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TextureParameter(GLTexture, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
		}

		public void SetMinFilter(TextureMinFilter filter)
		{
			GL.TextureParameter(GLTexture, TextureParameterName.TextureMinFilter, (int)filter);
		}

		public void SetMagFilter(TextureMagFilter filter)
		{
			GL.TextureParameter(GLTexture, TextureParameterName.TextureMagFilter, (int)filter);
		}

		public void Dispose()
		{
			GL.DeleteTexture(GLTexture);
		}
	}
}
