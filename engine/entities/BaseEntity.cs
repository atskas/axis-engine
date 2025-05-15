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
        public void OnLoad()
        {
            Transform.Scale = new Vector2(1.5f, 1.5f);
            Console.WriteLine(Transform.Scale);
        }

        // Runs every frame
        public void Think(float deltaTime)
        {

        }
    }
}
