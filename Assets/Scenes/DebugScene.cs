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
    
    // test objects (many of them)
    private Entity gold1 = new Entity();
    private Entity gold2 = new Entity();
    private Entity gold3 = new Entity();
    
    private MeshRenderer goldMeshRenderer;
    private Rigidbody goldRigidBody;
    private Rigidbody goldRigidBody2;
    private Rigidbody goldRigidBody3;
    private Collider goldCollider;
    private Collider goldCollider2;
    private Collider goldCollider3;

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
        debugFloor.AddComponent(debugFloorMeshRenderer);
        debugFloor.AddComponent(collider2);
        
        // gold pieces
        goldMeshRenderer = new MeshRenderer(new Texture("Assets/Textures/gold.jpg"));
        goldRigidBody = new Rigidbody();
        goldRigidBody2 = new Rigidbody();
        goldRigidBody3 = new Rigidbody();
        
        goldCollider = new Collider();
        goldCollider2 = new Collider();
        goldCollider3 = new Collider();
        
        // add their components
        gold1.AddComponent(goldRigidBody);
        gold2.AddComponent(goldRigidBody2);
        gold3.AddComponent(goldRigidBody3);
        gold1.AddComponent(goldCollider);
        gold2.AddComponent(goldCollider2);
        gold3.AddComponent(goldCollider3);
        gold1.AddComponent(goldMeshRenderer);
        gold2.AddComponent(goldMeshRenderer);
        gold3.AddComponent(goldMeshRenderer);
        
        // --- Set properties ---
        
        // object 1
        debugObject1.Name = "Test";
        rigidBody1.Mass = 1f;
        rigidBody1.Gravity = new Vector2(0f, -30f); // Set gravity to an unrealistic value
        rigidBody1.TerminalVelocity = -3f;
        collider1.Size = new Vector2(0.5f, 0.5f);
        debugObject1.Transform.Scale = new Vector2(0.5f, 0.5f);
        debugObject1.Transform.Position = new Vector2(0.8f, 1f);
        
        // object 2
        debugFloor.Name = "Floor";
        debugFloor.Transform.Position = new Vector2(0, -0.8f);
        debugFloor.Transform.Scale = new Vector2(5f, 0.45f);
        collider2.Size = debugFloor.Transform.Scale;
        
        // gold pieces (test objects)
        goldRigidBody.Mass = 2f;
        goldRigidBody2.Mass = 2f;
        goldRigidBody3.Mass = 2f;
        
        goldCollider.Size = new Vector2(0.25f, 0.25f);
        goldCollider2.Size = new Vector2(0.25f, 0.25f);
        goldCollider3.Size = new Vector2(0.25f, 0.25f);
        
        gold1.Transform.Scale = new Vector2(0.25f, 0.25f);
        gold2.Transform.Scale = new Vector2(0.25f, 0.25f);
        gold3.Transform.Scale = new Vector2(0.25f, 0.25f);
        
        gold1.Transform.Position = new Vector2(0f, 0f);
        gold2.Transform.Position = new Vector2(0f, 1f);
        
        // Add entities to the scene
        Entities.Add(cameraObject);
        Entities.Add(debugObject1);
        Entities.Add(debugFloor);
        Entities.Add(gold1);
        Entities.Add(gold2);
        Entities.Add(gold3);

        // Set this scene as the current active scene
        if (SceneManager.Instance == null)
            throw new InvalidOperationException("SceneManager instance is not initialized.");

        SceneManager.Instance.CurrentScene = this;
    }

    public override void UpdateScene()
    {
        base.UpdateScene();

        KeyboardState KeyboardState = Program.Engine.KeyboardState;

        if (KeyboardState.IsKeyDown(Keys.A))
            rigidBody1.Move(new Vector2(-1f, 0));
        if (KeyboardState.IsKeyDown(Keys.D))
            rigidBody1.Move(new Vector2(1f, 0));
        
        // Jumping
        if (KeyboardState.IsKeyDown(Keys.W))
        {
            rigidBody1.Move(new Vector2(0f, 5));
        }
    }
}