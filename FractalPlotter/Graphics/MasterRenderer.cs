using FractalPlotter.FractalPlotter;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;

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

		static int plane;
		static int bufferID;

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
			PointManager.Add(Vector3d.Zero, Color4.Violet);
			PointManager.Add(Vector3d.One, Color4.Yellow);
			PointManager.Add(Vector3d.UnitX, Color4.Blue);
			PointManager.Add(-Vector3d.UnitX, Color4.Red);

			// Configure GL properly.
			GL.ClearColor(Color4.Black);
			GL.Enable(EnableCap.ScissorTest);
			GL.Enable(EnableCap.AlphaTest);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Blend);

			// generate a GL Buffer with a plane in it and load it into GPU storage.
			bufferID = GL.GenBuffer();
			plane = GL.GenVertexArray();
			GL.BindVertexArray(plane);
			GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);
			GL.BufferData(BufferTarget.ArrayBuffer, Vector.Size * 6, IntPtr.Zero, BufferUsageHint.DynamicRead);
			updateFractalDisplay();

			GL.EnableVertexAttribArray(UniformManager.PositionID);
			GL.VertexAttribPointer(UniformManager.PositionID, 4, VertexAttribPointerType.Float, true, Vector.Size, 0);

			GL.EnableVertexAttribArray(UniformManager.ColorID);
			GL.VertexAttribPointer(UniformManager.ColorID, 4, VertexAttribPointerType.Float, true, Vector.Size, 16);

			GL.EnableVertexAttribArray(UniformManager.TexCoordID);
			GL.VertexAttribPointer(UniformManager.TexCoordID, 2, VertexAttribPointerType.Float, true, Vector.Size, 32);

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

			GL.UseProgram(defaultShader);
			defaultManager.Uniform();

			if (!Settings.Fancy)
				PointManager.Render();

			if (!Settings.ThreeDimensional)
			{
				GL.UseProgram(currentShader);
				GL.BindTexture(TextureTarget.Texture1D, currentPalette);

				currentManager.Uniform();

				GL.BindVertexArray(plane);
				GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);
				GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
			}

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

			updateFractalDisplay();

			Utils.CheckError("Resize");
		}

		/// <summary>
		/// Generate the fractal plane and load it into GPU memory.
		/// </summary>
		static void updateFractalDisplay()
		{
			var array = new[]
			{
				new Vector(new Vector4(-1, -1, 0, 1), Color4.White, new Vector2(-Camera.Ratio * 2f, -1 * 2f)),
				new Vector(new Vector4(-1, 1, 0, 1), Color4.White, new Vector2(-Camera.Ratio * 2f, 1 * 2f)),
				new Vector(new Vector4(1, -1, 0, 1), Color4.White, new Vector2(Camera.Ratio * 2f, -1 * 2f)),
				new Vector(new Vector4(1, -1, 0, 1), Color4.White, new Vector2(Camera.Ratio * 2f, -1 * 2f)),
				new Vector(new Vector4(-1, 1, 0, 1), Color4.White, new Vector2(-Camera.Ratio * 2f, 1 * 2f)),
				new Vector(new Vector4(1, 1, 0, 1), Color4.White, new Vector2(Camera.Ratio * 2f, 1 * 2f)),
			};

			GL.BindVertexArray(plane);
			GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);
			GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, Vector.Size * 6, array);
		}

		/// <summary>
		/// Clean up.
		/// </summary>
		public static void Dispose()
		{
			ShaderManager.Dispose();
			PaletteManager.Dispose();
		}
	}
}
