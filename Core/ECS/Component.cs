namespace UntitledEngine.Core.Entities;

public abstract class Component
{

    // The parent GameObject
    public Entity? GameObject { get; internal set; }

    // Runs once on component start
    public virtual void Start() { }

    // Runs each frame
    public virtual void Update() { }
}
