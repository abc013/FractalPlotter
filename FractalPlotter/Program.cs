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
		static GraphSettingsPipe pipe;

		[STAThread]
		public static void Main()
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

			Log.Initialize();
			AppDomain.CurrentDomain.UnhandledException += handleError;
			Settings.Initialize();

			pipe = new GraphSettingsPipe();

			initSettingsWindow();
			initGraphWindow();
		}

		static void handleError(object handler, UnhandledExceptionEventArgs args)
		{
			Log.WriteException(args.ExceptionObject);
		}

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

		public static void Exit()
		{
			Log.Exit();
		}
	}
}
