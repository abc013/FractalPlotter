using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Desktop;
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace ComplexNumberGrapher
{
	static class Program
	{
		static readonly GraphSettingsPipe pipe = new GraphSettingsPipe();

		/// <summary>
		/// Entry point.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

			Log.Initialize();
			AppDomain.CurrentDomain.UnhandledException += handleError;
			Settings.Initialize();

			initSettingsWindow();
			initGraphWindow();
		}

		/// <summary>
		/// Helper function that gets added to the domain. It executes when the program fails and writes the exception to the logs.
		/// </summary>
		static void handleError(object handler, UnhandledExceptionEventArgs args)
		{
			Log.WriteException(args.ExceptionObject);
		}

		/// <summary>
		/// Initializes the settings window in an independent thread and runs it.
		/// </summary>
		static void initSettingsWindow()
		{
			// Run in secondary thread, as the first one will be reserved for the GraphWindow instance.
			new Thread(() =>
			{
				Application.SetHighDpiMode(HighDpiMode.SystemAware);
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);

				var settingsWindow = new SettingsWindow(pipe);

				Application.Run(settingsWindow);
			}).Start();
		}

		/// <summary>
		/// Initializes the graph window and runs it.
		/// </summary>
		static void initGraphWindow()
		{
			var gameSettings = GameWindowSettings.Default;
			var nativeSettings = new NativeWindowSettings()
			{
				Title = "Graph | FractalPlotter",
				APIVersion = new Version(3, 3),
				Profile = ContextProfile.Compatability,
				IsEventDriven = Settings.EventDriven,
				Location = new OpenToolkit.Mathematics.Vector2i(Settings.GraphX, Settings.GraphY),
				Size = new OpenToolkit.Mathematics.Vector2i(Settings.GraphWidth, Settings.GraphHeight)
			};

			var graphWindow = new GraphWindow(pipe, gameSettings, nativeSettings);
			graphWindow.Run();
		}

		/// <summary>
		/// Exit the program.
		/// </summary>
		public static void Exit()
		{
			Log.Exit();
		}
	}
}
