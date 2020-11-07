#version 130
precision highp float;

in vec4 vs_color;
in vec2 vs_texCoord;

uniform sampler1D pal;

uniform vec2 location;
uniform float scale;
uniform int imax;
uniform float squaredLimit;

uniform vec2 fac1;

out vec4 color;

// Original: http://nuclear.mutantstargoat.com/articles/sdr_fract/
void main()
{
    vec2 z, c;
	
	c = fac1;
	
    z.x = vs_texCoord.x / scale + location.x;
    z.y = vs_texCoord.y / scale + location.y;

    int i;
    for(i=0; i < imax; i++)
	{
        float x = (z.x * z.x - z.y * z.y) + c.x;
        float y = 2 * (z.y * z.x) + c.y;

        if((x * x + y * y) > squaredLimit) break;
        z.x = x;
        z.y = y;
    }

    color = texture(pal, float(i)/imax);
}