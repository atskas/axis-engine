using System.Collections.ObjectModel;
using UntitledEngine.Core.Components;
using UntitledEngine.Core;
using UntitledEngine.Core.Rendering;

namespace UntitledEngine.Core.ECS;

public class Entity
{
    public string Name { get; set; } = "Entity";

    private readonly List<Component> _components = new();
    private HashSet<string> _tags = new();

    public ReadOnlyCollection<Component> Components => _components.AsReadOnly();

    public Transform? Transform => _components.OfType<Transform>().FirstOrDefault();

    // Constructor to initialise the GameObject
    public Entity() => AddComponent(new Transform());

    public void Destroy()
    {
        // If PhysicsBody found, erase its body before deleting component
        var pb = GetComponent<PhysicsBody>();
        if (pb != null)
            Engine.Instance.PhysicsManager.Box2DWorld.DestroyBody(pb.Body);
        
        var mr = GetComponent<MeshRenderer>();
        if (mr != null)
            Renderer.DestroyMeshRenderer(mr);
            
        
        // Remove all components and tags safely
        // (uses a copy of the lists when removing items)
        foreach (var component in _components.ToList())
            RemoveComponent(component);
        foreach (var tag in _tags.ToList())
            RemoveTag(tag);
        
        Engine.Instance.SceneManager.CurrentScene.Entities.Remove(this);
    }

    // --- COMPONENTS ---
    
    // Add component to entity
    public void AddComponent(Component component)
    {
        _components.Add(component);
        component.Entity = this;
    }

    public void RemoveComponent(Component component)
    {
        _components.Remove(component);
        component.Entity = null;
    }

    // Get component from entity
    public T GetComponent<T>() where T : Component
    {
        return _components.OfType<T>().FirstOrDefault();
    }
    
    // --- TAGS ---
    
    // Add tag to entity
    public void AddTag(string tag) => _tags.Add(tag);

    // Remove tag from entity
    public void RemoveTag(string tag) => _tags.Remove(tag);

    // Check if entity has tag
    public bool HasTag(string tag)
    {
        return _tags.Contains(tag);
    }
    
    // Get tags from entity
    public IEnumerable<string> GetTags()
    {
        return _tags;
    }
}
