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
        // Create entities and add components
        meshRenderer = new MeshRenderer(new Texture("Assets/Textures/texture.jpg"));
        debugObject.AddComponent(meshRenderer);

        Entity cameraObject = new CameraEntity();

        // Add entities to the scene
        Entities.Add(cameraObject);
        Entities.Add(debugObject);

        // Set this scene as the current active scene
        if (SceneManager.Instance == null)
            throw new InvalidOperationException("SceneManager instance is not initialized.");

        SceneManager.Instance.CurrentScene = this;
    }

    public override void UpdateScene()
    {
        time += Program.Engine.deltaTime;

        float r = (float)(Math.Sin(time) * 0.5 + 0.5);
        float g = (float)(Math.Sin(time + 2) * 0.5 + 0.5);
        float b = (float)(Math.Sin(time + 4) * 0.5 + 0.5);
        
        debugObject.Transform.Rotation += Program.Engine.deltaTime * 1.2f;
        meshRenderer.Color = new Vector4(r, g, b, 1f);
    }
}