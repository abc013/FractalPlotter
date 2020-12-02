using FractalPlotter.Graphics;
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.ComponentModel;
using System.Diagnostics;

namespace FractalPlotter
{
	/// <summary>
	/// Graph window supported by OpenTK.
	/// </summary>
	public class GraphWindow : GameWindow
	{
		public bool IsClosing { get; private set; }
		public bool IsLoaded { get; private set; }

		readonly GraphSettingsPipe pipe;

		/// <summary>
		/// Stopwatch used for measuring the time each render frame needs.
		/// </summary>
		readonly Stopwatch watch;

		/// <summary>
		/// ImGui controller that is used to handle the ImGui windows.
		/// </summary>
		ImGuiController controller;

		int localTick;

		/// <summary>
		/// Initialize the graph window. This calls a base constructor in OpenTK which handles window creation for us.
		/// </summary>
		public GraphWindow(GraphSettingsPipe pipe, GameWindowSettings gameSettings, NativeWindowSettings nativeSettings) : base(gameSettings, nativeSettings)
		{
			this.pipe = pipe;
			pipe.Add(this);
			watch = new Stopwatch();

			shaders = FileManager.GetGraphShaderNames().ToArray();
			palettes = FileManager.GetPaletteImageNames().ToArray();
		}

		/// <summary>
		/// Signals that the window is now loaded, which means we can load our graphics now.
		/// </summary>
		protected override void OnLoad()
		{
			base.OnLoad();
			MasterRenderer.Load();
			controller = new ImGuiController(ClientSize.X, ClientSize.Y);
			controller.SetScale(2f);
			//ImGui.SetWindowPos(System.Numerics.Vector2.Zero);
			//ImGui.SetWindowCollapsed(true);
			//ImGui.SetWindowSize(new System.Numerics.Vector2(ClientSize.X / 16, ClientSize.Y));

			pipe.UpdateTranslation();
			pipe.UpdateScale();
			pipe.UpdateParameters();

			IsLoaded = true;
		}

		long lastms;
		readonly string[] shaders;
		int currentShader;
		readonly string[] palettes;
		int currentPalette;
		Vector3d cursorLocation;
		/// <summary>
		/// Render frame, which renders the whole window.
		/// </summary>
		protected override void OnRenderFrame(FrameEventArgs args)
		{
			watch.Start();

			if (Camera.Changed)
			{
				cursorLocation = getCursorLocation();
				pipe.UpdateCursorLocation(cursorLocation.X, cursorLocation.Y);
			}

			base.OnRenderFrame(args);
			MasterRenderer.RenderFrame();

			ImGui.Begin("Information window");
			ImGui.Spacing();
			ImGui.Checkbox("Show points", ref Settings.Points);
			if (ImGui.CollapsingHeader("Shaders"))
			{
				ImGui.Text("Please select a shader from the list below.");
				ImGui.Text("Hover above this text for more information.");
				if (ImGui.IsItemHovered())
					ImGui.SetTooltip(".");

				if (ImGui.Combo("Shaders", ref currentShader, shaders, shaders.Length))
					MasterRenderer.ChangeShader(shaders[currentShader]);
			}
			if (ImGui.CollapsingHeader("Palettes"))
			{
				ImGui.Text("Please select a palette from the list below.");
				ImGui.Text("Hover above this text for more information.");
				if (ImGui.IsItemHovered())
					ImGui.SetTooltip(".");

				if (ImGui.Combo("Palettes", ref currentPalette, palettes, palettes.Length))
					MasterRenderer.ChangePalette(palettes[currentShader]);
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

				factorChanged |= ImGui.InputFloat("X", ref x);
				factorChanged |= ImGui.InputFloat("Y", ref y);

				if (factorChanged)
					MasterRenderer.Factor1 = new Vector2(x, y);

				var i = MasterRenderer.IMax;

				ImGui.Text("Maximum number of iterations (imax)");

				if (ImGui.InputInt("I", ref i))
					MasterRenderer.IMax = i;

				var l = MasterRenderer.SquaredLimit;

				ImGui.Text("Escape criterion value");

				if (ImGui.InputFloat("L", ref l))
					MasterRenderer.SquaredLimit = l;
			}
			if (ImGui.CollapsingHeader("Debug"))
			{
				ImGui.Text($"current: {localTick++} ticks");
				ImGui.Text($"render: {lastms} ms");
			}

			ImGui.NewLine();
			if (ImGui.Button("Add Point at current position"))
				PointManager.Add(Camera.Location, Utils.RandomColor());

			if (ImGui.Button("Take Screenshot"))
				MasterRenderer.TakeScreenshot(0, 0, ClientSize.X, ClientSize.Y);

			ImGui.NewLine();
			const string str = "00.000000000000";
			ImGui.Text($"cursor at\n{cursorLocation.X.ToString(str)}\n{cursorLocation.Y.ToString(str)}i");

			ImGui.End();
			
			controller.Render();

			SwapBuffers();

			pipe.PipeInfo($"current: {localTick++} ticks", true);
			pipe.PipeInfo($"render: {watch.ElapsedMilliseconds} ms", false);

			lastms = watch.ElapsedMilliseconds;
			watch.Reset();
		}

		/// <summary>
		/// Update frame, checking for any key movements.
		/// </summary>
		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			base.OnUpdateFrame(args);

			controller.Update(this, (float)args.Time);

			if (ImGui.IsWindowHovered(ImGuiHoveredFlags.AnyWindow))
				return;

			var x = checkKeyRegulator(Keys.Right, Keys.Left);
			var y = checkKeyRegulator(Keys.Up, Keys.Down);

			if (x != 0f || y != 0f)
			{
				Camera.Translate(x, y, 0);
				pipe.UpdateTranslation();
			}

			var dc1 = checkKeyRegulator(Keys.Q, Keys.A);
			var dc2 = checkKeyRegulator(Keys.W, Keys.S);
			var di = checkKeyRegulator(Keys.E, Keys.D);
			var dl = checkKeyRegulator(Keys.R, Keys.F);

			if (dc1 != 0f || dc2 != 0f || di != 0 || dl != 0)
			{
				MasterRenderer.Factor1 += new Vector2(dc1 * Settings.RegulatorSpeed / Camera.Scale, dc2 * Settings.RegulatorSpeed / Camera.Scale);

				MasterRenderer.IMax += di;
				if (MasterRenderer.IMax < 0)
					MasterRenderer.IMax = 0;

				MasterRenderer.SquaredLimit += dl * 0.1f;

				pipe.UpdateParameters();
			}

			if (pipe.TakeScreenshot || KeyboardState.IsKeyDown(Keys.Space))
			{
				pipe.TakeScreenshot = false;
				MasterRenderer.TakeScreenshot(0, 0, ClientSize.X, ClientSize.Y);
			}
		}

		/// <summary>
		/// Checks whether two keys are pressed and determines a value based on it.
		/// </summary>
		/// <returns>if <c>up</c> is pressed, 1. if <c>down</c> is pressed, -1. if both or none are pressed, 0.</returns>
		int checkKeyRegulator(Keys up, Keys down)
		{
			var delta = 0;

			if (KeyboardState.IsKeyDown(up))
				delta++;
			if (KeyboardState.IsKeyDown(down))
				delta--;

			return delta;
		}

		/// <summary>
		/// Adds a point when the mouse is getting clicked on the right button.
		/// Moves to the viewport center to the cursor location when the left button is clicked.
		/// </summary>
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			if (ImGui.IsWindowHovered(ImGuiHoveredFlags.AnyWindow))
				return;

			var location = getCursorLocation();
			if (e.Button == MouseButton.Right)
				pipe.AddPoint(new Vector3((float)location.X, (float)location.Y, (float)location.Z));
			else if (e.Button == MouseButton.Left)
			{
				Camera.SetTranslation(location.X, location.Y, location.Z);
				pipe.UpdateTranslation();
				MousePosition = new Vector2(Bounds.HalfSize.X, Bounds.HalfSize.Y);
			}
		}

		/// <summary>
		/// Updates the cursor location.
		/// </summary>
		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			cursorLocation = getCursorLocation();
			pipe.UpdateCursorLocation(cursorLocation.X, cursorLocation.Y);
		}

		/// <summary>
		/// Calculates the cursor location from screen to virtual space.
		/// </summary>
		Vector3d getCursorLocation()
		{
			var screenX = (MousePosition.X / ClientSize.X) * 4d - 2d;
			var screenY = (MousePosition.Y / ClientSize.Y) * 4d - 2d;
			screenX *= Camera.Ratio;
			screenX /= Camera.Scale;
			screenY /= -Camera.Scale;
			screenX += Camera.ExactLocation.X;
			screenY += Camera.ExactLocation.Y;

			return new Vector3d(screenX, screenY, 0);
		}

		/// <summary>
		/// Scales when the mouse wheel is used.
		/// </summary>
		float currentOffset;
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			if (ImGui.IsWindowHovered(ImGuiHoveredFlags.AnyWindow))
			{
				currentOffset = e.OffsetY;
				return;
			}

			var diff = e.OffsetY - currentOffset;
			currentOffset = e.OffsetY;
			Camera.Scaling(diff * 0.1f);
			pipe.UpdateScale();
		}

		protected override void OnTextInput(TextInputEventArgs e)
		{
			foreach (var c in e.AsString)
				controller.PressChar(c);
		}

		/// <summary>
		/// Updates the MasterRenderer when the viewport is being resized.
		/// </summary>
		protected override void OnResize(ResizeEventArgs e)
		{
			MasterRenderer.ResizeViewport(ClientSize.X, ClientSize.Y);
			controller.WindowResized(ClientSize.X, ClientSize.Y);
		}

		/// <summary>
		/// Closes the pipe and the MasterRenderer.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClosing(CancelEventArgs e)
		{
			IsClosing = true;
			pipe.Exit();

			MasterRenderer.Dispose();
			controller.Dispose();
		}
	}
}
