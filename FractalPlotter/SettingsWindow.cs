using ComplexNumberGrapher.Graphics;
using System;
using System.Windows.Forms;

namespace ComplexNumberGrapher
{
	public partial class SettingsWindow : Form
	{
		public bool IsClosing { get; private set; }
		public bool IsLoaded { get; private set; }

		readonly GraphSettingsPipe pipe;

		int updated;

		public SettingsWindow(GraphSettingsPipe pipe)
		{
			this.pipe = pipe;
			pipe.Add(this);

			InitializeComponent();

			fancyCheck.CheckedChanged += changeFancy;
			dimensionCheck.CheckedChanged += changeDimensional;

			locX.KeyPress += keyPress;
			locX.TextChanged += setTranslation;
			locY.KeyPress += keyPress;
			locY.TextChanged += setTranslation;
			locZ.KeyPress += keyPress;
			locZ.TextChanged += setTranslation;

			scaleX.KeyPress += keyPress;
			scaleX.TextChanged += setScale;

			rotX.KeyPress += keyPress;
			rotX.TextChanged += setRotation;
			rotY.KeyPress += keyPress;
			rotY.TextChanged += setRotation;
			rotZ.KeyPress += keyPress;
			rotZ.TextChanged += setRotation;

			dBox.KeyPress += keyPress;
			dBox.TextChanged += setParameters;
			c2Box.KeyPress += keyPress;
			c2Box.TextChanged += setParameters;
			c1Box.KeyPress += keyPress;
			c1Box.TextChanged += setParameters;
		}

		public void UpdateTranslation()
		{
			updated += 3;
			locX.Text = Camera.Location.X.ToString();
			locY.Text = Camera.Location.Y.ToString();
			locZ.Text = Camera.Location.Z.ToString();
		}

		public void UpdateScale()
		{
			updated += 3;
			scaleX.Text = Camera.Scale.X.ToString();
		}

		public void UpdateRotation()
		{
			updated += 3;
			rotX.Text = Camera.Rotation.X.ToString();
			rotY.Text = Camera.Rotation.Y.ToString();
			rotZ.Text = Camera.Rotation.Z.ToString();
		}

		public void UpdateParameters()
		{
			updated += 3;
			c1Box.Text = MasterRenderer.Factor1.X.ToString();
			c2Box.Text = MasterRenderer.Factor1.Y.ToString();
			dBox.Text = MasterRenderer.Factor2.ToString();
		}

		public void UpdateCursorLocation(double x, double y)
		{
			var str = "00.000000000000";
			cursorLocation.Text = $"cursor at\n{x.ToString(str)}\n{y.ToString(str)}i";
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			IsClosing = true;
			pipe.Exit();

			base.OnClosing(e);
		}

		void load(object sender, EventArgs e)
		{
			foreach (var file in FileManager.GetGraphShaderNames())
				ShaderList.Items.Add(file);
			ShaderList.SelectedItem = Settings.DefaultShader;

			foreach (var file in FileManager.GetPaletteImageNames())
				PaletteList.Items.Add(file);
			PaletteList.SelectedItem = Settings.DefaultPalette;

			fancyCheck.Checked = Settings.Fancy;
			dimensionCheck.Checked = Settings.ThreeDimensional;

			IsLoaded = true;
		}

		void changeFancy(object sender, EventArgs e)
		{
			if (IsLoaded)
				pipe.FancyCheck(fancyCheck.Checked);
		}

		void changeDimensional(object sender, EventArgs e)
		{
			if (IsLoaded)
				pipe.DimensionCheck(dimensionCheck.Checked);

			ShaderList.Enabled = !dimensionCheck.Checked;
			PaletteList.Enabled = !dimensionCheck.Checked;
		}

		void changeShader(object sender, EventArgs e)
		{
			if (IsLoaded)
				pipe.SetCurrentShader(ShaderList.Items[ShaderList.SelectedIndex].ToString());
		}

		void changePalette(object sender, EventArgs e)
		{
			if (IsLoaded)
				pipe.SetCurrentPalette(PaletteList.Items[PaletteList.SelectedIndex].ToString());
		}

		void keyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.')
				e.Handled = true;

			if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
				e.Handled = true;
		}

		void setTranslation(object sender, EventArgs e)
		{
			if (updated > 0)
			{
				updated -= 1;
				return;
			}
			var x = Utils.ToDouble(locX.Text);
			var y = Utils.ToDouble(locY.Text);
			var z = Utils.ToDouble(locZ.Text);
			pipe.SetTranslation(x, y, z);
		}

		void setScale(object sender, EventArgs e)
		{
			if (updated > 0)
			{
				updated -= 1;
				return;
			}
			var sx = Utils.ToFloat(scaleX.Text);
			pipe.SetScale(sx, sx, sx);
		}

		void setRotation(object sender, EventArgs e)
		{
			if (updated > 0)
			{
				updated -= 1;
				return;
			}
			var rx = Utils.ToFloat(rotX.Text, 0);
			var ry = Utils.ToFloat(rotY.Text, 0);
			var rz = Utils.ToFloat(rotZ.Text, 0);
			pipe.SetRotation(rx, ry, rz);
		}

		void setParameters(object sender, EventArgs e)
		{
			if (updated > 0)
			{
				updated -= 1;
				return;
			}
			var c1 = Utils.ToFloat(c1Box.Text, 0);
			var c2 = Utils.ToFloat(c2Box.Text, 0);
			var d = Utils.ToFloat(dBox.Text, 0);
			pipe.SetParameters(c1, c2, d);
		}

		void addPoint(object sender, EventArgs e)
		{
			pipe.AddPoint(Camera.ExactLocation);
		}
	}
}
