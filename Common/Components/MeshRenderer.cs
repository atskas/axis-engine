using OpenTK.Graphics.OpenGL4;
using UntitledEngine.Common.Assets;
using UntitledEngine.Common.Entities;

namespace UntitledEngine.Common.Components;

internal class MeshRenderer : GameComponent
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

    private int vao, vbo, ebo;

    public MeshRenderer()
    {
        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();
        ebo = GL.GenBuffer();
    }

    private void SetupMesh()
    {
        if (Mesh == null) return;

        GL.BindVertexArray(vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, Mesh.Vertices.Length * sizeof(float), Mesh.Vertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, Mesh.Indices.Length * sizeof(uint), Mesh.Indices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindVertexArray(0);
    }

    public void Draw()
    {
        if (Mesh == null)
            return;

        GL.BindVertexArray(vao);
        GL.DrawElements(PrimitiveType.Triangles, Mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
    }
}
