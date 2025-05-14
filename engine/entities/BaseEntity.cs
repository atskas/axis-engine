
namespace UntitledEngine.engine.entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            OnLoad();
        }

        public void OnLoad()
        {
            Console.WriteLine("BaseEntity spawned");
        }

        public void Think(float deltaTime)
        {
            Console.WriteLine("BaseEntity Thinking..");
        }
    }
}
