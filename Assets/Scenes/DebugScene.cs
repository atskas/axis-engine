using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using UntitledEngine.Core.Assets;
using UntitledEngine.Core.Components;
using UntitledEngine.Core.ECS;
using UntitledEngine.Core.Entities;
using UntitledEngine.Core.Physics;
using UntitledEngine.Core.Scenes;

namespace UntitledEngine.Assets.Scenes;

internal class DebugScene : Scene
{
    // object 1
    private Entity debugObject1 = new Entity();
    private MeshRenderer meshRenderer;
    private Rigidbody rigidBody1;
    private Collider collider1;
    
    // object 2
    private Entity debugFloor = new Entity();
    private MeshRenderer debugFloorMeshRenderer;
    private Collider collider2;
    private Rigidbody rigidBody2;

    private float time = 0f;
    
    public DebugScene()
    {
        // Camera
        Entity cameraObject = new CameraEntity();
        
        // Create components and add them
        meshRenderer = new MeshRenderer(new Texture("Assets/Textures/texture.jpg"));
        rigidBody1 = new Rigidbody();
        collider1 = new Collider();
        debugObject1.AddComponent(collider1);
        debugObject1.AddComponent(meshRenderer);
        debugObject1.AddComponent(rigidBody1);
        
        debugFloorMeshRenderer = new MeshRenderer(new Texture("Assets/Textures/floor.jpg"));
        collider2 = new Collider();
        rigidBody2 = new Rigidbody();
        debugFloor.AddComponent(debugFloorMeshRenderer);
        debugFloor.AddComponent(collider2);
        
        // --- Set properties ---
        
        // object 1
        rigidBody1.Mass = 1f;
        collider1.Size = debugObject1.Transform.Scale;
        
        // object 2
        rigidBody2.Mass = 0f;
        debugFloor.Transform.Position = new Vector2(0, -0.8f);
        debugFloor.Transform.Scale = new Vector2(1f, 0.25f);
        collider2.Size = debugFloor.Transform.Scale;

        // Add entities to the scene
        Entities.Add(cameraObject);
        Entities.Add(debugObject1);
        Entities.Add(debugFloor);
        
        // Set object transforms
        debugObject1.Transform.Scale = new Vector2(0.5f, 0.5f);

        // Set this scene as the current active scene
        if (SceneManager.Instance == null)
            throw new InvalidOperationException("SceneManager instance is not initialized.");

        SceneManager.Instance.CurrentScene = this;
    }
}