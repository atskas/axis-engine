using OpenTK.Mathematics;

namespace UntitledEngine.engine.entities
{
    public class BaseEntity
    {
        // Transform properties
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Rotation { get; set; } = 0f;
        public Vector2 Size { get; set; } = Vector2.One;

        public BaseEntity()
        {
            OnLoad();
        }

        // Runs when a new entity instance is created
        public void OnLoad()
        {

        }

        // Runs every frame
        public void Think(float deltaTime)
        {

        }

        // Compute model matrix combining position, rotation and scale
        public Matrix4 GetModelMatrix()
        {
            return
                Matrix4.CreateTranslation(new Vector3(-0.5f * Size.X, -0.5f * Size.Y, 0f)) * // center origin
                Matrix4.CreateScale(new Vector3(Size.X, Size.Y, 1f)) *
                Matrix4.CreateRotationZ(Rotation) *
                Matrix4.CreateTranslation(new Vector3(Position.X, Position.Y, 0f));
        }

        // Stuff like the transform properties you won't have to
        // redefine in any entity you create, you'll just have to
        // give them values.
        // For example:
        //  public YourEntity()
        //  {
        //      Position = new Vector2(0, 0);
        //      Size = new Vector2
        //      Rotation = 0f;
        //  }
    }
}
