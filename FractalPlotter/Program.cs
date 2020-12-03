using OpenTK.Windowing.Desktop;
using System;
using System.Globalization;

namespace FractalPlotter
{
	static class Program
	{
		/// <summary>
		/// Entry point.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			// Reset culture to invariant. This is needed because otherwise string parsing of floats in different countries can lead to different results/crashing.
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

			Log.Initialize();
			AppDomain.CurrentDomain.UnhandledException += handleError;
			Settings.Initialize();

			initWindow();
		}

		/// <summary>
		/// Helper function that gets added to the domain. It executes when the program fails and writes the exception to the logs.
		/// </summary>
		static void handleError(object handler, UnhandledExceptionEventArgs args)
		{
			Log.WriteException(args.ExceptionObject);
		}

		/// <summary>
		/// Initializes the graph window and runs it.
		/// </summary>
		static void initWindow()
		{
			var gameSettings = GameWindowSettings.Default;
			var nativeSettings = new NativeWindowSettings()
			{
				Title = "Graph | FractalPlotter",
				APIVersion = new Version(3, 3),
				IsEventDriven = Settings.EventDriven,
				Location = new OpenTK.Mathematics.Vector2i(Settings.GraphX, Settings.GraphY),
				Size = new OpenTK.Mathematics.Vector2i(Settings.GraphWidth, Settings.GraphHeight)
			};

			var graphWindow = new GraphWindow(gameSettings, nativeSettings);
			graphWindow.Run();
		}

		/// <summary>
		/// Closes the logs.
		/// </summary>
		public static void Exit()
		{
			Log.Exit();
		}
	}
}
