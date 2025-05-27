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
        vertexShader = Engine.Instance.gl.CreateShader(ShaderType.VertexShader);
        Engine.Instance.gl.ShaderSource(vertexShader, vertex);
        Engine.Instance.gl.CompileShader(vertexShader);
        CheckShaderCompile(vertexShader);

        // Create and compile the fragment shader
        fragmentShader = Engine.Instance.gl.CreateShader(ShaderType.FragmentShader);
        Engine.Instance.gl.ShaderSource(fragmentShader, fragment);
        Engine.Instance.gl.CompileShader(fragmentShader);
        CheckShaderCompile(fragmentShader);

        // Create the shader program and link shaders
        shaderProgram = Engine.Instance.gl.CreateProgram();
        Engine.Instance.gl.AttachShader(shaderProgram, vertexShader);
        Engine.Instance.gl.AttachShader(shaderProgram, fragmentShader);
        Engine.Instance.gl.LinkProgram(shaderProgram);
        CheckProgramLink(shaderProgram);

        Engine.Instance.gl.DeleteShader(vertexShader);
        Engine.Instance.gl.DeleteShader(fragmentShader);
    }

    public void Use()
    {
        Engine.Instance.gl.UseProgram(shaderProgram);
        // SwitchPolygonMode(PolygonMode.Line);

    }

    // Just for testing
    public void SwitchPolygonMode(PolygonMode mode)
    {
        Engine.Instance.gl.PolygonMode(TriangleFace.FrontAndBack, mode);

    }

    public void SetColor(Vector4 color)
    {
        int colorLocation = Engine.Instance.gl.GetUniformLocation(shaderProgram, "shapeColor");
        if (colorLocation == -1)
        {
            Console.WriteLine("Warning: 'shapeColor' uniform not found in shader.");
            return;
        }
        Engine.Instance.gl.Uniform4(colorLocation, color);
    }

    public void SetTexture(string name, int unit)
    {
        int location = Engine.Instance.gl.GetUniformLocation(shaderProgram, name);
        if (location == -1)
        {
            Console.WriteLine($"Warning: uniform {name} not found in shader.");
        }
        Engine.Instance.gl.Uniform1(location, unit);
    }

    public unsafe void SetMatrix4(string name, Matrix4x4 matrix)
    {
        int location = Engine.Instance.gl.GetUniformLocation(shaderProgram, name);
        if (location == -1)
        {
            Console.WriteLine($"Warning: uniform {name} not found in shader.");
            return;
        }
        
        ReadOnlySpan<float> matrixSpan = MemoryMarshal.Cast<Matrix4x4, float>(MemoryMarshal.CreateReadOnlySpan(ref matrix, 1));
        fixed (float* ptr = matrixSpan)
        {
            Engine.Instance.gl.UniformMatrix4(location, 1, false, ptr);
        }
    }

    public void Cleanup()
    {
        Engine.Instance.gl.DeleteProgram(shaderProgram);
    }

    private void CheckShaderCompile(uint shader)
    {
        int status;
        Engine.Instance.gl.GetShader(shader, GLEnum.CompileStatus, out status);
        if (status == 0)
        {
            string infoLog = Engine.Instance.gl.GetShaderInfoLog(shader);
            throw new Exception($"Shader compilation failed: {infoLog}");
        }
    }

    private void CheckProgramLink(uint program)
    {
        int status;
        Engine.Instance.gl.GetProgram(program, GLEnum.LinkStatus, out status);
        if (status == 0)
        {
            string infoLog = Engine.Instance.gl.GetProgramInfoLog(program);
            throw new Exception($"Program linking error: {infoLog}");
        }
    }
}
