using OpenTK.Mathematics;
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

    private float time = 0f;
    
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
        time += 0.01f;
        
        float r = (float)(Math.Sin(time) * 0.5 + 0.5);
        float g = (float)(Math.Sin(time + 2) * 0.5 + 0.5);
        float b = (float)(Math.Sin(time + 4) * 0.5 + 0.5);
        
        debugObject.Transform.Rotation += 0.0005f;
        meshRenderer.Color = new Vector4(r, g, b, 1f);
    }
}