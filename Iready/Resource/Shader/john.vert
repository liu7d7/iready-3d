#version 330 core

layout(location = 0) in vec3 pos;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec2 texCoords;
layout(location = 3) in vec4 color;

uniform mat4 _proj;
uniform mat4 _lookAt;
uniform int _rendering3d;
uniform int _renderingRed;

out vec4 v_Color;
out vec3 v_Normal;
out vec2 v_TexCoords;
out vec3 v_FragPos;
flat out int v_RenderingRed;

void main() {
    vec4 final = vec4(pos, 1.0) * _lookAt * _proj;
    if (_rendering3d == 1) {
        final += vec4(0.2, -0.095, 0.0, 0.0); 
    }
    gl_Position = final;
    v_TexCoords = texCoords;
    v_Color = color;
    v_Normal = normal;
    v_FragPos = pos;
    v_RenderingRed = _renderingRed;
}
