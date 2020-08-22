#version 130
precision highp float;

in vec4 position;
in vec4 color;
in vec2 texCoord;

uniform mat4 projection;

out vec4 vs_color;
out vec2 vs_texCoord;

void main(void)
{
    gl_Position = position * projection;
    vs_color = color;
	vs_texCoord = texCoord;
}