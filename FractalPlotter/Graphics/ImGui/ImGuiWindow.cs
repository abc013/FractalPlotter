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
		}

		public void ShowWindow(long lastms, Vector3d cursorLocation)
		{
			ImGui.Begin("Information window");
			ImGui.Spacing();
			ImGui.Checkbox("Show points", ref Settings.Points);
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
			if (ImGui.CollapsingHeader("Viewport settings"))
			{
				var posChanged = false;
				var x = Camera.ExactLocation.X;
				var y = Camera.ExactLocation.Y;

				ImGui.Text("Location");

				posChanged |= ImGui.InputDouble("X", ref x);
				posChanged |= ImGui.InputDouble("Y", ref y);

				if (posChanged)
					Camera.SetTranslation(x, y, 0);

				var s = Camera.Scale;

				ImGui.Text("Scale");

				if (ImGui.InputFloat("S", ref s))
					Camera.SetScale(s);
			}
			if (ImGui.CollapsingHeader("Parameter settings"))
			{
				var factorChanged = false;

				var x = MasterRenderer.Factor1.X;
				var y = MasterRenderer.Factor1.Y;

				ImGui.Text("Parameter value (c)");
				helpButton("This is the parameter value used for e.g. julia fractals." +
					"\nOther shaders may use this parameters for other purposes.");

				factorChanged |= ImGui.InputFloat("X", ref x);
				factorChanged |= ImGui.InputFloat("Y", ref y);

				if (factorChanged)
					MasterRenderer.Factor1 = new Vector2(x, y);

				var i = MasterRenderer.IMax;

				ImGui.Text("Maximum iterations (imax)");
				helpButton("Maximum value to iterate to in order to determine the color of a single pixel.");

				if (ImGui.InputInt("I", ref i))
					MasterRenderer.IMax = i;

				var l = (float)Math.Sqrt(MasterRenderer.SquaredLimit);

				ImGui.Text("Escape criterion value");
				helpButton("Value that determines when the iteration is stopped. Recommended is the value 2.");

				if (ImGui.InputFloat("L", ref l))
					MasterRenderer.SquaredLimit = l*l;
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
			}

			ImGui.NewLine();
			if (ImGui.Button("Add Point at current position"))
				PointManager.Add(Camera.Location, Utils.RandomColor());

			if (ImGui.Button("Take Screenshot"))
				MasterRenderer.TakeScreenshot(0, 0, window.ClientSize.X, window.ClientSize.Y);

			ImGui.NewLine();
			const string str = "00.000000000000";
			ImGui.Text($"cursor at\n{cursorLocation.X.ToString(str)}\n{cursorLocation.Y.ToString(str)}i");

			ImGui.End();
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
