using System.Numerics;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;
using Silk.NET.Maths;
using UntitledEngine.Core.Assets;
using UntitledEngine.Core.Rendering;

namespace UntitledEngine.Core;

public class Shader
{
    private uint vertexShader, fragmentShader, shaderProgram;

    public Shader(string vertex, string fragment)
    {
        // Create and compile the vertex shader
        vertexShader = Renderer.Gl.CreateShader(ShaderType.VertexShader);
        Renderer.Gl.ShaderSource(vertexShader, vertex);
        Renderer.Gl.CompileShader(vertexShader);
        CheckShaderCompile(vertexShader);

        // Create and compile the fragment shader
        fragmentShader = Renderer.Gl.CreateShader(ShaderType.FragmentShader);
        Renderer.Gl.ShaderSource(fragmentShader, fragment);
        Renderer.Gl.CompileShader(fragmentShader);
        CheckShaderCompile(fragmentShader);

        // Create the shader program and link shaders
        shaderProgram = Renderer.Gl.CreateProgram();
        Renderer.Gl.AttachShader(shaderProgram, vertexShader);
        Renderer.Gl.AttachShader(shaderProgram, fragmentShader);
        Renderer.Gl.LinkProgram(shaderProgram);
        CheckProgramLink(shaderProgram);

        Renderer.Gl.DeleteShader(vertexShader);
        Renderer.Gl.DeleteShader(fragmentShader);
    }

    public void Use()
    {
        Renderer.Gl.UseProgram(shaderProgram);
        // SwitchPolygonMode(PolygonMode.Line);

    }

    public void SetColor(Color color)
    {
        int colorLocation = Renderer.Gl.GetUniformLocation(shaderProgram, "shapeColor");
        if (colorLocation == -1)
        {
            Console.WriteLine("Warning: 'shapeColor' uniform not found in shader.");
            return;
        }
        
        Renderer.Gl.Uniform4(colorLocation, color.R, color.G, color.B, color.A);
    }

    public void SetTexture(string name, int unit)
    {
        int location = Renderer.Gl.GetUniformLocation(shaderProgram, name);
        if (location == -1)
        {
            Console.WriteLine($"Warning: uniform {name} not found in shader.");
        }
        Renderer.Gl.Uniform1(location, unit);
    }

    // public void SetUniform<T>(string name, T value) => Renderer.Gl.Uniform1(Engine.Instance.gl.GetUniformLocation(shaderProgram, name)); -- Remember to do this

    public unsafe void SetMatrix4(string name, Matrix4x4 matrix)
    {
        int location = Renderer.Gl.GetUniformLocation(shaderProgram, name);
        if (location == -1)
        {
            Console.WriteLine($"Warning: uniform {name} not found in shader.");
            return;
        }
        
        ReadOnlySpan<float> matrixSpan = MemoryMarshal.Cast<Matrix4x4, float>(MemoryMarshal.CreateReadOnlySpan(ref matrix, 1));
        fixed (float* ptr = matrixSpan)
        {
            Renderer.Gl.UniformMatrix4(location, 1, false, ptr);
        }
    }

    private void CheckShaderCompile(uint shader)
    {
        int status;
        Renderer.Gl.GetShader(shader, GLEnum.CompileStatus, out status);
        if (status == 0)
        {
            string infoLog = Renderer.Gl.GetShaderInfoLog(shader);
            throw new Exception($"Shader compilation failed: {infoLog}");
        }
    }

    private void CheckProgramLink(uint program)
    {
        int status;
        Renderer.Gl.GetProgram(program, GLEnum.LinkStatus, out status);
        if (status == 0)
        {
            string infoLog = Renderer.Gl.GetProgramInfoLog(program);
            throw new Exception($"Program linking error: {infoLog}");
        }
    }
}
