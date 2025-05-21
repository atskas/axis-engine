using OpenTK.Mathematics;

namespace UntitledEngine.engine.entities
{
    public class BaseEntity
    {
        public Transform2D Transform = new Transform2D(); // Position, Scale, Rotation

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
