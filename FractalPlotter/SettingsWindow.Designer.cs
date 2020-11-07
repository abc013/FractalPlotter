namespace FractalPlotter
{
	partial class SettingsWindow
	{

		System.ComponentModel.IContainer components = null;
		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pointCheck = new System.Windows.Forms.CheckBox();
			this.Info0 = new System.Windows.Forms.Label();
			this.Info1 = new System.Windows.Forms.Label();
			this.locX = new System.Windows.Forms.TextBox();
			this.locY = new System.Windows.Forms.TextBox();
			this.locZ = new System.Windows.Forms.TextBox();
			this.scaleX = new System.Windows.Forms.TextBox();
			this.locText = new System.Windows.Forms.Label();
			this.scaleText = new System.Windows.Forms.Label();
			this.ShaderList = new System.Windows.Forms.ListBox();
			this.shaderText = new System.Windows.Forms.Label();
			this.PaletteList = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.dimensionCheck = new System.Windows.Forms.CheckBox();
			this.imaxBox = new System.Windows.Forms.TextBox();
			this.c2Box = new System.Windows.Forms.TextBox();
			this.c1Box = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.pointButton = new System.Windows.Forms.Button();
			this.cursorLocation = new System.Windows.Forms.Label();
			this.screenshotButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// pointCheck
			// 
			this.pointCheck.AutoSize = true;
			this.pointCheck.Checked = true;
			this.pointCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.pointCheck.Location = new System.Drawing.Point(9, 165);
			this.pointCheck.Margin = new System.Windows.Forms.Padding(2);
			this.pointCheck.Name = "pointCheck";
			this.pointCheck.Size = new System.Drawing.Size(86, 29);
			this.pointCheck.TabIndex = 1;
			this.pointCheck.Text = "Points";
			this.pointCheck.UseVisualStyleBackColor = true;
			// 
			// Info0
			// 
			this.Info0.AutoSize = true;
			this.Info0.Location = new System.Drawing.Point(9, 731);
			this.Info0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.Info0.Name = "Info0";
			this.Info0.Size = new System.Drawing.Size(76, 25);
			this.Info0.TabIndex = 3;
			this.Info0.Text = "Debug0";
			// 
			// Info1
			// 
			this.Info1.AutoSize = true;
			this.Info1.Location = new System.Drawing.Point(9, 756);
			this.Info1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.Info1.Name = "Info1";
			this.Info1.Size = new System.Drawing.Size(76, 25);
			this.Info1.TabIndex = 3;
			this.Info1.Text = "Debug1";
			// 
			// locX
			// 
			this.locX.Location = new System.Drawing.Point(9, 319);
			this.locX.Margin = new System.Windows.Forms.Padding(2);
			this.locX.Name = "locX";
			this.locX.Size = new System.Drawing.Size(69, 31);
			this.locX.TabIndex = 4;
			// 
			// locY
			// 
			this.locY.Location = new System.Drawing.Point(82, 319);
			this.locY.Margin = new System.Windows.Forms.Padding(2);
			this.locY.Name = "locY";
			this.locY.Size = new System.Drawing.Size(69, 31);
			this.locY.TabIndex = 5;
			// 
			// locZ
			// 
			this.locZ.Location = new System.Drawing.Point(155, 319);
			this.locZ.Margin = new System.Windows.Forms.Padding(2);
			this.locZ.Name = "locZ";
			this.locZ.Size = new System.Drawing.Size(69, 31);
			this.locZ.TabIndex = 6;
			// 
			// scaleX
			// 
			this.scaleX.Location = new System.Drawing.Point(9, 386);
			this.scaleX.Margin = new System.Windows.Forms.Padding(2);
			this.scaleX.Name = "scaleX";
			this.scaleX.Size = new System.Drawing.Size(216, 31);
			this.scaleX.TabIndex = 7;
			// 
			// locText
			// 
			this.locText.AutoSize = true;
			this.locText.Location = new System.Drawing.Point(9, 291);
			this.locText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.locText.Name = "locText";
			this.locText.Size = new System.Drawing.Size(79, 25);
			this.locText.TabIndex = 3;
			this.locText.Tag = "";
			this.locText.Text = "Location";
			// 
			// scaleText
			// 
			this.scaleText.AutoSize = true;
			this.scaleText.Location = new System.Drawing.Point(9, 359);
			this.scaleText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.scaleText.Name = "scaleText";
			this.scaleText.Size = new System.Drawing.Size(83, 25);
			this.scaleText.TabIndex = 3;
			this.scaleText.Text = "Scalation";
			// 
			// ShaderList
			// 
			this.ShaderList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ShaderList.ItemHeight = 25;
			this.ShaderList.Location = new System.Drawing.Point(9, 32);
			this.ShaderList.Margin = new System.Windows.Forms.Padding(2);
			this.ShaderList.Name = "ShaderList";
			this.ShaderList.ScrollAlwaysVisible = true;
			this.ShaderList.Size = new System.Drawing.Size(239, 127);
			this.ShaderList.TabIndex = 0;
			this.ShaderList.SelectedIndexChanged += new System.EventHandler(this.changeShader);
			// 
			// shaderText
			// 
			this.shaderText.AutoSize = true;
			this.shaderText.Location = new System.Drawing.Point(9, 5);
			this.shaderText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.shaderText.Name = "shaderText";
			this.shaderText.Size = new System.Drawing.Size(75, 25);
			this.shaderText.TabIndex = 3;
			this.shaderText.Text = "Shaders";
			// 
			// PaletteList
			// 
			this.PaletteList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PaletteList.ItemHeight = 25;
			this.PaletteList.Location = new System.Drawing.Point(9, 220);
			this.PaletteList.Margin = new System.Windows.Forms.Padding(2);
			this.PaletteList.Name = "PaletteList";
			this.PaletteList.ScrollAlwaysVisible = true;
			this.PaletteList.Size = new System.Drawing.Size(238, 52);
			this.PaletteList.TabIndex = 3;
			this.PaletteList.SelectedIndexChanged += new System.EventHandler(this.changePalette);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 193);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 25);
			this.label1.TabIndex = 3;
			this.label1.Text = "Palettes";
			// 
			// dimensionCheck
			// 
			this.dimensionCheck.AutoSize = true;
			this.dimensionCheck.Location = new System.Drawing.Point(114, 165);
			this.dimensionCheck.Margin = new System.Windows.Forms.Padding(2);
			this.dimensionCheck.Name = "dimensionCheck";
			this.dimensionCheck.Size = new System.Drawing.Size(61, 29);
			this.dimensionCheck.TabIndex = 2;
			this.dimensionCheck.Text = "3D";
			this.dimensionCheck.UseVisualStyleBackColor = true;
			// 
			// imaxBox
			// 
			this.imaxBox.Location = new System.Drawing.Point(155, 514);
			this.imaxBox.Margin = new System.Windows.Forms.Padding(2);
			this.imaxBox.Name = "imaxBox";
			this.imaxBox.Size = new System.Drawing.Size(69, 31);
			this.imaxBox.TabIndex = 13;
			// 
			// c2Box
			// 
			this.c2Box.Location = new System.Drawing.Point(82, 514);
			this.c2Box.Margin = new System.Windows.Forms.Padding(2);
			this.c2Box.Name = "c2Box";
			this.c2Box.Size = new System.Drawing.Size(69, 31);
			this.c2Box.TabIndex = 12;
			// 
			// c1Box
			// 
			this.c1Box.Location = new System.Drawing.Point(9, 514);
			this.c1Box.Margin = new System.Windows.Forms.Padding(2);
			this.c1Box.Name = "c1Box";
			this.c1Box.Size = new System.Drawing.Size(69, 31);
			this.c1Box.TabIndex = 11;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 487);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label2.Size = new System.Drawing.Size(205, 25);
			this.label2.TabIndex = 3;
			this.label2.Text = "Parameters (c1, c2, imax)";
			// 
			// pointButton
			// 
			this.pointButton.Location = new System.Drawing.Point(9, 571);
			this.pointButton.Margin = new System.Windows.Forms.Padding(2);
			this.pointButton.Name = "pointButton";
			this.pointButton.Size = new System.Drawing.Size(215, 36);
			this.pointButton.TabIndex = 14;
			this.pointButton.Text = "Add Point";
			this.pointButton.UseVisualStyleBackColor = true;
			this.pointButton.Click += new System.EventHandler(this.addPoint);
			// 
			// cursorLocation
			// 
			this.cursorLocation.AutoSize = true;
			this.cursorLocation.Location = new System.Drawing.Point(9, 656);
			this.cursorLocation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.cursorLocation.Name = "cursorLocation";
			this.cursorLocation.Size = new System.Drawing.Size(131, 75);
			this.cursorLocation.TabIndex = 8;
			this.cursorLocation.Text = "CursorLocation\r\nX\r\nY\r\n";
			// 
			// screenshotButton
			// 
			this.screenshotButton.Location = new System.Drawing.Point(11, 611);
			this.screenshotButton.Margin = new System.Windows.Forms.Padding(2);
			this.screenshotButton.Name = "screenshotButton";
			this.screenshotButton.Size = new System.Drawing.Size(215, 36);
			this.screenshotButton.TabIndex = 14;
			this.screenshotButton.Text = "Take Screenshot";
			this.screenshotButton.UseVisualStyleBackColor = true;
			this.screenshotButton.Click += new System.EventHandler(this.takeScreenshot);
			// 
			// SettingsWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(260, 788);
			this.ControlBox = false;
			this.Controls.Add(this.screenshotButton);
			this.Controls.Add(this.cursorLocation);
			this.Controls.Add(this.pointButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.c1Box);
			this.Controls.Add(this.c2Box);
			this.Controls.Add(this.imaxBox);
			this.Controls.Add(this.dimensionCheck);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.PaletteList);
			this.Controls.Add(this.shaderText);
			this.Controls.Add(this.ShaderList);
			this.Controls.Add(this.scaleText);
			this.Controls.Add(this.locText);
			this.Controls.Add(this.scaleX);
			this.Controls.Add(this.locZ);
			this.Controls.Add(this.locY);
			this.Controls.Add(this.locX);
			this.Controls.Add(this.Info1);
			this.Controls.Add(this.Info0);
			this.Controls.Add(this.pointCheck);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximumSize = new System.Drawing.Size(282, 856);
			this.MinimumSize = new System.Drawing.Size(282, 68);
			this.Name = "SettingsWindow";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Settings | FractalPlotter";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox pointCheck;
		private System.Windows.Forms.TextBox locX;
		private System.Windows.Forms.TextBox locY;
		private System.Windows.Forms.TextBox locZ;
		private System.Windows.Forms.TextBox scaleX;
		private System.Windows.Forms.Label locText;
		private System.Windows.Forms.Label scaleText;
		public System.Windows.Forms.Label Info0;
		public System.Windows.Forms.Label Info1;
		private System.Windows.Forms.ListBox ShaderList;
		private System.Windows.Forms.Label shaderText;
		private System.Windows.Forms.ListBox PaletteList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox dimensionCheck;
		private System.Windows.Forms.TextBox imaxBox;
		private System.Windows.Forms.TextBox c2Box;
		private System.Windows.Forms.TextBox c1Box;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button pointButton;
		private System.Windows.Forms.Label cursorLocation;
		private System.Windows.Forms.Button screenshotButton;
	}
}

