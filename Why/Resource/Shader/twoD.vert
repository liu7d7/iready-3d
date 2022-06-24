#version 330 core

layout (location = 0) in vec2 texCoord;

uniform vec2 _screenSize;

out vec2 v_TexCoord;
out vec2 v_OneTexel;

void main()
{
    v_TexCoord = (texCoord + 1.0) * 0.5;
    v_OneTexel = 1.0 / _screenSize;

    gl_Position = vec4(texCoord, 0.0, 1.0);
}