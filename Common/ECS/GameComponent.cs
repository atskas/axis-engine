namespace UntitledEngine.Common.Entities;

public abstract class GameComponent
{

    // The parent GameObject
    public GameObject? GameObject { get; internal set; }

    // Runs once on component start
    public virtual void Start() { }

    // Runs each frame
    public virtual void Update() { }
}
