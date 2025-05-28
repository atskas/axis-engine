using System.Numerics;
using Silk.NET.OpenGL;
using UntitledEngine.Core.Assets;
using UntitledEngine.Core.ECS;
using UntitledEngine.Core.Rendering;
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
            Renderer.SetupMeshRenderer(this);
        }
    }

    private Mesh mesh = null!;
    public Texture Texture { get; set; } = null!;
    public Color Color { get; set; } = new Color(1f, 1f, 1f, 1f);

    public MeshRenderer(Texture texture)
    {
        Texture = texture;
        DrawQuad();
    }

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

        mesh = new Mesh { Vertices = quadVertices, Indices = indices };
        Renderer.SetupMeshRenderer(this);
    }
}
