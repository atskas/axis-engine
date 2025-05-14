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

        public int ID { get; private set; }

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

            ID = shaderProgram;
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

        public void SetShaderColor(Vector4 color)
        {
            int colorLocation = GL.GetUniformLocation(shaderProgram, "shapeColor");
            if (colorLocation == -1)
            {
                Console.WriteLine("Warning: 'shapeColor' uniform not found in shader.");
                return;
            }
            GL.Uniform4(colorLocation, color);
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
        }

        private void CheckProgramLink(int program)
        {
            string infoLog = GL.GetProgramInfoLog(program);
            if (!string.IsNullOrEmpty(infoLog))
            {
                Console.WriteLine($"Program linking error: {infoLog}");
            }
        }
    }
}
