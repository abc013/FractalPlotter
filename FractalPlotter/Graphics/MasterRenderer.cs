using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;

namespace ComplexNumberGrapher.Graphics
{
	public static class MasterRenderer
	{
		public static Vector2 Factor1;
		public static float Factor2;

		static UniformManager currentManager;
		static int currentPalette;
		static int currentShader;

		public static void Load()
		{
			Camera.Load();

			Factor1 = new Vector2(Settings.Factor1X, Settings.Factor1Y);
			Factor2 = Settings.Factor2;

			foreach (var name in FileManager.GetGraphShaderNames())
				ShaderManager.Add(name);

			foreach (var name in FileManager.GetPaletteImageNames())
				PaletteManager.Add(name);

			ChangeShader(Settings.DefaultShader);
			ChangePalette(Settings.DefaultPalette);

			PointManager.Add(Vector3d.Zero, Color4.Violet);
			PointManager.Add(Vector3d.One, Color4.Yellow);
			PointManager.Add(Vector3d.UnitX, Color4.Blue);
			PointManager.Add(-Vector3d.UnitX, Color4.Red);

			GL.ClearColor(Color4.Black);

			GL.Enable(EnableCap.ScissorTest);
			GL.Enable(EnableCap.AlphaTest);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Blend);

			Utils.CheckError("Load");
		}

		public static void ChangeShader(string name)
		{
			var newShader = ShaderManager.Fetch(name);
			if (newShader > 0)
			{
				currentShader = newShader;
				currentManager = ShaderManager.FetchManager(currentShader);
			}
		}

		public static void ChangePalette(string name)
		{
			currentPalette = PaletteManager.Fetch(name);
		}

		public static void RenderFrame()
		{
			Camera.CalculateMatrix();

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.UseProgram(0);
			GL.LoadMatrix(ref Camera.CameraMatrix);

			PointManager.Render();

			if (!Settings.ThreeDimensional)
			{
				GL.UseProgram(currentShader);
				GL.BindTexture(TextureTarget.Texture1D, currentPalette);

				currentManager.Uniform();

				var posID = UniformManager.PositionID;
				var texcoordID = UniformManager.TexCoordID;

				GL.LoadIdentity();
				GL.Begin(PrimitiveType.Quads);
				GL.VertexAttrib2(texcoordID, new Vector2(-Camera.Ratio * 2f, -1 * 2f));
				GL.VertexAttrib4(posID, new Vector4(-1, -1, 0, 1));
				GL.VertexAttrib2(texcoordID, new Vector2(Camera.Ratio * 2f, -1 * 2f));
				GL.VertexAttrib4(posID, new Vector4(1, -1, 0, 1));
				GL.VertexAttrib2(texcoordID, new Vector2(Camera.Ratio * 2f, 1 * 2f));
				GL.VertexAttrib4(posID, new Vector4(1, 1, 0, 1));
				GL.VertexAttrib2(texcoordID, new Vector2(-Camera.Ratio * 2f, 1 * 2f));
				GL.VertexAttrib4(posID, new Vector4(-1, 1, 0, 1));
				GL.End();
			}

			Utils.CheckError("Render");
		}

		//static void drawweird(Vector3 pos)
		//{
		//	GL.Begin(PrimitiveType.Triangles);
		//	GL.Color3(1f, 0f, 0f);
		//	GL.Vertex3(0.01f + pos.X, 0.01f + pos.Y, 0.01f + pos.Z);
		//	GL.Color3(0f, 1f, 0f);
		//	GL.Vertex3(-0.01f + pos.X, 0.01f + pos.Y, 0.01f + pos.Z);
		//	GL.Color3(0f, 0f, 1f);
		//	GL.Vertex3(0f + pos.X, -0.01f + pos.Y, 0.01f + pos.Z);
		//	GL.End();
		//}

		public static void ResizeViewport(int width, int height)
		{
			GL.Viewport(0, 0, width, height);
			GL.Scissor(0, 0, width, height);

			Camera.ResizeViewport(width, height);

			Utils.CheckError("Resize");
		}

		public static void Dispose()
		{
			ShaderManager.Dispose();
			PaletteManager.Dispose();
		}
	}
}
