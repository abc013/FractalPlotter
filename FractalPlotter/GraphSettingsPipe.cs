using ComplexNumberGrapher.Graphics;
using OpenToolkit.Mathematics;
using System;

namespace ComplexNumberGrapher
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
		/// Transmit current rotation to the settings window.
		/// </summary>
		public void UpdateRotation()
		{
			pipe(delegate { settingsWindow.UpdateRotation(); });
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
		public void SetScale(float x, float y, float z)
		{
			Camera.SetScale(x, y, z);
		}

		/// <summary>
		/// Set rotation in the graph window.
		/// </summary>
		public void SetRotation(float x, float y, float z)
		{
			Camera.SetRotation(x, y, z);
		}

		/// <summary>
		/// Set parameters in the graph window.
		/// </summary>
		public void SetParameters(float c1, float c2, float d)
		{
			MasterRenderer.Factor1 = new Vector2(c1, c2);
			MasterRenderer.Factor2 = d;
		}

		/// <summary>
		/// Set the fancy boolean in the settings.
		/// </summary>
		public void FancyCheck(bool @checked)
		{
			Settings.Fancy = @checked;
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
			graphWindow.IsFocused = true;
		}

		/// <summary>
		/// Sets the current palette for the graph window.
		/// </summary>
		public void SetCurrentPalette(string name)
		{
			MasterRenderer.ChangePalette(name);
			graphWindow.IsFocused = true;
		}

		/// <summary>
		/// Adds another point in the graph window.
		/// </summary>
		public void AddPoint(Vector3d location)
		{
			PointManager.Add(location, Utils.RandomColor());
			graphWindow.IsFocused = true;
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
