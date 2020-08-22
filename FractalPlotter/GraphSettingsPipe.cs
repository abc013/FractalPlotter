using ComplexNumberGrapher.Graphics;
using OpenToolkit.Mathematics;
using System;

namespace ComplexNumberGrapher
{
	public class GraphSettingsPipe
	{
		GraphWindow graphWindow;
		SettingsWindow settingsWindow;
		bool exiting;

		public GraphSettingsPipe() { }

		public void Add(GraphWindow window)
		{
			graphWindow = window;
		}

		public void Add(SettingsWindow window)
		{
			settingsWindow = window;
		}

		public void PipeInfo(object info, bool first)
		{
			if (exiting)
				return;

			if (first)
				pipe(delegate { settingsWindow.Info0.Text = info.ToString(); });
			else
				pipe(delegate { settingsWindow.Info1.Text = info.ToString(); });
		}

		public void UpdateTranslation()
		{
			pipe(delegate { settingsWindow.UpdateTranslation(); });
		}

		public void UpdateScale()
		{
			pipe(delegate { settingsWindow.UpdateScale(); });
		}

		public void UpdateRotation()
		{
			pipe(delegate { settingsWindow.UpdateRotation(); });
		}

		public void UpdateParameters()
		{
			pipe(delegate { settingsWindow.UpdateParameters(); });
		}

		public void UpdateCursorLocation(double x, double y)
		{
			pipe(delegate { settingsWindow.UpdateCursorLocation(x, y); });
		}

		public void SetTranslation(double x, double y, double z)
		{
			Camera.SetTranslation(x, y, z);
		}

		public void SetScale(float x, float y, float z)
		{
			Camera.SetScale(x, y, z);
		}

		public void SetRotation(float x, float y, float z)
		{
			Camera.SetRotation(x, y, z);
		}

		public void SetParameters(float c1, float c2, float d)
		{
			MasterRenderer.Factor1 = new Vector2(c1, c2);
			MasterRenderer.Factor2 = d;
		}

		public void FancyCheck(bool @checked)
		{
			Settings.Fancy = @checked;
		}

		public void DimensionCheck(bool @checked)
		{
			Settings.ThreeDimensional = @checked;
		}

		public void SetCurrentShader(string name)
		{
			MasterRenderer.ChangeShader(name);
			graphWindow.IsFocused = true;
		}

		public void SetCurrentPalette(string name)
		{
			MasterRenderer.ChangePalette(name);
			graphWindow.IsFocused = true;
		}

		public void AddPoint(Vector3d location)
		{
			PointManager.Add(location, Utils.RandomColor());
			graphWindow.IsFocused = true;
		}

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
