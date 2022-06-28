using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace FractalPlotter.Graphics
{
	/// <summary>
	/// Master class dedicated to controlling the interaction with GL.
	/// For more information about how GL works, visit https://glumpy.github.io/modern-gl.html.
	/// </summary>
	public static class MasterRenderer
	{
		public static Vector2 Factor1;
		public static int IMax;
		public static float SquaredLimit;

		// Default shader: It is used to render points and the crosshair.
		public static UniformManager DefaultManager { get; private set; }
		public static int DefaultShader { get; private set; }

		// Current shader: It is used to render the current fractal.
		static UniformManager currentManager;
		static int currentShader;

		// Current palette ID to use.
		static int currentPalette;

		static Point crosshair1, crosshair2;

		/// <summary>
		/// Initialization process.
		/// </summary>
		public static void Load()
		{
			Camera.Load();

			// Set variables to the settings default.
			Factor1 = new Vector2(Settings.Factor1X, Settings.Factor1Y);
			IMax = Settings.IMax;
			SquaredLimit = Settings.Limit * Settings.Limit;

			// Load in all the shaders found.
			foreach (var name in FileManager.GetGraphShaderNames())
				ShaderManager.Add(name);

			// Load all palettes found.
			foreach (var name in FileManager.GetPaletteImageNames())
				PaletteManager.Add(name);

			// Set the default shader for points
			ChangeShader(Settings.PointShader, true);

			// Activate the default shader and palette.
			ChangeShader(Settings.DefaultShader);
			ChangePalette(Settings.DefaultPalette);

			// Configure GL properly.
			GL.ClearColor(Color4.Black);

			GL.Enable(EnableCap.Blend);
			GL.LineWidth(2f);

			// generate a GL Buffer with a plane in it and load it into GPU storage.
			PointRenderable.Load();
			PlaneRenderable.Load();

			// Initialize the crosshair.
			crosshair1 = new Point(Vector3.Zero, Color4.Black, 0.012f);
			crosshair2 = new Point(new Vector3(0, 0, -0.000001f), Color4.White);

			// Check for GL errors.
			Utils.CheckError("Load");
		}

		/// <summary>
		/// Change the shader.
		/// </summary>
		/// <param name="name">Name of the shader.</param>
		/// <param name="default">Sets the default shader to this shader.</param>
		public static void ChangeShader(string name, bool @default = false)
		{
			var newShader = ShaderManager.Fetch(name);
			if (newShader > 0)
			{
				currentShader = newShader;
				currentManager = ShaderManager.FetchManager(currentShader);
				if (@default)
				{
					DefaultShader = currentShader;
					DefaultManager = currentManager;
				}
			}
			else
			{
				Log.WriteInfo($"Failed to fetch shader {name}.");

				if (@default)
					throw new DefaultShaderException();
			}
		}

		/// <summary>
		/// Update the palette by using its name.
		/// </summary>
		/// <param name="name">Name of the palette.</param>
		public static void ChangePalette(string name)
		{
			currentPalette = PaletteManager.Fetch(name);
		}

		/// <summary>
		/// Renders a frame into the frame buffer. First, it is rendering the points and then the shaders.
		/// Please note that depth buffering is enabled, which means that everything is rendered in depth order, not in call order.
		/// </summary>
		public static void RenderFrame()
		{
			Camera.CalculateMatrix();

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.UseProgram(currentShader);
			GL.BindTexture(TextureTarget.Texture1D, currentPalette);
			currentManager.Uniform();

			PlaneRenderable.Render();

			if (Settings.Points)
			{
				GL.UseProgram(DefaultShader);
				DefaultManager.Uniform();

				PointManager.Render();

				DefaultManager.UniformProjection(Camera.ScaleMatrix);
				crosshair1.Render();
				crosshair2.Render();
			}

			// Check for GL errors.
			Utils.CheckError("Render");
		}

		/// <summary>
		/// Respond to the resizing of the graph window and update accordingly.
		/// </summary>
		public static void ResizeViewport(int width, int height)
		{
			GL.Viewport(0, 0, width, height);
			GL.Scissor(0, 0, width, height);

			Camera.ResizeViewport(width, height);

			PlaneRenderable.UpdateBuffer();

			Utils.CheckError("Resize");
		}

		/// <summary>
		/// Used source: https://stackoverflow.com/questions/5844858/how-to-take-screenshot-in-opengl
		/// </summary>
		public static void TakeScreenshot(int x, int y, int width, int height)
		{
			var data = new byte[width * height * 3];
			GL.ReadPixels(x, y, width, height, PixelFormat.Bgr, PixelType.UnsignedByte, data);

			Utils.CheckError("Screenshot");

			FileManager.SaveScreenshot(data, width, height);
		}

		/// <summary>
		/// Clean up.
		/// </summary>
		public static void Dispose()
		{
			PointManager.Dispose();
			PointRenderable.Dispose();
			PlaneRenderable.Dispose();

			ShaderManager.Dispose();
			PaletteManager.Dispose();
		}
	}
}
