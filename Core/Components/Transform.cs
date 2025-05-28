using System.Numerics;
using UntitledEngine.Core.ECS;

namespace UntitledEngine.Core.Components;

public class Transform : Component
{
    // Object transforms
    public Vector2 Position { get; set; } = Vector2.Zero;
    public float Rotation { get; set; } = 0;
    public Vector2 Scale {  get; set; } = Vector2.One;


    // Compute model matrix combining transforms
    public Matrix4x4 GetTransformMatrix()
    {
        return Matrix4x4.CreateScale(new Vector3(Scale.X, Scale.Y, 0f)) *
               Matrix4x4.CreateRotationZ(Rotation) *
               Matrix4x4.CreateTranslation(new Vector3(Position.X, Position.Y, 0f));
    }
}
