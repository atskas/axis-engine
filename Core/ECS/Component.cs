namespace UntitledEngine.Core.ECS;

public abstract class Component
{

    // The parent Entity
    public Entity? Entity { get; internal set; }

    // Runs once on component start
    public virtual void Start() { }

    // Runs each frame
    public virtual void Update() { }
}
