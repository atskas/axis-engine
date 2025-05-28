using System.Numerics;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;
using Silk.NET.Maths;

namespace UntitledEngine.Core;

public class Shader
{
    private uint vertexShader, fragmentShader, shaderProgram;

    public Shader(string vertex, string fragment)
    {
        // Create and compile the vertex shader
        vertexShader = Engine.Instance.Gl.CreateShader(ShaderType.VertexShader);
        Engine.Instance.Gl.ShaderSource(vertexShader, vertex);
        Engine.Instance.Gl.CompileShader(vertexShader);
        CheckShaderCompile(vertexShader);

        // Create and compile the fragment shader
        fragmentShader = Engine.Instance.Gl.CreateShader(ShaderType.FragmentShader);
        Engine.Instance.Gl.ShaderSource(fragmentShader, fragment);
        Engine.Instance.Gl.CompileShader(fragmentShader);
        CheckShaderCompile(fragmentShader);

        // Create the shader program and link shaders
        shaderProgram = Engine.Instance.Gl.CreateProgram();
        Engine.Instance.Gl.AttachShader(shaderProgram, vertexShader);
        Engine.Instance.Gl.AttachShader(shaderProgram, fragmentShader);
        Engine.Instance.Gl.LinkProgram(shaderProgram);
        CheckProgramLink(shaderProgram);

        Engine.Instance.Gl.DeleteShader(vertexShader);
        Engine.Instance.Gl.DeleteShader(fragmentShader);
    }

    public void Use()
    {
        Engine.Instance.Gl.UseProgram(shaderProgram);
        // SwitchPolygonMode(PolygonMode.Line);

    }

    // Just for testing
    public void SwitchPolygonMode(PolygonMode mode)
    {
        Engine.Instance.Gl.PolygonMode(TriangleFace.FrontAndBack, mode);

    }

    public void SetColor(Vector4 color)
    {
        int colorLocation = Engine.Instance.Gl.GetUniformLocation(shaderProgram, "shapeColor");
        if (colorLocation == -1)
        {
            Console.WriteLine("Warning: 'shapeColor' uniform not found in shader.");
            return;
        }
        Engine.Instance.Gl.Uniform4(colorLocation, color);
    }

    public void SetTexture(string name, int unit)
    {
        int location = Engine.Instance.Gl.GetUniformLocation(shaderProgram, name);
        if (location == -1)
        {
            Console.WriteLine($"Warning: uniform {name} not found in shader.");
        }
        Engine.Instance.Gl.Uniform1(location, unit);
    }

    // public void SetUniform<T>(string name, T value) => Engine.Instance.gl.Uniform1(Engine.Instance.gl.GetUniformLocation(shaderProgram, name));

    public unsafe void SetMatrix4(string name, Matrix4x4 matrix)
    {
        int location = Engine.Instance.Gl.GetUniformLocation(shaderProgram, name);
        if (location == -1)
        {
            Console.WriteLine($"Warning: uniform {name} not found in shader.");
            return;
        }
        
        ReadOnlySpan<float> matrixSpan = MemoryMarshal.Cast<Matrix4x4, float>(MemoryMarshal.CreateReadOnlySpan(ref matrix, 1));
        fixed (float* ptr = matrixSpan)
        {
            Engine.Instance.Gl.UniformMatrix4(location, 1, false, ptr);
        }
    }

    public void Cleanup()
    {
        Engine.Instance.Gl.DeleteProgram(shaderProgram);
    }

    private void CheckShaderCompile(uint shader)
    {
        int status;
        Engine.Instance.Gl.GetShader(shader, GLEnum.CompileStatus, out status);
        if (status == 0)
        {
            string infoLog = Engine.Instance.Gl.GetShaderInfoLog(shader);
            throw new Exception($"Shader compilation failed: {infoLog}");
        }
    }

    private void CheckProgramLink(uint program)
    {
        int status;
        Engine.Instance.Gl.GetProgram(program, GLEnum.LinkStatus, out status);
        if (status == 0)
        {
            string infoLog = Engine.Instance.Gl.GetProgramInfoLog(program);
            throw new Exception($"Program linking error: {infoLog}");
        }
    }
}
