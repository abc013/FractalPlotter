using System;
using System.IO;
using System.Linq;

namespace ComplexNumberGrapher
{
	public class Settings
	{
		public const int WindowBar = 60;

		public static int GraphX = 360;
		public static int GraphY = WindowBar;

		public static int GraphWidth = 1920 - GraphX;
		public static int GraphHeight = 1080 - GraphY - 20;

		public static bool EventDriven = false;

		public static int IMax = 100;
		public static bool Fancy = true;
		public static bool ThreeDimensional = false;

		public static string DefaultShader = "default";
		public static string DefaultPalette = "palette";

		public static float CameraSpeed = 0.01f;

		public static float Scale = 2f;

		public static float LocationX = 0f;
		public static float LocationY = 0f;
		public static float LocationZ = 0f;

		public static float Factor1X = 0f;
		public static float Factor1Y = 0f;
		public static float Factor2 = 0f;

		public static void Initialize()
		{
			// Use a settings class to gain access to reflection methods, making assigning variables much easier
			new Settings();
		}

		Settings()
		{
			var file = FileManager.CheckSettings();

			if (string.IsNullOrEmpty(file))
				return;

			using var reader = new StreamReader(file);

			var fields = GetType().GetFields().Where(f => f.IsStatic && f.IsPublic);
			while (!reader.EndOfStream)
			{
				var line = reader.ReadLine().Trim();

				// Comments will be left out
				if (line.StartsWith("//"))
					continue;

				// Split into name and variable
				var split = line.Split('=');

				var field = fields.FirstOrDefault(f => f.Name == split[0].Trim());
				if (field != null)
				{
					field.SetValue(this, convert(field.FieldType, split[0].Trim(), split[1].Trim()));
					continue;
				}
			}
		}

		static object convert(Type type, string key, string value)
		{
			if (type == typeof(int))
			{
				if (int.TryParse(value, out var res))
					return res;

				throw new Exception($"Invalid value {value} of {key}. {type} expected.");
			}
			else if (type == typeof(float))
			{
				if (float.TryParse(value, out var res))
					return res;

				throw new Exception($"Invalid value {value} of {key}. {type} expected.");
			}
			else if (type == typeof(string))
			{
				return value;
			}
			else if (type == typeof(bool))
			{
				if (bool.TryParse(value, out var res))
					return res;

				throw new Exception($"Invalid value {value} of {key}. {type} expected.");
			}

			throw new Exception($"Missing conversion method for type {type}");
		}
	}
}
