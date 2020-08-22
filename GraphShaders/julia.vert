#version 330 core
#extension GL_ARB_gpu_shader_fp64 : enable
precision highp float;

in vec4 position;
in vec4 color;
in vec2 texCoord;

out vec4 vs_color;
out vec2 vs_texCoord;

void main(void)
{
    gl_Position = position;
    vs_color = color;
	vs_texCoord = texCoord;
}