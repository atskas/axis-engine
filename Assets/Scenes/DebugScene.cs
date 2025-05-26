using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using UntitledEngine.Core.Assets;
using UntitledEngine.Core.Components;
using UntitledEngine.Core.ECS;
using UntitledEngine.Core.Entities;
using UntitledEngine.Core.Scenes;

namespace UntitledEngine.Assets.Scenes;

internal class DebugScene : Scene
{
    private Entity debugObject = new Entity();
    private MeshRenderer meshRenderer;

    private float time = 0f;
    
    public DebugScene()
    {
        // Create entities and components
        meshRenderer = new MeshRenderer(new Texture("Assets/Textures/texture.jpg"));
        debugObject.AddComponent(meshRenderer);

        Entity cameraObject = new CameraEntity();

        // Add entities to the scene
        Entities.Add(cameraObject);
        Entities.Add(debugObject);
        
        // Set object transforms
        debugObject.Transform.Scale = new Vector2(0.5f, 0.5f);

        // Set this scene as the current active scene
        if (SceneManager.Instance == null)
            throw new InvalidOperationException("SceneManager instance is not initialized.");

        SceneManager.Instance.CurrentScene = this;
    }

    public override void UpdateScene()
    {
        // Rgb Effect
        time += Program.Engine.deltaTime;

        float r = (float)(Math.Sin(time) * 0.5 + 0.5);
        float g = (float)(Math.Sin(time + 2) * 0.5 + 0.5);
        float b = (float)(Math.Sin(time + 4) * 0.5 + 0.5);
        meshRenderer.Color = new Vector4(r, g, b, 1f);
        
        // Input
        KeyboardState keyboardState = Program.Engine.KeyboardState;

        if (keyboardState.IsKeyDown(Keys.D))
            debugObject.Transform.Rotation += Program.Engine.deltaTime * -1.65f;
        
        if (keyboardState.IsKeyDown(Keys.A))
            debugObject.Transform.Rotation += Program.Engine.deltaTime * 1.65f;
    }
}