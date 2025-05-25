using UntitledEngine.Common.Components;
using UntitledEngine.Common.Entities;
using UntitledEngine.Common.Assets;

public class Object1 : GameObject
{
    public Object1()
    {
        float[] vertices = {
            -0.5f,  0.5f,
            0.5f,  0.5f,
            0.5f, -0.5f,
            -0.5f, -0.5f
        };

        uint[] indices = {
            0, 1, 2,
            2, 3, 0 
        };


        var mesh = new Mesh { Vertices = vertices, Indices = indices };
        var renderer = new MeshRenderer { Mesh = mesh };

        AddComponent(renderer);
    }
}
