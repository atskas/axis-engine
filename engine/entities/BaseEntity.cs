
namespace UntitledEngine.engine.entities
{
    public class BaseEntity
    {
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
    }
}
