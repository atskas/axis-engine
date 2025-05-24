using OpenTK.Mathematics;

namespace UntitledEngine.engine
{
    public struct Transform
    {
        // Transform properties
        public Vector2 Position;
        public float Rotation; // Radians
        public Vector2 Scale;

        public Transform(Vector2 position, float rotation, Vector2 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        // Compute model matrix combining position, rotation and scale
        public Matrix4 GetTransformMatrix()
        {
            return Matrix4.CreateScale(new Vector3(Scale.X, Scale.Y, 1.0f)) *
                   Matrix4.CreateRotationZ(Rotation) *
                   Matrix4.CreateTranslation(new Vector3(Position.X, Position.Y, 0.0f));
        }
    }
}
