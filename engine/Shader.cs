using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace UntitledEngine
{
    public class Shader
    {
        private int VBO, VAO;
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

        private float[] vertices = {
            0.5f,  0.5f, 0.0f, // Top right
            0.5f, -0.5f, 0.0f, // Bottom right
            -0.5f, -0.5f, 0.0f, // Bottom left
            -0.5f, 0.5f, 0.0f // Top left
        };

        private int[] indices = // Starts from 0
        {
            0, 1, 3, // First triangle
            1, 2, 3 // second triangle
        };
        
        public Shader()
        {
            // Generate and bind VAO
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            // Generate and bind VBO
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Generate and bind EBO
            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

            // Set up vertex attribute pointers
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

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

            Console.WriteLine($"Generated VBO ID: {VBO}");
        }

        public void Use()
        {
            GL.UseProgram(shaderProgram);
            // SwitchPolygonMode(PolygonMode.Line);

        }

        // Temporarily here, might move
        public void Draw()
        {

            GL.BindVertexArray(VAO);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);

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

        public void Cleanup()
        {
            Console.WriteLine("Cleaning up..");
            GL.BindVertexArray(0);
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(EBO);
            GL.DeleteVertexArray(VAO);
            GL.DeleteProgram(shaderProgram);
        }
    }
}
