using OpenTK.Mathematics;

namespace UntitledEngine.engine.entities
{
    public class Camera : BaseEntity
    {
        public float Zoom { get; set; } = 1f;

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.CreateTranslation(-Transform.Position.X, -Transform.Position.Y, 0);
        }

        public Matrix4 GetProjectionMatrix(float aspectRatio)
        {
            float width = 2f / Zoom;
            float height = width / aspectRatio;
            return Matrix4.CreateOrthographic(width, height, -1, 1);
        }

    }
}
