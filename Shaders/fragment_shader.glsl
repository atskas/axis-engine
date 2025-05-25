#version 460 core

in vec4 TexCoord;
out vec4 FragColor;

uniform sampler2D spriteTexture;

void main()
{
    FragColor = texture(spriteTexture, TexCoord);
}
