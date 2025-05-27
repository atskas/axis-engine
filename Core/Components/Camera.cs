using OpenTK.Mathematics;
using UntitledEngine.Core.Entities;

namespace UntitledEngine.Core.Components;

// Camera, for rendering a scene.
internal class Camera : Component
{
    internal Matrix4 GetViewMatrix()
    {
        if (Entity?.Transform == null) // Cool check, if the object transform is null, gives identity matrix instead of crashing.
            return Matrix4.Identity;

        var transform = Entity.Transform;

        // View matrix is inverse of camera transform, in 2D atleast

        Matrix4 translation = Matrix4.CreateTranslation(-transform.Position.X, -transform.Position.Y, 0f);
        Matrix4 rotation = Matrix4.CreateRotationZ(-transform.Rotation);
        Matrix4 scale = Matrix4.CreateScale(1f / transform.Scale.X, 1f / transform.Scale.Y, 1f);

        return scale * rotation * translation; // Inverse order
    }
}
