﻿using FractalPlotter.FractalPlotter;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ComplexNumberGrapher.Graphics
{
	public static class MasterRenderer
	{
		public static Vector2 Factor1;
		public static int IMax;

		static UniformManager defaultManager;
		static int defaultShader;
		static UniformManager currentManager;
		static int currentShader;

		static int currentPalette;

		static Renderable planeRenderable;
		static Point crosshair1, crosshair2;

		/// <summary>
		/// Initialization process.
		/// </summary>
		public static void Load()
		{
			Camera.Load();

			Factor1 = new Vector2(Settings.Factor1X, Settings.Factor1Y);
			IMax = Settings.IMax;

			// Load in all the shaders found.
			foreach (var name in FileManager.GetGraphShaderNames())
				ShaderManager.Add(name);

			// Load all palettes found.
			foreach (var name in FileManager.GetPaletteImageNames())
				PaletteManager.Add(name);

			// Activate the default shader and palette.
			ChangeShader(Settings.DefaultShader, true);
			ChangePalette(Settings.DefaultPalette);

			// Add some debug points.
			PointRenderable.Load();

			PointManager.Add(Vector3.Zero, Color4.Violet);
			PointManager.Add(Vector3.UnitY, Color4.Red);
			PointManager.Add(-Vector3.UnitY, Color4.Red);
			PointManager.Add(Vector3.UnitX, Color4.Blue);
			PointManager.Add(-Vector3.UnitX, Color4.Blue);

			crosshair1 = new Point(Vector3.Zero, Color4.Black, 0.012f);
			crosshair2 = new Point(new Vector3(0,0,-0.000001f), Color4.White);

			// Configure GL properly.
			GL.ClearColor(Color4.Black);

			GL.Enable(EnableCap.Blend);
			GL.LineWidth(2f);

			// generate a GL Buffer with a plane in it and load it into GPU storage.
			planeRenderable = new Renderable(getPlaneVectors());

			// Check for GL errors.
			Utils.CheckError("Load");
		}

		/// <summary>
		/// Change the shader.
		/// </summary>
		/// <param name="name">Name of the shader.</param>
		/// <param name="default">Parameter to use when the shader is the first loaded.</param>
		public static void ChangeShader(string name, bool @default = false)
		{
			var newShader = ShaderManager.Fetch(name);
			if (newShader > 0)
			{
				currentShader = newShader;
				currentManager = ShaderManager.FetchManager(currentShader);
				if (@default)
				{
					defaultShader = currentShader;
					defaultManager = currentManager;
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

			if (!Settings.ThreeDimensional)
			{
				GL.UseProgram(currentShader);
				GL.BindTexture(TextureTarget.Texture1D, currentPalette);
				currentManager.Uniform();

				planeRenderable.Render();
			}

			if (!Settings.Fancy)
			{
				GL.UseProgram(defaultShader);
				defaultManager.Uniform();

				PointManager.Render();

				defaultManager.UniformProjection(Camera.ScaleMatrix);
				crosshair1.Render();
				crosshair2.Render();
			}

			// Check for GL errors.
			Utils.CheckError("Render");
		}

		public static void UniformColor(Color4 color)
		{
			defaultManager.UniformColor(color);
		}

		public static void UniformObjectMatrix(Matrix4 objectMatrix)
		{
			defaultManager.UniformModelView(objectMatrix);
		}

		/// <summary>
		/// Respond to the resizing of the graph window and update accordingly.
		/// </summary>
		public static void ResizeViewport(int width, int height)
		{
			GL.Viewport(0, 0, width, height);
			GL.Scissor(0, 0, width, height);

			Camera.ResizeViewport(width, height);

			planeRenderable.ChangeBuffer(getPlaneVectors());

			Utils.CheckError("Resize");
		}

		static Vector[] getPlaneVectors()
		{
			return new[]
			{
				new Vector(new Vector4(-1, -1, 0, 1), Color4.White, new Vector2(-Camera.Ratio * 2f, -1 * 2f)),
				new Vector(new Vector4(-1, 1, 0, 1), Color4.White, new Vector2(-Camera.Ratio * 2f, 1 * 2f)),
				new Vector(new Vector4(1, -1, 0, 1), Color4.White, new Vector2(Camera.Ratio * 2f, -1 * 2f)),
				new Vector(new Vector4(1, -1, 0, 1), Color4.White, new Vector2(Camera.Ratio * 2f, -1 * 2f)),
				new Vector(new Vector4(-1, 1, 0, 1), Color4.White, new Vector2(-Camera.Ratio * 2f, 1 * 2f)),
				new Vector(new Vector4(1, 1, 0, 1), Color4.White, new Vector2(Camera.Ratio * 2f, 1 * 2f)),
			};
		}

		/// <summary>
		/// Clean up.
		/// </summary>
		public static void Dispose()
		{
			planeRenderable.Dispose();

			PointManager.Dispose();
			PointRenderable.Dispose();

			ShaderManager.Dispose();
			PaletteManager.Dispose();
		}
	}
}
