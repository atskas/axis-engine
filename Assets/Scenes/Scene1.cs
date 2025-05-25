using UntitledEngine.Core.Assets;
using UntitledEngine.Core.Components;
using UntitledEngine.Core.ECS;
using UntitledEngine.Core.Entities;
using UntitledEngine.Core.Scenes;

namespace UntitledEngine.Assets.Scenes;

internal class Scene1
{
    private readonly Scene scene;

    public Scene1()
    {
        // Create new Scene
        scene = new Scene();

        // Create entities and components
        Entity debugObject = new Entity();
        MeshRenderer meshRenderer = new MeshRenderer(new Texture("Assets/Textures/texture.png"));
        debugObject.AddComponent(meshRenderer);

        Entity cameraObject = new CameraObject();

        // Add entities to the scene
        scene.Entities.Add(cameraObject);
        scene.Entities.Add(debugObject);

        // Set this scene as the current active scene
        if (SceneManager.Instance == null)
            throw new InvalidOperationException("SceneManager instance is not initialized.");

        SceneManager.Instance.CurrentScene = scene;
    }
}