using System.Numerics;
using Silk.NET.OpenGL;
using UntitledEngine.Core.Assets;
using UntitledEngine.Core.Entities;
using DrawElementsType = Silk.NET.OpenGL.DrawElementsType;
using PrimitiveType = Silk.NET.OpenGL.PrimitiveType;
using Texture = UntitledEngine.Core.Assets.Texture;

namespace UntitledEngine.Core.Components;

internal class MeshRenderer : Component
{
    public Mesh Mesh
    {
        get => mesh;
        set
        {
            mesh = value;
            SetupMesh();
        }
    }
    private Mesh mesh = null!;

    public Texture Texture { get; set; } = null!;
    public Vector4 Color { get; set; } = Vector4.One; // Defaults to white

    private uint vao, vbo, ebo;

    public MeshRenderer(Texture texture)
    {
        vao = Engine.Instance.gl.GenVertexArray();
        vbo = Engine.Instance.gl.GenBuffer();
        ebo = Engine.Instance.gl.GenBuffer();

        Texture = texture;
        DrawQuad(); // Initialize quad mesh
    }

    // Draw initial quad
    public void DrawQuad()
    {
        float[] quadVertices = {
            -0.5f, -0.5f,   0f, 0f,
            0.5f, -0.5f,   1f, 0f,
            0.5f,  0.5f,   1f, 1f,
            -0.5f,  0.5f,   0f, 1f
        };

        uint[] indices = {
            0, 1, 2,
            2, 3, 0
        };

        mesh = new Mesh();
        mesh.Vertices = quadVertices;
        mesh.Indices = indices;

        SetupMesh();
    }

    private unsafe void SetupMesh()
    {
        if (Mesh == null) return;

        var gl = Engine.Instance.gl;

        gl.BindVertexArray(vao);

        gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);

        unsafe
        {
            fixed (float* vertexPtr = mesh.Vertices)
            {
                gl.BufferData(BufferTargetARB.ArrayBuffer,
                    (nuint)(mesh.Vertices.Length * sizeof(float)),
                    vertexPtr,
                    BufferUsageARB.StaticDraw);
            }
        }

        gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);

        unsafe
        {
            fixed (uint* indexPtr = mesh.Indices)
            {
                gl.BufferData(BufferTargetARB.ElementArrayBuffer,
                    (nuint)(mesh.Indices.Length * sizeof(uint)),
                    indexPtr,
                    BufferUsageARB.StaticDraw);
            }
        }

        gl.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, (uint)mesh.VertexStride, (void*)0);
        gl.EnableVertexAttribArray(0);

        gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, (uint)mesh.VertexStride, (void*)(2 * sizeof(float)));
        gl.EnableVertexAttribArray(1);

        gl.BindVertexArray(0);
    }

    public unsafe void Draw()
    {
        if (Mesh == null)
            return;

        Engine.Instance.Shader.Use();
        Engine.Instance.Shader.SetTexture("uTexture", 0);

        Texture?.Bind(TextureUnit.Texture0);

        Engine.Instance.Shader.SetColor(Color);

        var gl = Engine.Instance.gl;
        gl.BindVertexArray(vao);
        gl.DrawElements(PrimitiveType.Triangles, (uint)Mesh.Indices.Length, DrawElementsType.UnsignedInt, (void*)0);
        gl.BindVertexArray(0);
    }

}
