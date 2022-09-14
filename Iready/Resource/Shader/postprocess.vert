#version 330 core

in vec2 pos;

uniform vec2 _screenSize;

out vec2 v_TexCoords;
out vec2 v_Pos;

void main() {
    gl_Position = vec4((pos / _screenSize) * 2 - 1, 0, 1);
    v_TexCoords = pos / _screenSize;
    v_Pos = pos;
}