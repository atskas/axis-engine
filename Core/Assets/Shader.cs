using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace UntitledEngine.Core;

public class Shader
{
    private int vertexShader, fragmentShader, shaderProgram;

    public Shader(string vertex, string fragment)
    {
        // Create and compile the vertex shader
        vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertex);
        GL.CompileShader(vertexShader);
        CheckShaderCompile(vertexShader);

        // Create and compile the fragment shader
        fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragment);
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
        GL.PolygonMode(TriangleFace.FrontAndBack, mode);

    }

    public void SetColor(Vector4 color)
    {
        int colorLocation = GL.GetUniformLocation(shaderProgram, "shapeColor");
        if (colorLocation == -1)
        {
            Console.WriteLine("Warning: 'shapeColor' uniform not found in shader.");
            return;
        }
        GL.Uniform4(colorLocation, color);
    }

    public void SetTexture(string name, int unit)
    {
        int location = GL.GetUniformLocation(shaderProgram, name);
        if (location == -1)
        {
            Console.WriteLine($"Warning: uniform {name} not found in shader.");
        }
        GL.Uniform1(location, unit);
    }

    public void SetMatrix4(string name, Matrix4 matrix)
    {
        int location = GL.GetUniformLocation(shaderProgram, name);
        if (location == -1)
        {
            Console.WriteLine($"Warning: uniform {name} not found in shader.");
            return;
        }
        GL.UniformMatrix4(location, false, ref matrix);
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
            throw new Exception($"Shader compilation failed: {infoLog}");
        }
    }

    private void CheckProgramLink(int program)
    {
        string infoLog = GL.GetProgramInfoLog(program);
        if (!string.IsNullOrEmpty(infoLog))
        {
            throw new Exception($"Program linking error: {infoLog}");
        }
    }
}
