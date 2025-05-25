using UntitledEngine.Common.Components;
using UntitledEngine.Common.Entities;
using UntitledEngine.Common.Assets;

public class Tony : GameObject
{
    public Tony()
    {
        var texture = new Texture("Textures/texture.png");
        var renderer = new MeshRenderer(texture);

        AddComponent(renderer);
    }
}
