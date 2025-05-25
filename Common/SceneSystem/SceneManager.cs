using UntitledEngine.Common.Assets;
using UntitledEngine.Common.Components;
using UntitledEngine.Common.ECS;
using UntitledEngine.Common.Entities;

namespace UntitledEngine.Common.Scenes;

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
        GameObject debugObject = new GameObject(); // Debug object
        MeshRenderer meshRenderer = new MeshRenderer(new Texture("Textures/texture.png")); // Assign debug texture to new mesh renderer
        
        debugObject.AddComponent(meshRenderer);

        GameObject cameraObject = new CameraObject();
        
        debugScene.GameObjects.Add(cameraObject);
        debugScene.GameObjects.Add(debugObject);
        
        CurrentScene = debugScene;
    }

    public void OnUpdate(float deltaTime) => CurrentScene?.UpdateScene();
}