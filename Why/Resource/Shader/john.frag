#version 330 core

out vec4 color;

uniform sampler2D _tex0;

in vec4 v_Color;
in vec2 v_TexCoords;

void main() {
    color = texture(_tex0, v_TexCoords) * v_Color;
}
