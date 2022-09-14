#version 330 core

in vec2 v_TexCoords;
in vec2 v_Pos;

uniform sampler2D _tex0;
uniform vec2 _pixSize;
uniform vec2 _screenSize;

out vec4 color;

void main() {
    float sx = v_Pos.x - (int(v_Pos.x) % int(_pixSize.x));
    float sy = v_Pos.y - (int(v_Pos.y) % int(_pixSize.y));
    vec3 col = vec3(0.0);
    for (float i = sx; i <= sx + _pixSize.x; i++) {
        for (float j = sy; j <= sy + _pixSize.y; j++) {
            col = col + texture(_tex0, vec2(i, j) / _screenSize).rgb;
        }
    }
    col = col / (_pixSize.x * _pixSize.y);
    col.g *= 0.833;
    col.r *= 0.925;
    col.b *= 0.925;
    color = vec4(col, 1);
}