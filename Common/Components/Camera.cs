using OpenTK.Mathematics;
using UntitledEngine.Common.Entities;

namespace UntitledEngine.Common.Components;

// Camera, for rendering a scene.
internal class Camera : GameComponent
{
    internal Matrix4 GetViewMatrix()
    {
        if (GameObject?.Transform == null) // Cool check, if the object transform is null, gives identity matrix instead of crashing.
            return Matrix4.Identity;

        var transform = GameObject.Transform;

        // View matrix is inverse of camera transform, in 2D atleast

        Matrix4 translation = Matrix4.CreateTranslation(-transform.Position.X, -transform.Position.Y, 0f);
        Matrix4 rotation = Matrix4.CreateRotationZ(-transform.Rotation);
        Matrix4 scale = Matrix4.CreateScale(1f / transform.Scale.X, 1f / transform.Scale.Y, 1f);

        return scale * rotation * translation; // Inverse order
    }
}
