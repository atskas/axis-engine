using System.Collections.ObjectModel;
using UntitledEngine.Core.Components;
using UntitledEngine.Core;

namespace UntitledEngine.Core.Entities;

public class Entity
{
    public string Name { get; set; } = string.Empty;

    private readonly List<Component> components = new();

    public ReadOnlyCollection<Component> Components => components.AsReadOnly();

    public Transform? Transform => components.OfType<Transform>().FirstOrDefault();

    // Constructor to initialise the GameObject
    public Entity()
    {
        AddComponent(new Transform());
    }

    // Add component to object
    public void AddComponent(Component component)
    {
        components.Add(component);
        component.GameObject = this;
    }

    // Get component from object
    public T GetComponent<T>() where T : Component
    {
        return components.OfType<T>().FirstOrDefault();
    }
}
