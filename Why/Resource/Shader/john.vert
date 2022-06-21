#version 330 core

layout(location = 0) in vec3 pos;
layout(location = 1) in vec4 color;

uniform mat4 _proj;

out vec4 v_Color;

void main() {
    vec4 projected = _proj * vec4(pos, 1.0);
    gl_Position = projected;
    v_Color = color;
}
