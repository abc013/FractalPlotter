using ComplexNumberGrapher.Graphics;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Common.Input;
using OpenToolkit.Windowing.Desktop;
using System.ComponentModel;
using System.Diagnostics;

namespace ComplexNumberGrapher
{
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

		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			base.OnUpdateFrame(args);

			var x = 0f;
			var y = 0f;
			if (KeyboardState.IsKeyDown(Key.Up))
				y += 1f;
			if (KeyboardState.IsKeyDown(Key.Down))
				y -= 1f;
			if (KeyboardState.IsKeyDown(Key.Right))
				x += 1f;
			if (KeyboardState.IsKeyDown(Key.Left))
				x -= 1f;

			if (x != 0f || y != 0f)
			{
				Camera.Translate(x, y, 0f);
				pipe.UpdateTranslation();
			}

			var f1 = 0f;
			var f2 = 0f;
			if (KeyboardState.IsKeyDown(Key.W))
				f1 -= 1f;
			if (KeyboardState.IsKeyDown(Key.S))
				f1 += 1f;
			if (KeyboardState.IsKeyDown(Key.A))
				f2 += 1f;
			if (KeyboardState.IsKeyDown(Key.D))
				f2 -= 1f;

			if (f1 != 0f || f2 != 0f)
			{
				MasterRenderer.Factor1 += new Vector2(f1 * 0.001f, f2 * 0.001f);
				pipe.UpdateParameters();
			}
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Right)
				pipe.AddPoint(getCursorLocation());
		}

		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			var location = getCursorLocation();
			pipe.UpdateCursorLocation(location.X, location.Y);
		}

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

		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			Camera.Scaling(e.OffsetY * 0.1f);
			pipe.UpdateScale();
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			MasterRenderer.ResizeViewport(ClientSize.X, ClientSize.Y);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			IsClosing = true;
			pipe.Exit();

			MasterRenderer.Dispose();
		}
	}
}
