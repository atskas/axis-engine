using UntitledEngine.Core.Entities;

namespace UntitledEngine.Core.Scenes;

internal class Scene
{
    public HashSet<Entity> GameObjects { get; set; } = new HashSet<Entity>();

    // So that you don't accidentally modify anything in the previous hashset
    // while iterating over it
    private readonly List<Entity> gameObjectBuffer = new();

    public void StartScene()
    {
        gameObjectBuffer.Clear();
        gameObjectBuffer.AddRange(GameObjects);

        for (int i = 0; i < gameObjectBuffer.Count; i++)
        {
            var go = gameObjectBuffer[i];
            
            var components = go.Components;
            // Iterate through all components and run their Start()
            for (int j = 0; j < components.Count; j++)
                components[j].Start();
        }
    }

    public void UpdateScene()
    {
        gameObjectBuffer.Clear();
        gameObjectBuffer.AddRange(GameObjects);
    
        // Iterate through all components and run their Update()
        for (int i = 0; i < gameObjectBuffer.Count; i++)
        {
            var go = gameObjectBuffer[i];
            var components = go.Components;
            
            for (int j = 0; j < components.Count; j++)
                components[j].Update();
        }
    }
}