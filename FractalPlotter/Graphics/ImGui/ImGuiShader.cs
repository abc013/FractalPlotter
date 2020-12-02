using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FractalPlotter.Graphics
{
    struct UniformFieldInfo
    {
        public int Location;
        public string Name;
        public int Size;
        public ActiveUniformType Type;
    }

    /// <summary>
    /// Shader used for ImGui.
    /// Taken from https://github.com/NogginBops/ImGui.NET_OpenTK_Sample, thanks to NogginBops!
    /// </summary>
    class ImGuiShader
    {
        public readonly string Name;
        public int Program { get; private set; }
        readonly Dictionary<string, int> uniformToLocation = new Dictionary<string, int>();
        bool initialized = false;

        public ImGuiShader(string name, string vertexShader, string fragmentShader)
        {
            Name = name;
            var files = new[]{
                (ShaderType.VertexShader, vertexShader),
                (ShaderType.FragmentShader, fragmentShader),
            };
            Program = createProgram(name, files);
        }

        public void Dispose()
        {
            if (initialized)
            {
                GL.DeleteProgram(Program);
                initialized = false;
            }
        }

        public UniformFieldInfo[] GetUniforms()
        {
            GL.GetProgram(Program, GetProgramParameterName.ActiveUniforms, out int UnifromCount);

            UniformFieldInfo[] Uniforms = new UniformFieldInfo[UnifromCount];

            for (int i = 0; i < UnifromCount; i++)
            {
                string Name = GL.GetActiveUniform(Program, i, out int Size, out ActiveUniformType Type);

                UniformFieldInfo FieldInfo;
                FieldInfo.Location = GetUniformLocation(Name);
                FieldInfo.Name = Name;
                FieldInfo.Size = Size;
                FieldInfo.Type = Type;

                Uniforms[i] = FieldInfo;
            }

            return Uniforms;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUniformLocation(string uniform)
        {
            if (uniformToLocation.TryGetValue(uniform, out int location) == false)
            {
                location = GL.GetUniformLocation(Program, uniform);
                uniformToLocation.Add(uniform, location);

                if (location == -1)
                    Debug.Print($"The uniform '{uniform}' does not exist in the shader '{Name}'!");
            }

            return location;
        }

        int createProgram(string name, params (ShaderType Type, string source)[] shaderPaths)
        {
            var program = GL.CreateProgram();

            var shaders = new int[shaderPaths.Length];
            for (int i = 0; i < shaderPaths.Length; i++)
                shaders[i] = compileShader(shaderPaths[i].Type, shaderPaths[i].source);

            foreach (var shader in shaders)
                GL.AttachShader(program, shader);

            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int Success);
            if (Success == 0)
            {
                string Info = GL.GetProgramInfoLog(program);
                Debug.WriteLine($"GL.LinkProgram had info log [{name}]:\n{Info}");
            }

            foreach (var Shader in shaders)
            {
                GL.DetachShader(program, Shader);
                GL.DeleteShader(Shader);
            }

            initialized = true;

            return program;
        }

        int compileShader(ShaderType type, string source)
        {
            var shader = GL.CreateShader(type);

            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string Info = GL.GetShaderInfoLog(shader);
                Debug.WriteLine($"GL.CompileShader for shader '{Name}' [{type}] had info log:\n{Info}");
            }

            return shader;
        }
    }
}
