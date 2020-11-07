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
    dvec2 z, c;

    c.x = double(vs_texCoord.x) / double(scale) + exactLocation.x;
    c.y = double(vs_texCoord.y) / double(scale) + exactLocation.y;

	// theory in https://www.ibiblio.org/e-notes/MSet/period.htm, js: https://www.ibiblio.org/e-notes/MSet/js/FBTFractal1w.js
	double cr = c.x;
	c.x = 0.25 - c.x*c.x + c.y * c.y;
	c.y = 2 * cr*c.y;

    z = c;

	dvec2 squared = dvec2(z.x * z.x, z.y * z.y);

    int i;
    for(i=0; i < imax; i++)
    {
        double x = (squared.x - squared.y) + c.x;
        double y = 2 * (z.y * z.x) + c.y;

		squared.x = x * x;
		squared.y = y * y;
        if((squared.x + squared.y) > squaredLimit) break;
        z.x = x;
        z.y = y;
    }

    color = texture(pal, float(i)/imax);
}