using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ComplexNumberGrapher
{
	public static class FileManager
	{
		public static readonly string Current = Directory.GetCurrentDirectory() + "\\";
		public static readonly string GraphShaders = Current + "GraphShaders\\";
		public static readonly string Palettes = Current + "Palettes\\";

		public static string CheckSettings()
		{
			var file = Current + "settings.txt";

			if (File.Exists(file))
				return file;

			return string.Empty;
		}

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

		public static string[] GetGraphShaders(string name)
		{
			return new[]
			{
				GraphShaders + name + ".frag",
				GraphShaders + name + ".vert"
			};
		}

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
	}
}
