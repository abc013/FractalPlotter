#version 130
precision highp float;

in vec4 vs_color;
in vec2 vs_texCoord;

uniform sampler1D pal;

uniform vec2 location;
uniform float scale;
uniform int imax;

uniform vec2 fac1;

out vec4 color;

// Original: http://nuclear.mutantstargoat.com/articles/sdr_fract/
void main(void)
{
	float n = fac1.x;
	
	if (n == 0)
		return;

    vec2 z, c;

    c.x = vs_texCoord.x / scale + location.x;
    c.y = vs_texCoord.y / scale + location.y;
	
    z = c;

    int i;
    for(i=0; i < imax; i++)
    {
		float pow = pow(z.x * z.x + z.y * z.y, (n / 2));
		float natan = n * atan(z.y, z.x);
		float xtmp = pow * cos(natan) + c.x;
		float y = pow * sin(natan) + c.y;
		float x = xtmp;

        if((x * x + y * y) > 4.0) break;
        z.x = x;
        z.y = y;
    }

    color = texture(pal, float(i)/imax);
}