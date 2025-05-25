using UntitledEngine.Common.Components;
using UntitledEngine.Common.Entities;

namespace UntitledEngine.Common.ECS;

public class CameraObject : GameObject
{
    public CameraObject()
    {
        // Add Camera component to the GameObject
        var camera = new Camera();
        AddComponent(camera);
    }
}
