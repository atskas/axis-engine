using UntitledEngine.Core.Assets;
using UntitledEngine.Core.Components;
using UntitledEngine.Core.ECS;
using UntitledEngine.Core.Entities;
using UntitledEngine.Core.Scenes;

namespace UntitledEngine.Assets.Scenes;

internal class Scene1 : Scene
{
    private Entity debugObject = new Entity();
    private MeshRenderer meshRenderer;
    
    public Scene1()
    {
        // Create entities and components
        meshRenderer = new MeshRenderer(new Texture("Assets/Textures/texture.jpg"));
        debugObject.AddComponent(meshRenderer);

        Entity cameraObject = new CameraObject();

        // Add entities to the scene
        this.Entities.Add(cameraObject);
        this.Entities.Add(debugObject);

        // Set this scene as the current active scene
        if (SceneManager.Instance == null)
            throw new InvalidOperationException("SceneManager instance is not initialized.");

        SceneManager.Instance.CurrentScene = this;
    }

    public override void UpdateScene()
    {
        debugObject.Transform.Rotation += 0.0005f;
    }
}