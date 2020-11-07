using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace FractalPlotter
{
	/// <summary>
	/// Class that keeps track of all the file stuff going on.
	/// </summary>
	public static class FileManager
	{
		/// <summary>
		/// Default directory.
		/// </summary>
		public static readonly string Current = Directory.GetCurrentDirectory() + "\\";
		/// <summary>
		/// Directory where the shaders are in.
		/// </summary>
		public static readonly string GraphShaders = Current + "Shaders\\";
		/// <summary>
		/// Directory where the palettes are in.
		/// </summary>
		public static readonly string Palettes = Current + "Palettes\\";
		/// <summary>
		/// Directory where the screenshots are in.
		/// </summary>
		public static readonly string Screenhots = Current + "Screenshots\\";

		/// <summary>
		/// Checks whether the settings.txt exists and returns the path if true.
		/// </summary>
		public static string CheckSettings(string file)
		{
			var filePath = Current + file;

			if (File.Exists(filePath))
				return filePath;

			return string.Empty;
		}

		/// <summary>
		/// Finds all shaders and returns their names.
		/// </summary>
		public static List<string> GetGraphShaderNames()
		{
			var files = Directory.GetFiles(GraphShaders).Where(f => f.EndsWith(".frag")).ToList();

			var results = new List<string>();

			foreach (var file in files)
			{
				var index = file.LastIndexOf('\\') + 1;
				var name = file.Substring(index, file.Length - index - 5);

				if (File.Exists(GraphShaders + name + ".vert"))
					results.Add(name);
			}

			return results;
		}

		/// <summary>
		/// Function that returns the paths to the corresponding shader files.
		/// </summary>
		/// <param name="name">Name of the shader.</param>
		public static string[] GetGraphShaders(string name)
		{
			return new[]
			{
				GraphShaders + name + ".frag",
				GraphShaders + name + ".vert"
			};
		}

		/// <summary>
		/// Finds all palette files and returns their names.
		/// </summary>
		public static List<string> GetPaletteImageNames()
		{
			var files = Directory.GetFiles(Palettes).Where(f => f.EndsWith(".png")).ToList();

			var results = new List<string>();

			foreach (var file in files)
			{
				var index = file.LastIndexOf('\\') + 1;
				var name = file.Substring(index, file.Length - index - 4);

				results.Add(name);
			}

			return results;
		}

		public static void SaveScreenshot(byte[] data, int width, int height)
		{
			if (!Directory.Exists(Screenhots))
				Directory.CreateDirectory(Screenhots);

			var file = Screenhots + "screenshot_" + DateTime.Now.ToString("HHmmss_ddMMyyyy") + ".png";

			using var img = new Bitmap(width, height, width * 3, System.Drawing.Imaging.PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(data, 0));

			img.Save(file);
		}
	}
}
