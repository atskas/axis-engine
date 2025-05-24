using OpenTK.Mathematics;

namespace UntitledEngine.engine.entities
{
    public class BaseEntity
    {
        public Transform Transform = new Transform(); // Position, Scale, Rotation

        public BaseEntity()
        {
            OnLoad();
        }

        // Runs when a new entity instance is created
        public virtual void OnLoad() { }

        // Runs every frame
        public virtual void Think(float deltaTime) { }
    }
}
