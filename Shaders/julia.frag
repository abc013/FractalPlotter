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

uniform vec2 fac1;

out vec4 color;

// Original: http://nuclear.mutantstargoat.com/articles/sdr_fract/
void main()
{
    dvec2 z, c;
	
	// Set c to the given factor
	c = fac1;
	
	// Make z based on pixel coordinates
    z.x = double(vs_texCoord.x) / double(scale) + exactLocation.x;
    z.y = double(vs_texCoord.y) / double(scale) + exactLocation.y;

	// save the squared values of z and use them. This will improve performance as we need to calculate the squared values for both calculating the next z
	// and for checking whether the value is out of the limit.
	dvec2 squared = dvec2(z.x * z.x, z.y * z.y);

    int i;
    for(i=0; i < imax; i++)
	{
		// calculate the next z with f(x)=x²+c
        double x = (squared.x - squared.y) + c.x;
        double y = 2 * (z.y * z.x) + c.y;

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