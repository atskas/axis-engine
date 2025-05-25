using System.Collections.ObjectModel;
using UntitledEngine.Common.Components;
using UntitledEngine.Common;

namespace UntitledEngine.Common.Entities;

public class GameObject
{
    public string Name { get; set; } = string.Empty;

    private readonly List<GameComponent> components = new();

    public ReadOnlyCollection<GameComponent> Components => components.AsReadOnly();

    public Transform? Transform => components.OfType<Transform>().FirstOrDefault();

    // Constructor to initialise the GameObject
    public GameObject()
    {
        AddComponent(new Transform());
    }

    // Add component to object
    public void AddComponent(GameComponent component)
    {
        components.Add(component);
        component.GameObject = this;
    }

    // Get component from object
    public T GetComponent<T>() where T : GameComponent
    {
        return components.OfType<T>().FirstOrDefault();
    }
}
