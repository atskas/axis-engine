using UntitledEngine.Assets.Scenes;
using UntitledEngine.Core.Assets;
using UntitledEngine.Core.Components;
using UntitledEngine.Core.ECS;
using UntitledEngine.Core.Entities;

namespace UntitledEngine.Core.Scenes;

public class SceneManager
{
    // Singleton instance of the SceneManager class
    public static SceneManager? Instance { get; private set; }
    public SceneManager() => Instance = this;

    private Scene? currScene;
    public Scene CurrentScene
    {
        get => currScene!;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "CurrentScene cannot be set to null.");

            currScene = value;
            currScene.StartScene();
        }
    }

    public void OnLoad()
    {
        Console.WriteLine("Scene Manager Loaded");
        
        // Load scenes
        DebugScene debugScene = new();
    }

    public void OnUpdate(float deltaTime) => CurrentScene?.UpdateScene();
}