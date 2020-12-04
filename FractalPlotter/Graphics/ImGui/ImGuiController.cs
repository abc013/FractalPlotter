using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FractalPlotter.Graphics
{
	/// <summary>
	/// A modified version of Veldrid.ImGui's ImGuiRenderer.
	/// Manages input for ImGui and handles rendering ImGui's DrawLists with Veldrid.
	/// Taken from https://github.com/NogginBops/ImGui.NET_OpenTK_Sample
	/// </summary>
	public class ImGuiController : IDisposable
	{
		public bool BufferChanged = true;
		public int VertexBufferSize => vertexBufferSize;
		public int IndexBufferSize => indexBufferSize;

		bool firstFrameOver;

		// objects
		int vertexArray;
		int vertexBuffer;
		int vertexBufferSize;
		int indexBuffer;
		int indexBufferSize;

		ImGuiTexture fontTexture;
		ImGuiShader shader;

		int windowWidth;
		int windowHeight;

		System.Numerics.Vector2 scaleFactor = System.Numerics.Vector2.One;

		/// <summary>
		/// Constructs a new ImGuiController.
		/// </summary>
		public ImGuiController(int width, int height)
		{
			windowWidth = width;
			windowHeight = height;

			IntPtr context = ImGui.CreateContext();
			ImGui.SetCurrentContext(context);
			var io = ImGui.GetIO();
			io.Fonts.AddFontDefault();

			io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

			CreateDeviceResources();
			setKeyMappings();

			updateData(1f / 60f);

			ImGui.NewFrame();
			firstFrameOver = true;
		}

		public void WindowResized(int width, int height)
		{
			windowWidth = width;
			windowHeight = height;
		}

		public void SetScale(float scale)
		{
			scaleFactor = new System.Numerics.Vector2(scale);
		}

		public void CreateDeviceResources()
		{
			GL.CreateVertexArrays(1, out vertexArray);

			// Magic start values, are increased when needed
			vertexBufferSize = 10000;
			indexBufferSize = 2000;

			GL.CreateBuffers(1, out vertexBuffer);
			GL.CreateBuffers(1, out indexBuffer);

			GL.NamedBufferData(vertexBuffer, vertexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
			GL.NamedBufferData(indexBuffer, indexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);

			RecreateFontDeviceTexture();

			const string VertexSource = @"#version 330 core

uniform mat4 projection_matrix;

layout(location = 0) in vec2 in_position;
layout(location = 1) in vec2 in_texCoord;
layout(location = 2) in vec4 in_color;

out vec4 color;
out vec2 texCoord;

void main()
{
	gl_Position = projection_matrix * vec4(in_position, 0, 1);
	color = in_color;
	texCoord = in_texCoord;
}";
			const string FragmentSource = @"#version 330 core

uniform sampler2D in_fontTexture;

in vec4 color;
in vec2 texCoord;

out vec4 outputColor;

void main()
{
	outputColor = color * texture(in_fontTexture, texCoord);
}";

			shader = new ImGuiShader("ImGui", VertexSource, FragmentSource);

			GL.VertexArrayVertexBuffer(vertexArray, 0, vertexBuffer, IntPtr.Zero, Unsafe.SizeOf<ImDrawVert>());
			GL.VertexArrayElementBuffer(vertexArray, indexBuffer);

			GL.EnableVertexArrayAttrib(vertexArray, 0);
			GL.VertexArrayAttribBinding(vertexArray, 0, 0);
			GL.VertexArrayAttribFormat(vertexArray, 0, 2, VertexAttribType.Float, false, 0);

			GL.EnableVertexArrayAttrib(vertexArray, 1);
			GL.VertexArrayAttribBinding(vertexArray, 1, 0);
			GL.VertexArrayAttribFormat(vertexArray, 1, 2, VertexAttribType.Float, false, 8);

			GL.EnableVertexArrayAttrib(vertexArray, 2);
			GL.VertexArrayAttribBinding(vertexArray, 2, 0);
			GL.VertexArrayAttribFormat(vertexArray, 2, 4, VertexAttribType.UnsignedByte, true, 16);

			Utils.CheckError("End of ImGui setup");
		}

		/// <summary>
		/// Recreates the device texture used to render text.
		/// </summary>
		public void RecreateFontDeviceTexture()
		{
			ImGuiIOPtr io = ImGui.GetIO();
			io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height, out _);

			fontTexture = new ImGuiTexture("ImGui Text Atlas", width, height, pixels);
			fontTexture.SetMagFilter(TextureMagFilter.Linear);
			fontTexture.SetMinFilter(TextureMinFilter.Linear);

			io.Fonts.SetTexID((IntPtr)fontTexture.GLTexture);

			io.Fonts.ClearTexData();
		}

		/// <summary>
		/// Renders the ImGui draw list data.
		/// This method requires a <see cref="GraphicsDevice"/> because it may create new DeviceBuffers if the size of vertex
		/// or index data has increased beyond the capacity of the existing buffers.
		/// A <see cref="CommandList"/> is needed to submit drawing and resource update commands.
		/// </summary>
		public void Render()
		{
			if (firstFrameOver)
			{
				firstFrameOver = false;
				ImGui.Render();
				renderInternal(ImGui.GetDrawData());
			}
		}

		/// <summary>
		/// Updates ImGui input and IO configuration state.
		/// </summary>
		public void Update(GameWindow wnd, float deltaSeconds)
		{
			if (firstFrameOver)
				ImGui.Render();

			updateData(deltaSeconds);
			updateInput(wnd);

			firstFrameOver = true;
			ImGui.NewFrame();
		}

		/// <summary>
		/// Sets per-frame data based on the associated window.
		/// This is called by Update(float).
		/// </summary>
		void updateData(float deltaSeconds)
		{
			var io = ImGui.GetIO();
			io.DisplaySize = new System.Numerics.Vector2(
				windowWidth / scaleFactor.X,
				windowHeight / scaleFactor.Y);
			io.DisplayFramebufferScale = scaleFactor;
			io.DeltaTime = deltaSeconds; // DeltaTime is in seconds.
		}

		readonly List<char> pressedChars = new List<char>();

		void updateInput(GameWindow wnd)
		{
			var io = ImGui.GetIO();

			var mouseState = wnd.MouseState;
			var keyboardState = wnd.KeyboardState;

			io.MouseDown[0] = mouseState.IsButtonDown(MouseButton.Left);
			io.MouseDown[1] = mouseState.IsButtonDown(MouseButton.Right);
			io.MouseDown[2] = mouseState.IsButtonDown(MouseButton.Middle);

			io.MousePos = new System.Numerics.Vector2(wnd.MousePosition.X / scaleFactor.X, wnd.MousePosition.Y / scaleFactor.Y);

			io.MouseWheel = mouseState.ScrollDelta.Y;
			io.MouseWheelH = mouseState.ScrollDelta.X;

			foreach (Keys key in Enum.GetValues(typeof(Keys)))
			{
				var keyNum = (int)key;
				if (keyNum < 0 || keyNum >= io.KeysDown.Count)
					continue;

				io.KeysDown[keyNum] = keyboardState.IsKeyDown(key);
			}

			foreach (var c in pressedChars)
				io.AddInputCharacter(c);
			pressedChars.Clear();

			io.KeyCtrl = keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl);
			io.KeyAlt = keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt);
			io.KeyShift = keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift);
			io.KeySuper = keyboardState.IsKeyDown(Keys.LeftSuper) || keyboardState.IsKeyDown(Keys.RightSuper);
		}

		internal void PressChar(char keyChar)
		{
			pressedChars.Add(keyChar);
		}

		static void setKeyMappings()
		{
			ImGuiIOPtr io = ImGui.GetIO();
			io.KeyMap[(int)ImGuiKey.Tab] = (int)Keys.Tab;
			io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)Keys.Left;
			io.KeyMap[(int)ImGuiKey.RightArrow] = (int)Keys.Right;
			io.KeyMap[(int)ImGuiKey.UpArrow] = (int)Keys.Up;
			io.KeyMap[(int)ImGuiKey.DownArrow] = (int)Keys.Down;
			io.KeyMap[(int)ImGuiKey.PageUp] = (int)Keys.PageUp;
			io.KeyMap[(int)ImGuiKey.PageDown] = (int)Keys.PageDown;
			io.KeyMap[(int)ImGuiKey.Home] = (int)Keys.Home;
			io.KeyMap[(int)ImGuiKey.End] = (int)Keys.End;
			io.KeyMap[(int)ImGuiKey.Delete] = (int)Keys.Delete;
			io.KeyMap[(int)ImGuiKey.Backspace] = (int)Keys.Backspace;
			io.KeyMap[(int)ImGuiKey.Enter] = (int)Keys.Enter;
			io.KeyMap[(int)ImGuiKey.Escape] = (int)Keys.Escape;
			io.KeyMap[(int)ImGuiKey.A] = (int)Keys.A;
			io.KeyMap[(int)ImGuiKey.C] = (int)Keys.C;
			io.KeyMap[(int)ImGuiKey.V] = (int)Keys.V;
			io.KeyMap[(int)ImGuiKey.X] = (int)Keys.X;
			io.KeyMap[(int)ImGuiKey.Y] = (int)Keys.Y;
			io.KeyMap[(int)ImGuiKey.Z] = (int)Keys.Z;
		}

		void renderInternal(ImDrawDataPtr draw_data)
		{
			BufferChanged = false;

			if (draw_data.CmdListsCount == 0)
				return;

			var vertexSize = draw_data.TotalVtxCount * Unsafe.SizeOf<ImDrawVert>();
			if (vertexSize > vertexBufferSize)
			{
				BufferChanged = true;

				var newSize = (int)(vertexSize * 1.5f);
				GL.DeleteBuffer(vertexBuffer);
				GL.CreateBuffers(1, out vertexBuffer);
				GL.NamedBufferData(vertexBuffer, newSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
				GL.VertexArrayVertexBuffer(vertexArray, 0, vertexBuffer, IntPtr.Zero, Unsafe.SizeOf<ImDrawVert>());
				vertexBufferSize = newSize;
			}

			var indexSize = draw_data.TotalIdxCount * sizeof(ushort);
			if (indexSize > indexBufferSize)
			{
				BufferChanged = true;

				var newSize = (int)(indexSize * 1.5f);
				GL.DeleteBuffer(indexBuffer);
				GL.CreateBuffers(1, out indexBuffer);
				GL.NamedBufferData(indexBuffer, newSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
				GL.VertexArrayElementBuffer(vertexArray, indexBuffer);
				indexBufferSize = newSize;
			}

			// Setup orthographic projection matrix into our constant buffer
			var io = ImGui.GetIO();
			var mvp = Matrix4.CreateOrthographicOffCenter(
				0.0f,
				io.DisplaySize.X,
				io.DisplaySize.Y,
				0.0f,
				-1.0f,
				1.0f);

			GL.UseProgram(shader.Program);
			GL.UniformMatrix4(shader.GetUniformLocation("projection_matrix"), false, ref mvp);
			GL.Uniform1(shader.GetUniformLocation("in_fontTexture"), 0);
			Utils.CheckError("Projection");

			GL.BindVertexArray(vertexArray);
			Utils.CheckError("VAO");

			draw_data.ScaleClipRects(io.DisplayFramebufferScale);

			GL.Enable(EnableCap.ScissorTest);
			GL.BlendEquation(BlendEquationMode.FuncAdd);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			GL.Disable(EnableCap.CullFace);
			GL.Disable(EnableCap.DepthTest);
			Utils.CheckError($"Render state");

			// Render command lists
			for (int n = 0; n < draw_data.CmdListsCount; n++)
			{
				var cmd_list = draw_data.CmdListsRange[n];

				GL.NamedBufferSubData(vertexBuffer, IntPtr.Zero, cmd_list.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>(), cmd_list.VtxBuffer.Data);
				Utils.CheckError($"Data Vert {n}");

				GL.NamedBufferSubData(indexBuffer, IntPtr.Zero, cmd_list.IdxBuffer.Size * sizeof(ushort), cmd_list.IdxBuffer.Data);
				Utils.CheckError($"Data Idx {n}");

				int idx_offset = 0;
				for (int cmd_i = 0; cmd_i < cmd_list.CmdBuffer.Size; cmd_i++)
				{
					ImDrawCmdPtr pcmd = cmd_list.CmdBuffer[cmd_i];
					if (pcmd.UserCallback != IntPtr.Zero)
						throw new NotImplementedException();

					GL.ActiveTexture(TextureUnit.Texture0);
					GL.BindTexture(TextureTarget.Texture2D, (int)pcmd.TextureId);
					Utils.CheckError("Texture");

					// We do _windowHeight - (int)clip.W instead of (int)clip.Y because gl has flipped Y when it comes to these coordinates
					var clip = pcmd.ClipRect;
					GL.Scissor((int)clip.X, windowHeight - (int)clip.W, (int)(clip.Z - clip.X), (int)(clip.W - clip.Y));
					Utils.CheckError("Scissor");

					if ((io.BackendFlags & ImGuiBackendFlags.RendererHasVtxOffset) != 0)
						GL.DrawElementsBaseVertex(PrimitiveType.Triangles, (int)pcmd.ElemCount, DrawElementsType.UnsignedShort, (IntPtr)(idx_offset * sizeof(ushort)), 0);
					else
						GL.DrawElements(BeginMode.Triangles, (int)pcmd.ElemCount, DrawElementsType.UnsignedShort, (int)pcmd.IdxOffset * sizeof(ushort));
					Utils.CheckError("Draw");

					idx_offset += (int)pcmd.ElemCount;
				}
			}

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, 0);

			GL.Disable(EnableCap.ScissorTest);

			GL.BindVertexArray(0);
		}

		/// <summary>
		/// Frees all graphics resources used by the renderer.
		/// </summary>
		public void Dispose()
		{
			fontTexture.Dispose();
			shader.Dispose();
		}
	}
}
