using System;
using System.IO;
using System.Linq;

namespace FractalPlotter
{
	public class Settings
	{
		/// <summary>
		/// Height of the default window bar (estimated).
		/// </summary>
		public const int WindowBar = 60;

		/// <summary>
		/// X-Coordinate of the Graph window.
		/// </summary>
		public static int GraphX = 360;
		/// <summary>
		/// Y-Coordinate of the Graph window.
		/// </summary>
		public static int GraphY = WindowBar;

		/// <summary>
		/// Width of the Graph window.
		/// </summary>
		public static int GraphWidth = 1920;

		/// <summary>
		/// Height of the Graph window.
		/// </summary>
		public static int GraphHeight = 1080;

		/// <summary>
		/// Draw graph only when modifying. Enabling this will lead to glitches when moving around but is less power consuming.
		/// </summary>
		public static bool EventDriven = false;

		/// <summary>
		/// Maximal iteration number. Increase to get better results (as long as the palette has the same amount of different colors)
		/// </summary>
		public static int IMax = 256;
		/// <summary>
		/// Escape condition value. Usually increased to get better pictures for art. Should not be put below 2.
		/// </summary>
		public static float Limit = 2.0f;
		/// <summary>
		/// Render points and viewport crosshair.
		/// </summary>
		public static bool Points = true;

		/// <summary>
		/// Allows setting the default shader which will be used on startup.
		/// </summary>
		public static string DefaultShader = "default";
		/// <summary>
		/// Allows setting the default palette which will be used on startup.
		/// </summary>
		public static string DefaultPalette = "palette";

		/// <summary>
		/// Allows modification of the camera speed.
		/// </summary>
		public static float CameraSpeed = 0.01f;

		/// <summary>
		/// Allows modification of regulator speed. This determines how fast e.g. the c-Factors are changed when pressing keys.
		/// </summary>
		public static float RegulatorSpeed = 0.001f;

		/// <summary>
		/// Default scale.
		/// </summary>
		public static float Scale = 2f;

		/// <summary>
		/// Default X-location.
		/// </summary>
		public static float LocationX = 0f;
		/// <summary>
		/// Default Y-location.
		/// </summary>
		public static float LocationY = 0f;
		/// <summary>
		/// Default Z-location.
		/// </summary>
		public static float LocationZ = 0f;

		/// <summary>
		/// Default c1-factor.
		/// </summary>
		public static float Factor1X = 0f;
		/// <summary>
		/// Default c2-factor.
		/// </summary>
		public static float Factor1Y = 0f;

		/// <summary>
		/// This method reads data from an existing settings.txt if possible. Otherwise it will use the default variables.
		/// </summary>
		public static void Initialize()
		{
			// Use a settings class to gain access to reflection methods, making assigning variables much easier
			new Settings();
		}

		Settings()
		{
			// Check file path for "settings.txt"
			var file = FileManager.CheckFile("settings.txt");

			// If there is none, return;
			if (string.IsNullOrEmpty(file))
				return;

			// Load the reader
			using var reader = new StreamReader(file);

			// Fetch a collection of all fields in this class that are 1) static and 2) public.
			var fields = GetType().GetFields().Where(f => f.IsStatic && f.IsPublic);

			// While there is something to read
			while (!reader.EndOfStream)
			{
				var line = reader.ReadLine().Trim();

				// Comments will be left out
				if (line.StartsWith("//"))
					continue;

				// Split into name and variable
				var split = line.Split('=');

				// Get field of the same name and, if possible, set the value.
				var field = fields.FirstOrDefault(f => f.Name == split[0].Trim());
				if (field != null)
					field.SetValue(this, convert(field.FieldType, split[0].Trim(), split[1].Trim()));
			}
		}

		/// <summary>
		/// Method used to convert a string into the corresponding type.
		/// </summary>
		/// <param name="type">Type to convert to.</param>
		/// <param name="key">Name of the setting.</param>
		/// <param name="value">Value to convert.</param>
		/// <returns></returns>
		static object convert(Type type, string key, string value)
		{
			if (type == typeof(int))
			{
				if (int.TryParse(value, out var res))
					return res;

				throw new InvalidSettingsException($"Invalid value {value} of {key}. {type} expected.");
			}
			else if (type == typeof(float))
			{
				if (float.TryParse(value, out var res))
					return res;

				throw new InvalidSettingsException($"Invalid value {value} of {key}. {type} expected.");
			}
			else if (type == typeof(string))
			{
				return value;
			}
			else if (type == typeof(bool))
			{
				if (bool.TryParse(value, out var res))
					return res;

				throw new InvalidSettingsException($"Invalid value {value} of {key}. {type} expected.");
			}

			throw new InvalidSettingsException($"Missing conversion method for type {type}");
		}
	}
}
