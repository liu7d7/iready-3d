#version 330 core

out vec4 color;

uniform sampler2D _tex0;
uniform vec3 lightPos;

in vec4 v_Color;
in vec3 v_Normal;
in vec2 v_TexCoords;
in vec3 v_FragPos;
flat in int v_RenderingRed;

vec3 lerp(vec3 start, vec3 end, float progress) {
    return start + (end - start) * progress;
}

void main() {
    if (v_RenderingRed == 1) {
        color = v_Color * vec4(1.0, 1.0, 1.0, texture(_tex0, v_TexCoords).r);
        return;
    }
    // ambient
    vec3 lightColor = vec3(1.0, 0.85, 1.0);

    float ambientStrength = 0.88;
    vec3 ambient = ambientStrength * lightColor;

    // diffuse
    vec3 norm = normalize(v_Normal);
    vec3 lightDir = normalize(lightPos - v_FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lerp(vec3(1.0), lightColor, diff) * 0.12;

    vec4 result = vec4(ambient + diffuse, 1.0) * texture(_tex0, v_TexCoords).rgba;
    color = vec4(result) * v_Color;
}
