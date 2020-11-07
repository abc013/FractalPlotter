#version 130
precision highp float;

in vec4 vs_color;
in vec2 vs_texCoord;

uniform sampler1D pal;

uniform vec2 location;
uniform float scale;
uniform int imax;
uniform float squaredLimit;

out vec4 color;

// Original: http://nuclear.mutantstargoat.com/articles/sdr_fract/
void main(void)
{
    vec2 z, c;

    c.x = vs_texCoord.x / scale + location.x;
    c.y = vs_texCoord.y / scale + location.y;

	// theory in https://www.ibiblio.org/e-notes/MSet/period.htm, js: https://www.ibiblio.org/e-notes/MSet/js/FBTFractal1w.js
	float cr = c.x;
	c.x = 0.25 - c.x*c.x + c.y * c.y;
	c.y = 2 * cr*c.y;

    z = c;

	vec2 squared = vec2(z.x * z.x, z.y * z.y);

    int i;
    for(i=0; i < imax; i++)
    {
        float x = (squared.x - squared.y) + c.x;
        float y = 2 * (z.y * z.x) + c.y;

		squared.x = x * x;
		squared.y = y * y;
        if((squared.x + squared.y) > squaredLimit) break;
        z.x = x;
        z.y = y;
    }

    color = texture(pal, float(i)/imax);
}