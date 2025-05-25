using UntitledEngine.Core.Components;
using UntitledEngine.Core.Entities;

namespace UntitledEngine.Core.ECS;

public class CameraEntity : Entity
{
    public CameraEntity()
    {
        // Add Camera component to the GameObject
        var camera = new Camera();
        AddComponent(camera);
    }
}
