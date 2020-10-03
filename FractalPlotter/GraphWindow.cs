using ComplexNumberGrapher.Graphics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.ComponentModel;
using System.Diagnostics;

namespace ComplexNumberGrapher
{
	/// <summary>
	/// Graph window supported by OpenTK.
	/// </summary>
	public class GraphWindow : GameWindow
	{
		public bool IsClosing { get; private set; }
		public bool IsLoaded { get; private set; }

		readonly GraphSettingsPipe pipe;

		readonly Stopwatch watch;

		int localTick;

		public GraphWindow(GraphSettingsPipe pipe, GameWindowSettings gameSettings, NativeWindowSettings nativeSettings) : base(gameSettings, nativeSettings)
		{
			this.pipe = pipe;
			pipe.Add(this);
			watch = new Stopwatch();
		}

		protected override void OnLoad()
		{
			base.OnLoad();
			MasterRenderer.Load();

			pipe.UpdateTranslation();
			pipe.UpdateScale();
			pipe.UpdateRotation();
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

			var x = 0f;
			var y = 0f;
			if (KeyboardState.IsKeyDown(Keys.Up))
				y += 1f;
			if (KeyboardState.IsKeyDown(Keys.Down))
				y -= 1f;
			if (KeyboardState.IsKeyDown(Keys.Right))
				x += 1f;
			if (KeyboardState.IsKeyDown(Keys.Left))
				x -= 1f;

			if (x != 0f || y != 0f)
			{
				Camera.Translate(x, y, 0f);
				pipe.UpdateTranslation();
			}

			var f1 = 0f;
			var f2 = 0f;
			if (KeyboardState.IsKeyDown(Keys.Q))
				f1 += 1f;
			if (KeyboardState.IsKeyDown(Keys.A))
				f1 -= 1f;
			if (KeyboardState.IsKeyDown(Keys.W))
				f2 += 1f;
			if (KeyboardState.IsKeyDown(Keys.S))
				f2 -= 1f;

			if (f1 != 0f || f2 != 0f)
			{
				MasterRenderer.Factor1 += new Vector2(f1 * 0.001f, f2 * 0.001f);
				pipe.UpdateParameters();
			}
		}

		/// <summary>
		/// Adds a point when the mouse is getting clicked.
		/// </summary>
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Right)
				pipe.AddPoint(getCursorLocation());
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
			// Don't calculate in threedimensional space
			if (Settings.ThreeDimensional)
				return Vector3d.Zero;

			var screenX = (MousePosition.X / ClientSize.X) * 4d - 2d;
			var screenY = (MousePosition.Y / ClientSize.Y) * 4d - 2d;
			screenX *= Camera.Ratio;
			screenX /= Camera.Scale.X;
			screenY /= -Camera.Scale.Y;
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
