#version 130
precision highp float;

in vec4 vs_color;
in vec2 vs_texCoord;

uniform sampler1D pal;

uniform vec2 location;
uniform float scale;
uniform int imax;

void main(void)
{
    vec2 z, c;

    c.x = vs_texCoord.x / scale + location.x;
    c.y = vs_texCoord.y / scale + location.y;
	
    z = c;

    int i;
    for(i=0; i < imax; i++)
    {
        float x = (z.x * z.x - z.y * z.y) + c.x;
        float y = (z.y * z.x + z.x * z.y) + c.y;

        if((x * x + y * y) > 4.0) break;
        z.x = x;
        z.y = y;
    }

    gl_FragColor = texture1D(pal, float(i)/imax);
}