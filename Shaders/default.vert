#version 130
precision highp float;

in vec4 position;
in vec4 color;
in vec2 texCoord;

uniform mat4 projection;
uniform mat4 modelView;
uniform vec4 modelColor;

out vec4 vs_color;
out vec2 vs_texCoord;

void main(void)
{
    gl_Position = projection * modelView * position;
    vs_color = color * modelColor;
	vs_texCoord = texCoord;
}