using System.Collections.ObjectModel;
using UntitledEngine.Core.Components;
using UntitledEngine.Core;

namespace UntitledEngine.Core.Entities;

public class Entity
{
    public string Name { get; set; } = "Entity";

    private readonly List<Component> components = new();
    private HashSet<string> tags = new();

    public ReadOnlyCollection<Component> Components => components.AsReadOnly();

    public Transform? Transform => components.OfType<Transform>().FirstOrDefault();

    // Constructor to initialise the GameObject
    public Entity()
    {
        AddComponent(new Transform());
    }

    // --- COMPONENTS ---
    
    // Add component to entity
    public void AddComponent(Component component)
    {
        components.Add(component);
        component.Entity = this;
    }

    // Get component from entity
    public T GetComponent<T>() where T : Component
    {
        return components.OfType<T>().FirstOrDefault();
    }
    
    // --- TAGS ---
    
    // Add tag to entity
    public void AddTag(string tag)
    {
        tags.Add(tag);
    }
    
    // Remove tag from entity
    public void RemoveTag(string tag)
    {
        tags.Remove(tag);
    }
    
    // Check if entity has tag
    public bool HasTag(string tag)
    {
        return tags.Contains(tag);
    }
    
    // Get tags from entity
    public IEnumerable<string> GetTags()
    {
        return tags;
    }
}
