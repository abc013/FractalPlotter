using ImGuiNET;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace FractalPlotter.Graphics
{
	public class ImGuiWindow
	{
		readonly GraphWindow window;
		readonly ImGuiController controller;

		int localTick;
		bool firstTick = true;
		bool showDialog;

		readonly string[] shaders;
		int currentShader;

		readonly string[] palettes;
		int currentPalette;

		readonly Queue<float> lastMS = new Queue<float>();
		readonly Queue<float> lastVertexBufferSize = new Queue<float>();
		readonly Queue<float> lastIndexBufferSize = new Queue<float>();

		public ImGuiWindow(GraphWindow window, ImGuiController controller)
		{
			this.window = window;
			this.controller = controller;

			shaders = FileManager.GetGraphShaderNames().ToArray();
			palettes = FileManager.GetPaletteImageNames().ToArray();

			showDialog = Settings.ShowWelcomeDialog;
		}

		public void ShowWindow(long lastms, Vector3d cursorLocation)
		{
			if (firstTick && Settings.AutoResizeWindow)
			{
				ImGui.SetNextWindowPos(System.Numerics.Vector2.Zero);
				ImGui.SetNextWindowSize(new System.Numerics.Vector2(window.ClientSize.X / (Settings.UIScaling * 4), window.ClientSize.Y / Settings.UIScaling));
				firstTick = false;
			}
			ImGui.Begin("Information Window");
			ImGui.Spacing();
			if (ImGui.CollapsingHeader("Shaders"))
			{
				ImGui.Text("Select a shader from the list below.");
				helpButton("Change the current shader by clicking on one of the names below." +
					"\nThe shaders themselves can be found in the 'Shaders' directory.");

				if (ImGui.ListBox("Shaders", ref currentShader, shaders, shaders.Length))
					MasterRenderer.ChangeShader(shaders[currentShader]);
			}
			if (ImGui.CollapsingHeader("Palettes"))
			{
				ImGui.Text("Select a palette from the list below.");
				helpButton("Change current palette by clicking on one of the names below." +
					"\nThe palettes can be found in the 'Palettes' directory." +
					"\nPlease note that the default palettes contain 256 different colors." +
					"\nThis means that, when increasing imax over that limit, no more colors will be added.");

				if (ImGui.ListBox("Palettes", ref currentPalette, palettes, palettes.Length))
					MasterRenderer.ChangePalette(palettes[currentPalette]);
			}
			if (ImGui.CollapsingHeader("Point settings"))
			{
				ImGui.Checkbox("Show points", ref Settings.Points);
				if (ImGui.TreeNode("Points"))
				{
					var points = PointManager.Points;
					var pointsToRemove = new List<Point>();
					for (int i = 0; i < points.Count; i++)
					{
						var point = points[i];
						if (ImGui.TreeNode($"Point{i}"))
						{
							ImGui.Text($"X: {point.Position.X}");
							ImGui.SameLine();
							ImGui.Text($"Y: {point.Position.Y}i");

							if (ImGui.Button("Copy to clipboard"))
								ImGui.SetClipboardText($"{point.Position.X},{point.Position.Y}");

							var c = new System.Numerics.Vector4(point.Color.R, point.Color.G, point.Color.B, point.Color.A);
							if (ImGui.ColorEdit4("", ref c))
								point.Color = new Color4(c.X, c.Y, c.Z, c.W);

							if (ImGui.Button("Remove"))
								pointsToRemove.Add(point);
							ImGui.TreePop();
						}
					}
					ImGui.TreePop();

					if (pointsToRemove.Count != 0)
						points.RemoveAll(p => pointsToRemove.Contains(p));
				}
				ImGui.TextWrapped("Place points by clicking the right mouse button or using the button.");
				ImGui.Checkbox("Use random color", ref Settings.UseRandomColor);
				if (!Settings.UseRandomColor)
				{
					ImGui.NewLine();
					var c = new System.Numerics.Vector4(Utils.StandardColor.R, Utils.StandardColor.G, Utils.StandardColor.B, Utils.StandardColor.A);
					if (ImGui.ColorPicker4("Color", ref c))
						Utils.StandardColor = new Color4(c.X, c.Y, c.Z, c.W);
				}
			}
			if (ImGui.CollapsingHeader("Viewport settings"))
			{
				var posChanged = false;
				var x = Camera.ExactLocation.X;
				var y = Camera.ExactLocation.Y;

				ImGui.Text("Location");
				helpButton("Hotkeys: [Arrows]");

				posChanged |= ImGui.InputDouble("X", ref x, Camera.RelativeSpeed, Camera.RelativeSpeed * 5);
				posChanged |= ImGui.InputDouble("Y", ref y, Camera.RelativeSpeed, Camera.RelativeSpeed * 5);

				if (posChanged)
					Camera.SetTranslation(x, y, 0);

				var s = Camera.Scale;

				ImGui.Text("Scale");

				if (ImGui.InputFloat("S", ref s, .1f, .2f))
					Camera.SetScale(s);

				ImGui.NewLine();
				ImGui.Text("Camera Speed");
				helpButton("Determines how fast the viewport is moved around by pressing keys.");
				ImGui.SliderFloat("C-Speed", ref Settings.CameraSpeed, 0.001f, .02f, "%.3f");
			}
			if (ImGui.CollapsingHeader("Parameter settings"))
			{
				var factorChanged = false;

				var x = MasterRenderer.Factor1.X;
				var y = MasterRenderer.Factor1.Y;

				ImGui.Text("Parameter value (c)");
				helpButton("This is the parameter value used for e.g. julia fractals." +
					"\nOther shaders may use this parameters for other purposes." +
					"\nHotkeys: [cX: Q, A; cY: W, S]");

				factorChanged |= ImGui.InputFloat("cX", ref x, Settings.RegulatorSpeed, Settings.RegulatorSpeed * 5);
				factorChanged |= ImGui.InputFloat("cY", ref y, Settings.RegulatorSpeed, Settings.RegulatorSpeed * 5);

				if (factorChanged)
					MasterRenderer.Factor1 = new Vector2(x, y);

				var i = MasterRenderer.IMax;

				ImGui.Text("Maximum iterations (imax)");
				helpButton("Maximum value to iterate to in order to determine the color of a single pixel." +
					"\nHotkeys: [E, D]");

				if (ImGui.InputInt("I", ref i))
					MasterRenderer.IMax = i;

				var l = (float)Math.Sqrt(MasterRenderer.SquaredLimit);

				ImGui.Text("Escape criterion value");
				helpButton("Value that determines when the iteration is stopped. Recommended is the value 2." +
					"\nHotkeys: [R, F]");

				if (ImGui.InputFloat("L", ref l, Settings.RegulatorSpeed, Settings.RegulatorSpeed * 5))
					MasterRenderer.SquaredLimit = l*l;

				ImGui.NewLine();
				ImGui.Text("Change Speed");
				helpButton("Determines how fast the c parameter are changed when pressing keys.");
				ImGui.SliderFloat("P-Speed", ref Settings.RegulatorSpeed, 0.0001f, .01f, "%.4f");
			}

			lastMS.Enqueue(lastms);
			if (lastMS.Count > 300)
				lastMS.Dequeue();

			if (controller.BufferChanged)
			{
				lastVertexBufferSize.Enqueue(controller.VertexBufferSize);
				lastIndexBufferSize.Enqueue(controller.IndexBufferSize);
			}

			if (ImGui.CollapsingHeader("Debug"))
			{
				ImGui.Text($"current: {localTick++} ticks");
				ImGui.Text($"render: {lastms} ms");

				var array = lastMS.ToArray();
				ImGui.PlotLines("graph", ref array[0], array.Length);

				ImGui.Text("buffer history");
				var array2 = lastVertexBufferSize.ToArray();
				ImGui.PlotHistogram("vertex", ref array2[0], array2.Length);

				var array3 = lastIndexBufferSize.ToArray();
				ImGui.PlotHistogram("index", ref array3[0], array3.Length);

				if (ImGui.Button("Show welcome dialog", new System.Numerics.Vector2(ImGui.GetWindowContentRegionMax().X, 20)))
					showDialog = true;
			}

			ImGui.NewLine();
			if (ImGui.Button("Add Point at current position", new System.Numerics.Vector2(ImGui.GetWindowContentRegionMax().X, 20)))
				PointManager.Add(Camera.Location, Utils.GetColor());

			if (ImGui.Button("Take Screenshot", new System.Numerics.Vector2(ImGui.GetWindowContentRegionMax().X, 20)))
				window.DoScreenshot();

			ImGui.Checkbox("Show UI in Screenshot", ref Settings.ScreenshotUI);

			ImGui.NewLine();
			const string str = "00.000000000000";
			ImGui.Text($"cursor at\n{cursorLocation.X.ToString(str)}\n{cursorLocation.Y.ToString(str)}i");

			ImGui.End();

			if (showDialog)
			{
				ImGui.SetNextWindowPos(new System.Numerics.Vector2(window.ClientSize.X / (Settings.UIScaling * 2) - 150, window.ClientSize.Y / (Settings.UIScaling * 2) - 110));
				ImGui.SetNextWindowSize(new System.Numerics.Vector2(300, 220));
				ImGui.Begin("Welcome!", ImGuiWindowFlags.NoDecoration);
				ImGui.Text("Welcome to FractalPlotter!");
				ImGui.TextWrapped("Click on the headers to open/close their contents.");
				ImGui.TextWrapped("You can get more information about the parameters when hovering above this symbol: ");
				ImGui.Text("Parameter");
				helpButton("Parameter information!");
				ImGui.TextWrapped("The information window can be resized by clicking and dragging the lower right corner. It can be closed by clicking on the arrow next to the title.");
				ImGui.Text("Enjoy!");
				if (ImGui.Button("Close this window", new System.Numerics.Vector2(ImGui.GetWindowContentRegionMax().X, 20)))
					showDialog = false;
				ImGui.End();
			}
		}

		static void helpButton(string description)
		{
			ImGui.SameLine();
			ImGui.TextDisabled("[?]");
			if (ImGui.IsItemHovered())
				ImGui.SetTooltip(description);
		}
	}
}
