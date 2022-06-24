#version 330 core

layout(location = 0) in vec3 pos;
layout(location = 1) in vec2 texCoords;
layout(location = 2) in vec4 color;

uniform mat4 _proj;
uniform mat4 _lookAt;
uniform vec2 _tex0Size;

out vec4 v_Color;
out vec2 v_TexCoords;

void main() {
    gl_Position = vec4(pos, 1.0) * _lookAt * _proj;
    v_TexCoords = texCoords / _tex0Size;
    v_Color = color;
}
