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
		// calculate the next z with f(x)=x²+c
        float x = (squared.x - squared.y) + c.x;
        float y = 2 * (z.y * z.x) + c.y;

		squared.x = x * x;
		squared.y = y * y;
		// Check whether the value is out of the given limit
        if((squared.x + squared.y) > squaredLimit) break;
        z.x = x;
        z.y = y;
    }

	// Gets the texture based on the palette and a percentage
    color = texture(pal, float(i)/imax);
}