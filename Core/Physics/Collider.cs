using OpenTK.Mathematics;
using UntitledEngine.Core.Entities;

namespace UntitledEngine.Core.Physics;

public class Collider : Component
{
    public Vector2 Offset = Vector2.Zero;
    public Vector2 Size = Vector2.One;
    public bool IsTrigger = false;
}
