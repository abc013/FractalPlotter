using FractalPlotter.Graphics;
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

		int localTick;

		/// <summary>
		/// Initialize the graph window. This calls a base constructor in OpenTK which handles window creation for us.
		/// </summary>
		public GraphWindow(GraphSettingsPipe pipe, GameWindowSettings gameSettings, NativeWindowSettings nativeSettings) : base(gameSettings, nativeSettings)
		{
			this.pipe = pipe;
			pipe.Add(this);
			watch = new Stopwatch();
		}

		/// <summary>
		/// Signals that the window is now loaded, which means we can load our graphics now.
		/// </summary>
		protected override void OnLoad()
		{
			base.OnLoad();
			MasterRenderer.Load();

			pipe.UpdateTranslation();
			pipe.UpdateScale();
			pipe.UpdateParameters();

			IsLoaded = true;
		}

		/// <summary>
		/// Render frame, which renders the whole window.
		/// </summary>
		protected override void OnRenderFrame(FrameEventArgs args)
		{
			watch.Start();

			if (Camera.Changed)
			{
				var location = getCursorLocation();
				pipe.UpdateCursorLocation(location.X, location.Y);
			}

			base.OnRenderFrame(args);
			MasterRenderer.RenderFrame();

			SwapBuffers();

			pipe.PipeInfo($"current: {localTick++} ticks", true);
			pipe.PipeInfo($"render: {watch.ElapsedMilliseconds} ms", false);

			watch.Reset();
		}

		/// <summary>
		/// Update frame, checking for any key movements.
		/// </summary>
		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			base.OnUpdateFrame(args);

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
		/// Adds a point when the mouse is getting clicked.
		/// </summary>
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Right)
			{
				var location = getCursorLocation();
				pipe.AddPoint(new Vector3((float)location.X, (float)location.Y, (float)location.Z));
			}
		}

		/// <summary>
		/// Updates the cursor location.
		/// </summary>
		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			var location = getCursorLocation();
			pipe.UpdateCursorLocation(location.X, location.Y);
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
			Camera.Scaling(e.OffsetY * 0.1f);
			pipe.UpdateScale();
		}

		/// <summary>
		/// Updates the MasterRenderer when the viewport is being resized.
		/// </summary>
		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			MasterRenderer.ResizeViewport(ClientSize.X, ClientSize.Y);
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
		}
	}
}
