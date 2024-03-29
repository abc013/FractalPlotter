﻿using FractalPlotter.Graphics;
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

		/// <summary>
		/// If set to true, a screenshot will be taken next or in this frame.
		/// </summary>
		bool screenshot;

		/// <summary>
		/// Stopwatch used for measuring the time each render frame needs.
		/// </summary>
		readonly Stopwatch watch;

		/// <summary>
		/// ImGui controller that is used to handle the ImGui windows.
		/// </summary>
		ImGuiController controller;
		/// <summary>
		/// ImGui window that contains some window states and controls what is drawn.
		/// </summary>
		ImGuiWindow window;

		/// <summary>
		/// Initialize the graph window. This calls a base constructor in OpenTK which handles window creation for us.
		/// </summary>
		public GraphWindow(GameWindowSettings gameSettings, NativeWindowSettings nativeSettings) : base(gameSettings, nativeSettings)
		{
			watch = new Stopwatch();

			// Initialize values
			int maxWidth, maxHeight;
			unsafe
			{
				var mode = GLFW.GetVideoMode(CurrentMonitor.ToUnsafePtr<Monitor>());
				maxWidth = mode->Width;
				maxHeight = mode->Height;
			}

			if (Settings.GraphWidth > maxWidth)
				Settings.GraphWidth = maxWidth;
			if (Settings.GraphHeight > maxHeight)
				Settings.GraphHeight = maxHeight;
		}

		/// <summary>
		/// Signals that the window is now loaded, which means we can load our graphics now.
		/// </summary>
		protected override void OnLoad()
		{
			base.OnLoad();
			MasterRenderer.Load();

			if (Settings.UseSystemUIScaling)
			{
				if (TryGetCurrentMonitorDpi(out float hdpi, out _))
					Settings.UIScaling = hdpi / 100;
				else
					Log.WriteInfo("Failed to fetch system dpi scaling.");
			}

			controller = new ImGuiController(ClientSize.X, ClientSize.Y);
			controller.SetScale(Settings.UIScaling);
			window = new ImGuiWindow(this, controller);

			var minimum = ClientRectangle.Min;
			ClientRectangle = new Box2i(minimum.X, minimum.Y, Settings.GraphWidth + minimum.X, Settings.GraphHeight + minimum.Y);

			IsLoaded = true;
		}

		long lastms;
		Vector3d cursorLocation;

		/// <summary>
		/// Render frame, which renders the whole window.
		/// </summary>
		protected override void OnRenderFrame(FrameEventArgs args)
		{
			if (IsExiting)
				return;

			watch.Start();

			if (Camera.Changed)
				cursorLocation = getCursorLocation();

			base.OnRenderFrame(args);
			MasterRenderer.RenderFrame();

			window.ShowWindow(lastms, cursorLocation);

			// Without UI, since it isn't rendered yet
			if (screenshot && !Settings.ScreenshotUI)
				takeScreenshot();

			controller.Render();

			// With UI, since it is rendered now
			if (screenshot && Settings.ScreenshotUI)
				takeScreenshot();

			SwapBuffers();

			lastms = watch.ElapsedMilliseconds;
			watch.Reset();
		}

		/// <summary>
		/// Update frame, checking for any key movements.
		/// </summary>
		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			if (IsExiting)
				return;

			base.OnUpdateFrame(args);

			controller.Update(this, (float)args.Time);

			// Time spent from last update frame to this one, multiplied to fit earlier values.
			// This is used so that, although we are lagging, we are still using correct values for input over time.
			var timeFactor = (float)(UpdateTime * 250);

			if (ImGui.IsAnyItemActive())
				return;

			var x = checkKeyRegulator(Keys.Right, Keys.Left, timeFactor);
			var y = checkKeyRegulator(Keys.Up, Keys.Down, timeFactor);

			if (x != 0f || y != 0f)
				Camera.Translate(x, y, 0);

			var dc1 = checkKeyRegulator(Keys.Q, Keys.A, timeFactor);
			var dc2 = checkKeyRegulator(Keys.W, Keys.S, timeFactor);
			var di = checkKeyRegulator(Keys.E, Keys.D, timeFactor);
			var dl = checkKeyRegulator(Keys.R, Keys.F, timeFactor);

			if (dc1 != 0f || dc2 != 0f || di != 0 || dl != 0)
			{
				MasterRenderer.Factor1 += new Vector2(dc1 * Settings.RegulatorSpeed / Camera.Scale, dc2 * Settings.RegulatorSpeed / Camera.Scale);

				MasterRenderer.IMax += (int)di;
				if (MasterRenderer.IMax < 0)
					MasterRenderer.IMax = 0;

				MasterRenderer.SquaredLimit += dl * 0.1f;
			}

			if (KeyboardState.IsKeyDown(Keys.Space))
				DoScreenshot();
		}

		/// <summary>
		/// Checks whether two keys are pressed and determines a value based on it.
		/// </summary>
		/// <returns>if <c>up</c> is pressed, 1. if <c>down</c> is pressed, -1. if both or none are pressed, 0.</returns>
		float checkKeyRegulator(Keys up, Keys down, float timeFactor)
		{
			var delta = 0;

			if (KeyboardState.IsKeyDown(up))
				delta++;
			if (KeyboardState.IsKeyDown(down))
				delta--;

			return delta * timeFactor;
		}

		/// <summary>
		/// Tells that this or next frame, a screenshot has to be taken.
		/// </summary>
		public void DoScreenshot()
		{
			screenshot = true;
		}

		/// <summary>
		/// Takes a screenshot.
		/// </summary>
		void takeScreenshot()
		{
			MasterRenderer.TakeScreenshot(0, 0, ClientSize.X, ClientSize.Y);
			screenshot = false;
		}

		/// <summary>
		/// Adds a point when the mouse is getting clicked on the right button.
		/// Moves to the viewport center to the cursor location when the left button is clicked.
		/// </summary>
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			if (ImGui.IsWindowHovered(ImGuiHoveredFlags.AnyWindow))
				return;

			var location = cursorLocation;
			if (e.Button == MouseButton.Right)
				PointManager.Add(new Vector3((float)location.X, (float)location.Y, (float)location.Z), Utils.GetColor());
			else if (e.Button == MouseButton.Left)
			{
				Camera.SetTranslation(location.X, location.Y, location.Z);
				MousePosition = new Vector2(Bounds.HalfSize.X, Bounds.HalfSize.Y);
			}
		}

		/// <summary>
		/// Updates the cursor location.
		/// </summary>
		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			cursorLocation = getCursorLocation();
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
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			if (ImGui.IsWindowHovered(ImGuiHoveredFlags.AnyWindow))
				return;

			Camera.Scaling(e.OffsetY * 0.1f);
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
			MasterRenderer.ResizeViewport(e.Width, e.Height);
			controller.WindowResized(e.Width, e.Height);
		}

		/// <summary>
		/// Closes the pipe and the MasterRenderer.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClosing(CancelEventArgs e)
		{
			IsClosing = true;

			MasterRenderer.Dispose();
			controller.Dispose();
		}
	}
}
