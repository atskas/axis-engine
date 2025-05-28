using System.Numerics;
using UntitledEngine.Core.ECS;

namespace UntitledEngine.Core.Components;

// Camera, for rendering a scene.
internal class Camera : Component
{
    internal Matrix4x4 GetViewMatrix()
    {
        if (Entity?.Transform == null) // Cool check, if the object transform is null, gives identity matrix instead of crashing.
            return Matrix4x4.Identity;

        var transform = Entity.Transform;

        // View matrix is inverse of camera transform, in 2D atleast

        Matrix4x4 translation = Matrix4x4.CreateTranslation(-transform.Position.X, -transform.Position.Y, 0f);
        Matrix4x4 rotation = Matrix4x4.CreateRotationZ(-transform.Rotation);
        Matrix4x4 scale = Matrix4x4.CreateScale(1f / transform.Scale.X, 1f / transform.Scale.Y, 1f);

        return scale * rotation * translation; // Inverse order
    }
}
