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
			this.fancyCheck = new System.Windows.Forms.CheckBox();
			this.Info0 = new System.Windows.Forms.Label();
			this.Info1 = new System.Windows.Forms.Label();
			this.locX = new System.Windows.Forms.TextBox();
			this.locY = new System.Windows.Forms.TextBox();
			this.locZ = new System.Windows.Forms.TextBox();
			this.scaleX = new System.Windows.Forms.TextBox();
			this.rotX = new System.Windows.Forms.TextBox();
			this.rotY = new System.Windows.Forms.TextBox();
			this.rotZ = new System.Windows.Forms.TextBox();
			this.locText = new System.Windows.Forms.Label();
			this.scaleText = new System.Windows.Forms.Label();
			this.rotText = new System.Windows.Forms.Label();
			this.ShaderList = new System.Windows.Forms.ListBox();
			this.shaderText = new System.Windows.Forms.Label();
			this.PaletteList = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.dimensionCheck = new System.Windows.Forms.CheckBox();
			this.imaxBox = new System.Windows.Forms.TextBox();
			this.c2Box = new System.Windows.Forms.TextBox();
			this.c1Box = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.cursorLocation = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// fancyCheck
			// 
			this.fancyCheck.AutoSize = true;
			this.fancyCheck.Checked = true;
			this.fancyCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fancyCheck.Location = new System.Drawing.Point(12, 211);
			this.fancyCheck.Name = "fancyCheck";
			this.fancyCheck.Size = new System.Drawing.Size(106, 36);
			this.fancyCheck.TabIndex = 1;
			this.fancyCheck.Text = "Fancy";
			this.fancyCheck.UseVisualStyleBackColor = true;
			// 
			// Info0
			// 
			this.Info0.AutoSize = true;
			this.Info0.Location = new System.Drawing.Point(12, 936);
			this.Info0.Name = "Info0";
			this.Info0.Size = new System.Drawing.Size(99, 32);
			this.Info0.TabIndex = 3;
			this.Info0.Text = "Debug0";
			// 
			// Info1
			// 
			this.Info1.AutoSize = true;
			this.Info1.Location = new System.Drawing.Point(12, 968);
			this.Info1.Name = "Info1";
			this.Info1.Size = new System.Drawing.Size(99, 32);
			this.Info1.TabIndex = 3;
			this.Info1.Text = "Debug1";
			// 
			// locX
			// 
			this.locX.Location = new System.Drawing.Point(12, 408);
			this.locX.Name = "locX";
			this.locX.Size = new System.Drawing.Size(89, 39);
			this.locX.TabIndex = 4;
			// 
			// locY
			// 
			this.locY.Location = new System.Drawing.Point(107, 408);
			this.locY.Name = "locY";
			this.locY.Size = new System.Drawing.Size(89, 39);
			this.locY.TabIndex = 5;
			// 
			// locZ
			// 
			this.locZ.Location = new System.Drawing.Point(202, 408);
			this.locZ.Name = "locZ";
			this.locZ.Size = new System.Drawing.Size(89, 39);
			this.locZ.TabIndex = 6;
			// 
			// scaleX
			// 
			this.scaleX.Location = new System.Drawing.Point(12, 494);
			this.scaleX.Name = "scaleX";
			this.scaleX.Size = new System.Drawing.Size(279, 39);
			this.scaleX.TabIndex = 7;
			// 
			// rotX
			// 
			this.rotX.Location = new System.Drawing.Point(12, 575);
			this.rotX.Name = "rotX";
			this.rotX.Size = new System.Drawing.Size(89, 39);
			this.rotX.TabIndex = 8;
			// 
			// rotY
			// 
			this.rotY.Location = new System.Drawing.Point(107, 575);
			this.rotY.Name = "rotY";
			this.rotY.Size = new System.Drawing.Size(89, 39);
			this.rotY.TabIndex = 9;
			// 
			// rotZ
			// 
			this.rotZ.Location = new System.Drawing.Point(202, 575);
			this.rotZ.Name = "rotZ";
			this.rotZ.Size = new System.Drawing.Size(89, 39);
			this.rotZ.TabIndex = 10;
			// 
			// locText
			// 
			this.locText.AutoSize = true;
			this.locText.Location = new System.Drawing.Point(12, 373);
			this.locText.Name = "locText";
			this.locText.Size = new System.Drawing.Size(104, 32);
			this.locText.TabIndex = 3;
			this.locText.Tag = "";
			this.locText.Text = "Location";
			// 
			// scaleText
			// 
			this.scaleText.AutoSize = true;
			this.scaleText.Location = new System.Drawing.Point(12, 459);
			this.scaleText.Name = "scaleText";
			this.scaleText.Size = new System.Drawing.Size(110, 32);
			this.scaleText.TabIndex = 3;
			this.scaleText.Text = "Scalation";
			// 
			// rotText
			// 
			this.rotText.AutoSize = true;
			this.rotText.Location = new System.Drawing.Point(12, 540);
			this.rotText.Name = "rotText";
			this.rotText.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.rotText.Size = new System.Drawing.Size(103, 32);
			this.rotText.TabIndex = 3;
			this.rotText.Text = "Rotation";
			// 
			// ShaderList
			// 
			this.ShaderList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ShaderList.ItemHeight = 32;
			this.ShaderList.Location = new System.Drawing.Point(12, 41);
			this.ShaderList.Name = "ShaderList";
			this.ShaderList.ScrollAlwaysVisible = true;
			this.ShaderList.Size = new System.Drawing.Size(310, 162);
			this.ShaderList.TabIndex = 0;
			this.ShaderList.SelectedIndexChanged += new System.EventHandler(this.changeShader);
			// 
			// shaderText
			// 
			this.shaderText.AutoSize = true;
			this.shaderText.Location = new System.Drawing.Point(12, 6);
			this.shaderText.Name = "shaderText";
			this.shaderText.Size = new System.Drawing.Size(98, 32);
			this.shaderText.TabIndex = 3;
			this.shaderText.Text = "Shaders";
			// 
			// PaletteList
			// 
			this.PaletteList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PaletteList.ItemHeight = 32;
			this.PaletteList.Location = new System.Drawing.Point(12, 282);
			this.PaletteList.Name = "PaletteList";
			this.PaletteList.ScrollAlwaysVisible = true;
			this.PaletteList.Size = new System.Drawing.Size(309, 66);
			this.PaletteList.TabIndex = 3;
			this.PaletteList.SelectedIndexChanged += new System.EventHandler(this.changePalette);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 247);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 32);
			this.label1.TabIndex = 3;
			this.label1.Text = "Palettes";
			// 
			// dimensionCheck
			// 
			this.dimensionCheck.AutoSize = true;
			this.dimensionCheck.Location = new System.Drawing.Point(124, 211);
			this.dimensionCheck.Name = "dimensionCheck";
			this.dimensionCheck.Size = new System.Drawing.Size(76, 36);
			this.dimensionCheck.TabIndex = 2;
			this.dimensionCheck.Text = "3D";
			this.dimensionCheck.UseVisualStyleBackColor = true;
			// 
			// imaxBox
			// 
			this.imaxBox.Location = new System.Drawing.Point(202, 658);
			this.imaxBox.Name = "imaxBox";
			this.imaxBox.Size = new System.Drawing.Size(89, 39);
			this.imaxBox.TabIndex = 13;
			// 
			// c2Box
			// 
			this.c2Box.Location = new System.Drawing.Point(107, 658);
			this.c2Box.Name = "c2Box";
			this.c2Box.Size = new System.Drawing.Size(89, 39);
			this.c2Box.TabIndex = 12;
			// 
			// c1Box
			// 
			this.c1Box.Location = new System.Drawing.Point(12, 658);
			this.c1Box.Name = "c1Box";
			this.c1Box.Size = new System.Drawing.Size(89, 39);
			this.c1Box.TabIndex = 11;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 623);
			this.label2.Name = "label2";
			this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label2.Size = new System.Drawing.Size(274, 32);
			this.label2.TabIndex = 3;
			this.label2.Text = "Parameters (c1, c2, imax)";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(33, 747);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(229, 38);
			this.button1.TabIndex = 14;
			this.button1.Text = "Add Point";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.addPoint);
			// 
			// cursorLocation
			// 
			this.cursorLocation.AutoSize = true;
			this.cursorLocation.Location = new System.Drawing.Point(12, 840);
			this.cursorLocation.Name = "cursorLocation";
			this.cursorLocation.Size = new System.Drawing.Size(173, 96);
			this.cursorLocation.TabIndex = 8;
			this.cursorLocation.Text = "CursorLocation\r\nX\r\nY\r\n";
			// 
			// SettingsWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(334, 1009);
			this.ControlBox = false;
			this.Controls.Add(this.cursorLocation);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.c1Box);
			this.Controls.Add(this.c2Box);
			this.Controls.Add(this.imaxBox);
			this.Controls.Add(this.dimensionCheck);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.PaletteList);
			this.Controls.Add(this.shaderText);
			this.Controls.Add(this.ShaderList);
			this.Controls.Add(this.rotText);
			this.Controls.Add(this.scaleText);
			this.Controls.Add(this.locText);
			this.Controls.Add(this.rotZ);
			this.Controls.Add(this.rotY);
			this.Controls.Add(this.rotX);
			this.Controls.Add(this.scaleX);
			this.Controls.Add(this.locZ);
			this.Controls.Add(this.locY);
			this.Controls.Add(this.locX);
			this.Controls.Add(this.Info1);
			this.Controls.Add(this.Info0);
			this.Controls.Add(this.fancyCheck);
			this.MaximumSize = new System.Drawing.Size(360, 1080);
			this.MinimumSize = new System.Drawing.Size(360, 71);
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

		private System.Windows.Forms.CheckBox fancyCheck;
		private System.Windows.Forms.TextBox locX;
		private System.Windows.Forms.TextBox locY;
		private System.Windows.Forms.TextBox locZ;
		private System.Windows.Forms.TextBox scaleX;
		private System.Windows.Forms.TextBox rotX;
		private System.Windows.Forms.TextBox rotY;
		private System.Windows.Forms.TextBox rotZ;
		private System.Windows.Forms.Label locText;
		private System.Windows.Forms.Label scaleText;
		private System.Windows.Forms.Label rotText;
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
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label cursorLocation;
	}
}

