using OpenTK.Mathematics;
using UntitledEngine.Common.Entities;

namespace UntitledEngine.Common.Components;

public class Transform : GameComponent
{
    // Object transforms
    public Vector2 Position { get; set; } = Vector2.Zero;
    public float Rotation { get; set; } = 0;
    public Vector2 Scale {  get; set; } = Vector2.One;


    // Compute model matrix combining transforms
    public Matrix4 GetTransformMatrix()
    {
        return Matrix4.CreateScale(new Vector3(Scale.X, Scale.Y, 0f)) *
               Matrix4.CreateRotationZ(Rotation) *
               Matrix4.CreateTranslation(new Vector3(Position.X, Position.Y, 0f));
    }
}
