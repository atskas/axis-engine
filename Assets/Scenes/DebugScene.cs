using Box2D.NetStandard.Dynamics.Bodies;
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

    private PhysicsBody debugBody;
    
    // object 2
    private Entity debugFloor = new Entity();
    private MeshRenderer debugFloorMeshRenderer;
    
    private PhysicsBody debugFloorBody;
    
    // test objects (many of them)
    private Entity gold1 = new Entity();
    private Entity gold2 = new Entity();
    private MeshRenderer goldMeshRenderer;
    
    private PhysicsBody goldBody1;
    
    public DebugScene()
    {
        // Camera
        Entity cameraObject = new CameraEntity();
        
        // Create components and add them
        meshRenderer = new MeshRenderer(new Texture("Assets/Textures/texture.jpg"));
        debugObject1.AddComponent(meshRenderer);
        
        debugFloorMeshRenderer = new MeshRenderer(new Texture("Assets/Textures/floor.jpg"));
        debugFloor.AddComponent(debugFloorMeshRenderer);
        
        // gold pieces
        goldMeshRenderer = new MeshRenderer(new Texture("Assets/Textures/gold.jpg"));
        
        // add their components
        gold1.AddComponent(goldMeshRenderer);
        
        // --- Add physics and initialise ---
        debugBody = new PhysicsBody(BodyType.Dynamic);
        debugObject1.AddComponent(debugBody);
        
        goldBody1 = new PhysicsBody(BodyType.Dynamic);
        gold1.AddComponent(goldBody1);
        
        debugFloorBody = new PhysicsBody(BodyType.Static);
        debugFloor.AddComponent(debugFloorBody);
        
        // --- Set properties ---
        
        // object 1
        debugObject1.Name = "Test";
        debugObject1.Transform.Scale = new Vector2(0.5f, 0.5f);
        debugObject1.Transform.Position = new Vector2(0.8f, 1f);
        
        // object 2
        debugFloor.Name = "Floor";
        debugFloor.Transform.Position = new Vector2(0, -0.8f);
        debugFloor.Transform.Scale = new Vector2(5f, 0.45f);
        
        // gold piece (test object)
        gold1.Transform.Scale = new Vector2(0.25f, 0.25f);
        gold1.Transform.Position = new Vector2(0f, 0f);
        
        // Add entities to the scene
        Entities.Add(cameraObject);
        Entities.Add(debugObject1);
        Entities.Add(debugFloor);
        Entities.Add(gold1);

        // Set this scene as the current active scene
        if (SceneManager.Instance == null)
            throw new InvalidOperationException("SceneManager instance is not initialized.");

        SceneManager.Instance.CurrentScene = this;
    }

    public override void UpdateScene()
    {
        base.UpdateScene();

        var keyboard = Program.Engine.KeyboardState;
        float moveSpeed = 2.5f;
        float jumpVelocity = 1f;

        var body = debugBody.Body;

        // Get current linear velocity
        Vector2 velocity = Engine.ToOpenTK(body.GetLinearVelocity());

        // Horizontal movement
        if (keyboard.IsKeyDown(Keys.A))
            velocity.X = -moveSpeed;
        else if (keyboard.IsKeyDown(Keys.D))
            velocity.X = moveSpeed;
        else
            velocity.X = 0;
        
        if (keyboard.IsKeyDown(Keys.W))
            velocity.Y = jumpVelocity;

        // Set updated velocity
        body.SetLinearVelocity(Engine.ToNumerics(velocity));
    }
}