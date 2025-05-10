using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace UntitledEngine
{
    public class Shader
    {
        private int VBO;
        private int VAO;
        private int EBO;
        private int vertexShader, fragmentShader, shaderProgram;

        private string vertexShaderSrc = @"
            #version 460 core
            layout (location = 0) in vec3 aPos;
            void main()
            {
                gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);
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
            // Create and compile the vertex shader
            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSrc);
            GL.CompileShader(vertexShader);
            CheckShaderCompile(vertexShader);

            // Create and compile the fragment shader
            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSrc);
            GL.CompileShader(fragmentShader);
            CheckShaderCompile(fragmentShader);

            // Create the shader program and link shaders
            shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);
            CheckProgramLink(shaderProgram);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

        }

        public void Use()
        {
            GL.UseProgram(shaderProgram);
            // SwitchPolygonMode(PolygonMode.Line);

        }

        // Just for testing
        public void SwitchPolygonMode(PolygonMode mode)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, mode);

        }

        public void SetShapeColor(float r, float g, float b, float a)
        {
            int colorLocation = GL.GetUniformLocation(shaderProgram, "shapeColor");
            if (colorLocation == -1)
            {
                Console.WriteLine("Warning: 'shapeColor' uniform not found in shader.");
                return;
            }
            GL.Uniform4(colorLocation, r, g, b, a);
        }

        public void Cleanup()
        {
            GL.DeleteProgram(shaderProgram);
        }

        private void CheckShaderCompile(int shader)
        {
            string infoLog = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrEmpty(infoLog))
            {
                Console.WriteLine($"Shader compilation failed: {infoLog}");
            }
            else
            {
                Console.WriteLine("Shader compiled successfully!");
            }
        }

        private void CheckProgramLink(int program)
        {
            string infoLog = GL.GetProgramInfoLog(program);
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
