using FractalPlotter.Graphics;
using OpenTK.Mathematics;
using System;

namespace FractalPlotter
{
	/// <summary>
	/// Class to use for information transmittion between the graph window and the settings window.
	/// Reason behind this class is that inter-thread communication is very sensitive. So, in order to update values from the main thread (the graph window) to the settings thread, a delegate must be invoked.
	/// This class is meant to collect all communication in one class so that nothing flies around freely.
	/// This also covers communications vice-versa, however, surprisingly, these do not cause any problems and thus need no delegates.
	/// </summary>
	public class GraphSettingsPipe
	{
		GraphWindow graphWindow;
		SettingsWindow settingsWindow;
		bool exiting;

		/// <summary>
		/// Takes a screenshot. The boolean is needed because GL crashes when trying to read pixels when calling MasterRenderer.TakeScreenshot() directly.
		/// </summary>
		public bool TakeScreenshot;

		public GraphSettingsPipe() { }

		/// <summary>
		/// Add the graph window instance.
		/// </summary>
		public void Add(GraphWindow window)
		{
			graphWindow = window;
		}

		/// <summary>
		/// Add the settings window instance.
		/// </summary>
		public void Add(SettingsWindow window)
		{
			settingsWindow = window;
		}

		/// <summary>
		/// Transmit information texts to the settings window.
		/// </summary>
		public void PipeInfo(object info, bool first)
		{
			if (exiting)
				return;

			if (first)
				pipe(delegate { settingsWindow.Info0.Text = info.ToString(); });
			else
				pipe(delegate { settingsWindow.Info1.Text = info.ToString(); });
		}

		/// <summary>
		/// Transmit current translation to the settings window.
		/// </summary>
		public void UpdateTranslation()
		{
			pipe(delegate { settingsWindow.UpdateTranslation(); });
		}

		/// <summary>
		/// Transmit current scale to the settings window.
		/// </summary>
		public void UpdateScale()
		{
			pipe(delegate { settingsWindow.UpdateScale(); });
		}

		/// <summary>
		/// Transmit current parameters to the settings window.
		/// </summary>
		public void UpdateParameters()
		{
			pipe(delegate { settingsWindow.UpdateParameters(); });
		}

		/// <summary>
		/// Transmit current cursor location to the settings window.
		/// </summary>
		public void UpdateCursorLocation(double x, double y)
		{
			pipe(delegate { settingsWindow.UpdateCursorLocation(x, y); });
		}

		/// <summary>
		/// Set translation in the graph window.
		/// </summary>
		public void SetTranslation(double x, double y, double z)
		{
			Camera.SetTranslation(x, y, z);
		}

		/// <summary>
		/// Set scale in the graph window.
		/// </summary>
		public void SetScale(float s)
		{
			Camera.SetScale(s);
		}

		/// <summary>
		/// Set parameters in the graph window.
		/// </summary>
		public void SetParameters(float c1, float c2, int imax, float limit)
		{
			MasterRenderer.Factor1 = new Vector2(c1, c2);
			MasterRenderer.IMax = imax;
			MasterRenderer.SquaredLimit = limit*limit;
		}

		/// <summary>
		/// Set the points boolean in the settings.
		/// </summary>
		public void PointCheck(bool @checked)
		{
			Settings.Points = @checked;
		}

		/// <summary>
		/// Set the 3D boolean in the settings.
		/// </summary>
		public void DimensionCheck(bool @checked)
		{
			Settings.ThreeDimensional = @checked;
		}

		/// <summary>
		/// Sets the current shader for the graph window.
		/// </summary>
		public void SetCurrentShader(string name)
		{
			MasterRenderer.ChangeShader(name);
			graphWindow.Focus();
		}

		/// <summary>
		/// Sets the current palette for the graph window.
		/// </summary>
		public void SetCurrentPalette(string name)
		{
			MasterRenderer.ChangePalette(name);
			graphWindow.Focus();
		}

		/// <summary>
		/// Adds another point in the graph window.
		/// </summary>
		public void AddPoint(Vector3 location)
		{
			PointManager.Add(location, Utils.RandomColor());
			graphWindow.Focus();
		}

		/// <summary>
		/// Closes the pipe and prevents any further communication.
		/// Also closes the graph window if not already closing. Same applies to the settings window.
		/// </summary>
		public void Exit()
		{
			if (exiting)
				return;

			exiting = true;

			if (!settingsWindow.IsClosing)
				pipe(delegate { settingsWindow.Close(); });

			if (!graphWindow.IsClosing)
				graphWindow.Close();

			Program.Exit();
		}

		/// <summary>
		/// Pipes the action needed to the settings thread.
		/// </summary>
		void pipe(Action method)
		{
			if (settingsWindow.ActiveControl == null || settingsWindow.IsClosing)
			{
				Log.WriteInfo("Tried to access settingsWindow while init/close.");
				return;
			}

			try
			{
				settingsWindow.ActiveControl.Invoke(method);
			}
			catch (System.ComponentModel.InvalidAsynchronousStateException)
			{
				// this happens when the Thread already exited. Can be left unnoticed.
			}
		}
	}
}
