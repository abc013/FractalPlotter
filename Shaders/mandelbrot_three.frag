#version 330 core
// http://blog.hvidtfeldts.net/index.php/2012/07/double-precision-in-opengl-and-webgl/
#extension GL_ARB_gpu_shader_fp64 : enable
precision highp float;

in vec4 vs_color;
in vec2 vs_texCoord;

uniform sampler1D pal;

uniform dvec2 exactLocation;
uniform float scale;
uniform int imax;
uniform float squaredLimit;

out vec4 color;

// Original: http://nuclear.mutantstargoat.com/articles/sdr_fract/
void main(void)
{
	double n = 3;
    dvec2 z, c;

	// Make c based on pixel coordinates
    c.x = double(vs_texCoord.x) / double(scale) + exactLocation.x;
    c.y = double(vs_texCoord.y) / double(scale) + exactLocation.y;
	
	// Instead of setting z to 0, we can set it to c as we know that the first iteration would do that
	// This means we save one iteration!
    z = c;

	// save the squared values of z and use them. This will improve performance as we need to calculate the squared values for both calculating the next z
	// and for checking whether the value is out of the limit.
	dvec2 squared = dvec2(z.x * z.x, z.y * z.y);

    int i;
    for(i=0; i < imax; i++)
    {
		// calculate the next z with f(x)=x^3+c
        double x = z.x * (squared.x - 3 * squared.y) + c.x;
        double y = z.y * (3 * squared.x - squared.y) + c.y;

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