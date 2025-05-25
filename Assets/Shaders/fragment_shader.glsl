#version 460 core

in vec2 TexCoord;
out vec4 FragColor;

uniform sampler2D uTexture;
uniform vec4 shapeColor;

void main()
{
    vec4 texColor = texture(uTexture, TexCoord);
    FragColor = texColor * shapeColor; // multiply texture by color tint
}
