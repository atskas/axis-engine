using OpenTK.Mathematics;

namespace UntitledEngine.engine.entities
{
    public class BaseEntity
    {
        public Transform2D Transform = new Transform2D();

        public BaseEntity()
        {
            OnLoad();
        }

        // Runs when a new entity instance is created
        public virtual void OnLoad() { }

        // Runs every frame
        public virtual void Think(float deltaTime) { }

        public virtual void Render(Shader shader)
        {
            shader.Use();

            shader.SetMatrix4("projection", Engine.Projection);

            Matrix4 model = Matrix4.CreateScale(new Vector3(Transform.Scale.X, Transform.Scale.Y, 1f)) *
                            Matrix4.CreateRotationZ(Transform.Rotation) *
                            Matrix4.CreateTranslation(new Vector3(Transform.Position.X, Transform.Position.Y, 0f));

            shader.SetMatrix4("model", model);

            Draw(shader);
        }

        protected virtual void Draw(Shader shader) { }

        public virtual void Cleanup() { }
    }
}
