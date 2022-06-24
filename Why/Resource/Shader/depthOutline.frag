#version 330 core

in vec2 v_TexCoord;
in vec2 v_OneTexel;

uniform sampler2D _color;
uniform sampler2D _depth;
uniform float _radius;
uniform float _threshold;

out vec4 v_Color;

void main() 
{
    bool foundDepth = false;
    
    float centerDepth = texture(_depth, v_TexCoord).r;
    float thresh = _threshold * centerDepth;
    
    for (float i = -_radius; i <= _radius; i++)
    {
        for (float j = -_radius; j <= _radius; j++)
        {
            if (j == 0 && i == 0)
            {
                continue;
            }
            vec2 texCoord = v_TexCoord + vec2(i * v_OneTexel.x, j * v_OneTexel.y);
            float depth = texture(_depth, texCoord).r;
            if (centerDepth < depth && depth - centerDepth > thresh)
            {
                foundDepth = true;
                break;
            }
        }
        if (foundDepth) 
        {
            break;
        }
    }
    
    if (foundDepth)
    {
        v_Color = vec4(1.0, 1.0, 1.0, 1.0);
        return;
    }
    
    v_Color = texture(_color, v_TexCoord);
    
}