using UntitledEngine.Core.Assets;
using UntitledEngine.Core.Components;
using UntitledEngine.Core.ECS;
using UntitledEngine.Core.Entities;

namespace UntitledEngine.Core.Scenes;

internal class SceneManager
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

    public void SwitchScene(Scene newScene)
    {
        CurrentScene = newScene;
        CurrentScene.StartScene();
    }

    // Just for the debug scene
    public void OnLoad()
    {
        // Debug scene
        Scene debugScene = new Scene();
        Entity debugObject = new Entity(); // Debug object
        MeshRenderer meshRenderer = new MeshRenderer(new Texture("Assets/Textures/texture.png")); // Assign debug texture to new mesh renderer
        
        debugObject.AddComponent(meshRenderer);

        Entity cameraObject = new CameraObject();
        
        debugScene.GameObjects.Add(cameraObject);
        debugScene.GameObjects.Add(debugObject);
        
        CurrentScene = debugScene;
    }

    public void OnUpdate(float deltaTime) => CurrentScene?.UpdateScene();
}