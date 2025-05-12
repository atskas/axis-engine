using Silk.NET.OpenGL;
using Silk.NET.Maths;
using System;

namespace UntitledEngine
{
    public class Shader
    {
        private uint VAO, VBO, EBO;
        private uint vertexShader, fragmentShader;
        private uint shaderProgram;

        private GL gl;

        public uint ID { get; private set; }

        private string vertexShaderSrc = @"
            #version 460 core
            layout (location = 0) in vec3 aPos;
            uniform mat4 model;
            void main()
            {
                gl_Position = model * vec4(aPos, 1.0);
            }";

        private string fragmentShaderSrc = @"
            #version 460 core
            out vec4 FragColor;
            uniform vec4 shapeColor;
            void main()
            {
                FragColor = shapeColor;
            }";

        public Shader()
        {
            gl = GLContext.Instance ?? throw new InvalidOperationException("GL context is not initialized.");

            // Create and compile the vertex shader
            vertexShader = gl.CreateShader(GLEnum.VertexShader);
            gl.ShaderSource(vertexShader, vertexShaderSrc);
            gl.CompileShader(vertexShader);
            CheckShaderCompile(vertexShader);

            // Create and compile the fragment shader
            fragmentShader = gl.CreateShader(GLEnum.FragmentShader);
            gl.ShaderSource(fragmentShader, fragmentShaderSrc);
            gl.CompileShader(fragmentShader);
            CheckShaderCompile(fragmentShader);

            // Create the shader program and link shaders
            shaderProgram = gl.CreateProgram();
            gl.AttachShader(shaderProgram, vertexShader);
            gl.AttachShader(shaderProgram, fragmentShader);
            gl.LinkProgram(shaderProgram);
            CheckProgramLink(shaderProgram);

            gl.DeleteShader(vertexShader);
            gl.DeleteShader(fragmentShader);

            ID = shaderProgram;
        }

        public void Use()
        {
            gl.UseProgram(shaderProgram);
        }

        public void SetShapeColor(Vector4D<float> color)
        {
            int colorLocation = gl.GetUniformLocation(shaderProgram, "shapeColor");
            if (colorLocation == -1)
            {
                Console.WriteLine("Warning: 'shapeColor' uniform not found in shader.");
                return;
            }

            // Pass the color as an array
            float[] colorArray = { color.X, color.Y, color.Z, color.W };
            gl.Uniform4(colorLocation, colorArray);
        }

        public void Cleanup()
        {
            gl.DeleteProgram(shaderProgram);
        }

        private void CheckShaderCompile(uint shader)
        {
            string infoLog = gl.GetShaderInfoLog(shader);
            if (!string.IsNullOrEmpty(infoLog))
            {
                Console.WriteLine($"Shader compilation failed: {infoLog}");
            }
            else
            {
                Console.WriteLine("Shader compiled successfully!");
            }
        }

        private void CheckProgramLink(uint program)
        {
            string infoLog = gl.GetProgramInfoLog(program);
            if (!string.IsNullOrEmpty(infoLog))
            {
                Console.WriteLine($"Program linking error: {infoLog}");
            }
            else
            {
                Console.WriteLine("Program linked successfully!");
            }
        }
    }
}
