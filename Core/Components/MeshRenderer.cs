using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using UntitledEngine.Core.Assets;
using UntitledEngine.Core.Entities;

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

    private int vao, vbo, ebo;

    public MeshRenderer(Texture texture)
    {
        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();
        ebo = GL.GenBuffer();

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

    private void SetupMesh()
    {
        if (Mesh == null) return;

        GL.BindVertexArray(vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, Mesh.Vertices.Length * sizeof(float), Mesh.Vertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, Mesh.Indices.Length * sizeof(uint), Mesh.Indices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, Mesh.VertexStride, 0);
        GL.EnableVertexAttribArray(0);

        // VertexAttribPointer layout
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, Mesh.VertexStride, 2 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.BindVertexArray(0);
    }

    public void Draw()
    {
        if (Mesh == null)
            return;

        Program.Engine.Shader.Use();
        Program.Engine.Shader.SetTexture("uTexture", 0);

        if (Texture != null)
        {
            Texture.Bind(TextureUnit.Texture0);
        }

        Program.Engine.Shader.SetColor(Color);

        GL.BindVertexArray(vao);
        GL.DrawElements(PrimitiveType.Triangles, Mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
    }
}
