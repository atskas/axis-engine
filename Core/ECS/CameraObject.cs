using UntitledEngine.Core.Components;
using UntitledEngine.Core.Entities;

namespace UntitledEngine.Core.ECS;

public class CameraObject : Entity
{
    public CameraObject()
    {
        // Add Camera component to the GameObject
        var camera = new Camera();
        AddComponent(camera);
    }
}
