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
			this.scaleX = new System.Windows.Forms.TextBox();
			this.locText = new System.Windows.Forms.Label();
			this.scaleText = new System.Windows.Forms.Label();
			this.ShaderList = new System.Windows.Forms.ListBox();
			this.shaderText = new System.Windows.Forms.Label();
			this.PaletteList = new System.Windows.Forms.ListBox();
			this.paletteText = new System.Windows.Forms.Label();
			this.imaxBox = new System.Windows.Forms.TextBox();
			this.c2Box = new System.Windows.Forms.TextBox();
			this.c1Box = new System.Windows.Forms.TextBox();
			this.cText = new System.Windows.Forms.Label();
			this.pointButton = new System.Windows.Forms.Button();
			this.cursorLocation = new System.Windows.Forms.Label();
			this.screenshotButton = new System.Windows.Forms.Button();
			this.limitBox = new System.Windows.Forms.TextBox();
			this.imaxText = new System.Windows.Forms.Label();
			this.limitText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pointCheck
			// 
			this.pointCheck.AutoSize = true;
			this.pointCheck.Checked = true;
			this.pointCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.pointCheck.Location = new System.Drawing.Point(10, 255);
			this.pointCheck.Margin = new System.Windows.Forms.Padding(2);
			this.pointCheck.Name = "pointCheck";
			this.pointCheck.Size = new System.Drawing.Size(135, 29);
			this.pointCheck.TabIndex = 2;
			this.pointCheck.Text = "Show Points";
			this.pointCheck.UseVisualStyleBackColor = true;
			this.pointCheck.CheckedChanged += new System.EventHandler(this.changeFancy);
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
			this.locX.Location = new System.Drawing.Point(10, 310);
			this.locX.Margin = new System.Windows.Forms.Padding(2);
			this.locX.Name = "locX";
			this.locX.Size = new System.Drawing.Size(115, 31);
			this.locX.TabIndex = 4;
			this.locX.TextChanged += new System.EventHandler(this.setTranslation);
			this.locX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyPress);
			// 
			// locY
			// 
			this.locY.Location = new System.Drawing.Point(135, 310);
			this.locY.Margin = new System.Windows.Forms.Padding(2);
			this.locY.Name = "locY";
			this.locY.Size = new System.Drawing.Size(115, 31);
			this.locY.TabIndex = 5;
			this.locY.TextChanged += new System.EventHandler(this.setTranslation);
			this.locY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyPress);
			// 
			// scaleX
			// 
			this.scaleX.Location = new System.Drawing.Point(10, 375);
			this.scaleX.Margin = new System.Windows.Forms.Padding(2);
			this.scaleX.Name = "scaleX";
			this.scaleX.Size = new System.Drawing.Size(238, 31);
			this.scaleX.TabIndex = 6;
			this.scaleX.TextChanged += new System.EventHandler(this.setScale);
			this.scaleX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyPress);
			// 
			// locText
			// 
			this.locText.AutoSize = true;
			this.locText.Location = new System.Drawing.Point(10, 285);
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
			this.scaleText.Location = new System.Drawing.Point(10, 350);
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
			this.ShaderList.Location = new System.Drawing.Point(10, 35);
			this.ShaderList.Margin = new System.Windows.Forms.Padding(2);
			this.ShaderList.Name = "ShaderList";
			this.ShaderList.ScrollAlwaysVisible = true;
			this.ShaderList.Size = new System.Drawing.Size(240, 127);
			this.ShaderList.TabIndex = 0;
			this.ShaderList.SelectedIndexChanged += new System.EventHandler(this.changeShader);
			// 
			// shaderText
			// 
			this.shaderText.AutoSize = true;
			this.shaderText.Location = new System.Drawing.Point(10, 10);
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
			this.PaletteList.Location = new System.Drawing.Point(10, 195);
			this.PaletteList.Margin = new System.Windows.Forms.Padding(2);
			this.PaletteList.Name = "PaletteList";
			this.PaletteList.ScrollAlwaysVisible = true;
			this.PaletteList.Size = new System.Drawing.Size(240, 52);
			this.PaletteList.TabIndex = 1;
			this.PaletteList.SelectedIndexChanged += new System.EventHandler(this.changePalette);
			// 
			// paletteText
			// 
			this.paletteText.AutoSize = true;
			this.paletteText.Location = new System.Drawing.Point(10, 170);
			this.paletteText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.paletteText.Name = "paletteText";
			this.paletteText.Size = new System.Drawing.Size(72, 25);
			this.paletteText.TabIndex = 3;
			this.paletteText.Text = "Palettes";
			// 
			// imaxBox
			// 
			this.imaxBox.Location = new System.Drawing.Point(10, 505);
			this.imaxBox.Margin = new System.Windows.Forms.Padding(2);
			this.imaxBox.Name = "imaxBox";
			this.imaxBox.Size = new System.Drawing.Size(115, 31);
			this.imaxBox.TabIndex = 9;
			this.imaxBox.TextChanged += new System.EventHandler(this.setParameters);
			this.imaxBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyPressInteger);
			// 
			// c2Box
			// 
			this.c2Box.Location = new System.Drawing.Point(135, 440);
			this.c2Box.Margin = new System.Windows.Forms.Padding(2);
			this.c2Box.Name = "c2Box";
			this.c2Box.Size = new System.Drawing.Size(115, 31);
			this.c2Box.TabIndex = 8;
			this.c2Box.TextChanged += new System.EventHandler(this.setParameters);
			this.c2Box.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyPress);
			// 
			// c1Box
			// 
			this.c1Box.Location = new System.Drawing.Point(10, 440);
			this.c1Box.Margin = new System.Windows.Forms.Padding(2);
			this.c1Box.Name = "c1Box";
			this.c1Box.Size = new System.Drawing.Size(115, 31);
			this.c1Box.TabIndex = 7;
			this.c1Box.TextChanged += new System.EventHandler(this.setParameters);
			this.c1Box.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyPress);
			// 
			// cText
			// 
			this.cText.AutoSize = true;
			this.cText.Location = new System.Drawing.Point(10, 415);
			this.cText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.cText.Name = "cText";
			this.cText.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cText.Size = new System.Drawing.Size(91, 25);
			this.cText.TabIndex = 3;
			this.cText.Text = "Parameter";
			// 
			// pointButton
			// 
			this.pointButton.Location = new System.Drawing.Point(9, 550);
			this.pointButton.Margin = new System.Windows.Forms.Padding(2);
			this.pointButton.Name = "pointButton";
			this.pointButton.Size = new System.Drawing.Size(238, 36);
			this.pointButton.TabIndex = 11;
			this.pointButton.Text = "Add Point";
			this.pointButton.UseVisualStyleBackColor = true;
			this.pointButton.Click += new System.EventHandler(this.addPoint);
			// 
			// cursorLocation
			// 
			this.cursorLocation.AutoSize = true;
			this.cursorLocation.Location = new System.Drawing.Point(9, 645);
			this.cursorLocation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.cursorLocation.Name = "cursorLocation";
			this.cursorLocation.Size = new System.Drawing.Size(131, 75);
			this.cursorLocation.TabIndex = 8;
			this.cursorLocation.Text = "CursorLocation\r\nX\r\nY\r\n";
			// 
			// screenshotButton
			// 
			this.screenshotButton.Location = new System.Drawing.Point(9, 595);
			this.screenshotButton.Margin = new System.Windows.Forms.Padding(2);
			this.screenshotButton.Name = "screenshotButton";
			this.screenshotButton.Size = new System.Drawing.Size(238, 36);
			this.screenshotButton.TabIndex = 12;
			this.screenshotButton.Text = "Take Screenshot";
			this.screenshotButton.UseVisualStyleBackColor = true;
			this.screenshotButton.Click += new System.EventHandler(this.takeScreenshot);
			// 
			// limitBox
			// 
			this.limitBox.Location = new System.Drawing.Point(135, 505);
			this.limitBox.Margin = new System.Windows.Forms.Padding(2);
			this.limitBox.Name = "limitBox";
			this.limitBox.Size = new System.Drawing.Size(115, 31);
			this.limitBox.TabIndex = 10;
			this.limitBox.TextChanged += new System.EventHandler(this.setParameters);
			this.limitBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyPress);
			// 
			// imaxText
			// 
			this.imaxText.AutoEllipsis = true;
			this.imaxText.AutoSize = true;
			this.imaxText.Location = new System.Drawing.Point(10, 480);
			this.imaxText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.imaxText.Name = "imaxText";
			this.imaxText.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.imaxText.Size = new System.Drawing.Size(124, 25);
			this.imaxText.TabIndex = 3;
			this.imaxText.Text = "Max Iterations";
			// 
			// limitText
			// 
			this.limitText.AutoEllipsis = true;
			this.limitText.AutoSize = true;
			this.limitText.Location = new System.Drawing.Point(135, 480);
			this.limitText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.limitText.Name = "limitText";
			this.limitText.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.limitText.Size = new System.Drawing.Size(88, 25);
			this.limitText.TabIndex = 3;
			this.limitText.Text = "Max Limit";
			// 
			// SettingsWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(258, 794);
			this.ControlBox = false;
			this.Controls.Add(this.limitText);
			this.Controls.Add(this.imaxText);
			this.Controls.Add(this.limitBox);
			this.Controls.Add(this.screenshotButton);
			this.Controls.Add(this.cursorLocation);
			this.Controls.Add(this.pointButton);
			this.Controls.Add(this.cText);
			this.Controls.Add(this.c1Box);
			this.Controls.Add(this.c2Box);
			this.Controls.Add(this.imaxBox);
			this.Controls.Add(this.paletteText);
			this.Controls.Add(this.PaletteList);
			this.Controls.Add(this.shaderText);
			this.Controls.Add(this.ShaderList);
			this.Controls.Add(this.scaleText);
			this.Controls.Add(this.locText);
			this.Controls.Add(this.scaleX);
			this.Controls.Add(this.locY);
			this.Controls.Add(this.locX);
			this.Controls.Add(this.Info1);
			this.Controls.Add(this.Info0);
			this.Controls.Add(this.pointCheck);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximumSize = new System.Drawing.Size(280, 850);
			this.MinimumSize = new System.Drawing.Size(280, 100);
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
		private System.Windows.Forms.TextBox scaleX;
		private System.Windows.Forms.Label locText;
		private System.Windows.Forms.Label scaleText;
		public System.Windows.Forms.Label Info0;
		public System.Windows.Forms.Label Info1;
		private System.Windows.Forms.ListBox ShaderList;
		private System.Windows.Forms.Label shaderText;
		private System.Windows.Forms.ListBox PaletteList;
		private System.Windows.Forms.Label paletteText;
		private System.Windows.Forms.TextBox imaxBox;
		private System.Windows.Forms.TextBox c2Box;
		private System.Windows.Forms.TextBox c1Box;
		private System.Windows.Forms.Label cText;
		private System.Windows.Forms.Button pointButton;
		private System.Windows.Forms.Label cursorLocation;
		private System.Windows.Forms.Button screenshotButton;
		private System.Windows.Forms.TextBox limitBox;
		private System.Windows.Forms.Label imaxText;
		private System.Windows.Forms.Label limitText;
	}
}

