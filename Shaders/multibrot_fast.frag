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
void main(void)
{
	float n = fac1.x;
	
	if (n == 0)
		return;

    vec2 z, c;

	// Make c based on pixel coordinates
    c.x = vs_texCoord.x / scale + location.x;
    c.y = vs_texCoord.y / scale + location.y;
	
	// Instead of setting z to 0, we can set it to c as we know that the first iteration would do that
	// This means we save one iteration!
    z = c;

	// save the squared values of z and use them. This will improve performance as we need to calculate the squared values for both calculating the next z
	// and for checking whether the value is out of the limit.
	vec2 squared = vec2(z.x * z.x, z.y * z.y);

    int i;
    for(i=0; i < imax; i++)
    {
		// calculate the next z with polar coordinates.
		float pow = pow(squared.x + squared.y, (n / 2));
		float natan = n * atan(z.y, z.x);
		float xtmp = pow * cos(natan) + c.x;
		float y = pow * sin(natan) + c.y;
		float x = xtmp;

		squared.x = x * x;
		squared.y = y * y;
        if((squared.x + squared.y) > squaredLimit) break;
        z.x = x;
        z.y = y;
    }

	// Gets the texture based on the palette and a percentage
    color = texture(pal, float(i)/imax);
}