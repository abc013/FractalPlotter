using System.Collections.Generic;

namespace ComplexNumberGrapher.Graphics
{
	public static class PaletteManager
	{
		static readonly Dictionary<string, Palette> palettes = new Dictionary<string, Palette>();

		public static void Add(string name)
		{
			var palette = new Palette(FileManager.Palettes + name + ".png");

			palettes.Add(name, palette);
		}

		public static int Fetch(string name)
		{
			if (string.IsNullOrEmpty(name) || !palettes.ContainsKey(name))
				return -1;

			return palettes[name].ID;
		}

		public static void Dispose()
		{
			foreach (var palette in palettes.Values)
				palette.Dispose();
		}
	}
}
