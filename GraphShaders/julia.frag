#version 330 core
#extension GL_ARB_gpu_shader_fp64 : enable
precision highp float;

in vec4 vs_color;
in vec2 vs_texCoord;

uniform sampler1D pal;

uniform dvec2 exactlocation;
uniform float scale;
uniform int imax;

uniform vec2 fac1;

void main()
{
    dvec2 z, c;
	
	c = fac1;
	
    z.x = double(vs_texCoord.x) / double(scale) + exactlocation.x;
    z.y = double(vs_texCoord.y) / double(scale) + exactlocation.y;

    int i;
    for(i=0; i < imax; i++)
	{
        double x = (z.x * z.x - z.y * z.y) + c.x;
        double y = (z.y * z.x + z.x * z.y) + c.y;

        if((x * x + y * y) > 4.0) break;
        z.x = x;
        z.y = y;
    }

    gl_FragColor = texture1D(pal, float(i)/imax);
}