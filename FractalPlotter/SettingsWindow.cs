using FractalPlotter.Graphics;
using System;
using System.Windows.Forms;

namespace FractalPlotter
{
	/// <summary>
	/// Settings window supported by Windows Forms.
	/// </summary>
	public partial class SettingsWindow : Form
	{
		public bool IsClosing { get; private set; }
		public bool IsLoaded { get; private set; }

		readonly GraphSettingsPipe pipe;

		/// <summary>
		///  This is required because when setting some values coming from the graph window, the type event will be invoked and would write the value back, which causes problems.
		/// </summary>
		bool receivesUpdate;

		public SettingsWindow(GraphSettingsPipe pipe)
		{
			this.pipe = pipe;
			pipe.Add(this);

			InitializeComponent();
		}

		/// <summary>
		/// Write the camera translation.
		/// </summary>
		public void UpdateTranslation()
		{
			receivesUpdate = true;
			locX.Text = Camera.Location.X.ToString();
			locY.Text = Camera.Location.Y.ToString();
			receivesUpdate = false;
		}

		/// <summary>
		/// Write the camera scale.
		/// </summary>
		public void UpdateScale()
		{
			receivesUpdate = true;
			scaleX.Text = Camera.Scale.ToString();
			receivesUpdate = false;
		}

		/// <summary>
		/// Write the cursor location.
		/// </summary>
		public void UpdateParameters()
		{
			receivesUpdate = true;
			c1Box.Text = MasterRenderer.Factor1.X.ToString();
			c2Box.Text = MasterRenderer.Factor1.Y.ToString();
			imaxBox.Text = MasterRenderer.IMax.ToString();
			limitBox.Text = Math.Sqrt(MasterRenderer.SquaredLimit).ToString();
			receivesUpdate = false;
		}

		/// <summary>
		/// Write the cursor location.
		/// </summary>
		public void UpdateCursorLocation(double x, double y)
		{
			const string str = "00.000000000000";
			cursorLocation.Text = $"cursor at\n{x.ToString(str)}\n{y.ToString(str)}i";
		}

		/// <summary>
		/// Modified exit event which closes the pipe as well, if necessary.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			IsClosing = true;
			pipe.Exit();

			base.OnClosing(e);
		}

		/// <summary>
		/// Sets some default values already after loading.
		/// </summary>
		void load(object sender, EventArgs e)
		{
			foreach (var file in FileManager.GetGraphShaderNames())
				ShaderList.Items.Add(file);
			ShaderList.SelectedItem = Settings.DefaultShader;

			foreach (var file in FileManager.GetPaletteImageNames())
				PaletteList.Items.Add(file);
			PaletteList.SelectedItem = Settings.DefaultPalette;

			pointCheck.Checked = Settings.Points;
			dimensionCheck.Checked = Settings.ThreeDimensional;

			IsLoaded = true;
		}

		/// <summary>
		/// Update the fancy boolean in the graph window.
		/// </summary>
		void changeFancy(object sender, EventArgs e)
		{
			if (IsLoaded)
				pipe.PointCheck(pointCheck.Checked);
		}

		/// <summary>
		/// Update the 3D boolean in the graph window.
		/// </summary>
		void changeDimensional(object sender, EventArgs e)
		{
			if (IsLoaded)
				pipe.DimensionCheck(dimensionCheck.Checked);

			ShaderList.Enabled = !dimensionCheck.Checked;
			PaletteList.Enabled = !dimensionCheck.Checked;
		}

		/// <summary>
		/// Change the shader in the graph window.
		/// </summary>
		void changeShader(object sender, EventArgs e)
		{
			if (IsLoaded)
				pipe.SetCurrentShader(ShaderList.Items[ShaderList.SelectedIndex].ToString());
		}

		/// <summary>
		/// Change the palette in the graph window.
		/// </summary>
		void changePalette(object sender, EventArgs e)
		{
			if (IsLoaded)
				pipe.SetCurrentPalette(PaletteList.Items[PaletteList.SelectedIndex].ToString());
		}

		/// <summary>
		/// KeyPress event which controls that no non-numerical values are inserted.
		/// Modified code. Original from https://ourcodeworld.com/articles/read/507/how-to-allow-only-numbers-inside-a-textbox-in-winforms-c-sharp.
		/// </summary>
		void keyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
				e.Handled = true;

			var textbox = sender as TextBox;

			if ((e.KeyChar == '.') && textbox.Text.IndexOf('.') > -1)
				e.Handled = true;

			if ((e.KeyChar == '-') && textbox.SelectionStart != 0)
				e.Handled = true;
		}

		/// <summary>
		/// KeyPress event which controls that no non-numerical positive integer values are inserted.
		/// Modified code. Original from https://ourcodeworld.com/articles/read/507/how-to-allow-only-numbers-inside-a-textbox-in-winforms-c-sharp.
		/// </summary>
		void keyPressInteger(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
				e.Handled = true;
		}

		/// <summary>
		/// Checks and sets the translation and pipes it to the graph window then.
		/// </summary>
		void setTranslation(object sender, EventArgs e)
		{
			if (receivesUpdate)
				return;

			var x = Utils.ToDouble(locX.Text);
			var y = Utils.ToDouble(locY.Text);
			pipe.SetTranslation(x, y, 0);
		}

		/// <summary>
		/// Checks and sets the scale and pipes it to the graph window then.
		/// </summary>
		void setScale(object sender, EventArgs e)
		{
			if (receivesUpdate)
				return;

			var s = Utils.ToFloat(scaleX.Text);
			pipe.SetScale(s);
		}

		/// <summary>
		/// Checks and sets the parameters and pipes it to the graph window then.
		/// </summary>
		void setParameters(object sender, EventArgs e)
		{
			if (receivesUpdate)
				return;

			var c1 = Utils.ToFloat(c1Box.Text, 0);
			var c2 = Utils.ToFloat(c2Box.Text, 0);
			var imax = Utils.ToInt(imaxBox.Text, 0);
			var limit = Utils.ToFloat(limitBox.Text, 0);
			pipe.SetParameters(c1, c2, imax, limit);
		}

		/// <summary>
		/// Adds a point exactly where the viewport currently is at.
		/// </summary>
		void addPoint(object sender, EventArgs e)
		{
			pipe.AddPoint(Camera.Location);
		}

		/// <summary>
		/// Takes a screenshot of the whole frame.
		/// </summary>
		void takeScreenshot(object sender, EventArgs e)
		{
			pipe.TakeScreenshot = true;
		}
	}
}
