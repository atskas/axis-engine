using UntitledEngine.Core.Entities;

namespace UntitledEngine.Core.Scenes;

public class Scene
{
    public HashSet<Entity> Entities { get; set; } = new HashSet<Entity>();

    // So that you don't accidentally modify anything in the previous hashset
    // while iterating over it
    private readonly List<Entity> entityBuffer = new();

    public void StartScene()
    {
        entityBuffer.Clear();
        entityBuffer.AddRange(Entities);

        for (int i = 0; i < entityBuffer.Count; i++)
        {
            var go = entityBuffer[i];
            
            var components = go.Components;
            // Iterate through all components and run their Start()
            for (int j = 0; j < components.Count; j++)
                components[j].Start();
        }
    }

    public void UpdateScene()
    {
        entityBuffer.Clear();
        entityBuffer.AddRange(Entities);
    
        // Iterate through all components and run their Update()
        for (int i = 0; i < entityBuffer.Count; i++)
        {
            var go = entityBuffer[i];
            var components = go.Components;
            
            for (int j = 0; j < components.Count; j++)
                components[j].Update();
        }
    }
}