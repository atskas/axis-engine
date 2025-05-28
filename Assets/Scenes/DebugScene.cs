using System.Numerics;
using Box2D.NetStandard.Dynamics.Bodies;
using Silk.NET.Input;
using UntitledEngine.Core.Assets;
using UntitledEngine.Core.Components;
using UntitledEngine.Core.ECS;
using UntitledEngine.Core.Physics;
using UntitledEngine.Core.Scenes;

namespace UntitledEngine.Assets.Scenes;

internal class DebugScene : Scene
{
    // camera
    private Entity cameraEntity;
    private Camera camera;
    
    // object 1
    private Entity debugObject1 = new Entity();
    private MeshRenderer meshRenderer;
    private PlayerController playerController;

    private PhysicsBody debugBody;
    
    // object 2
    private Entity debugFloor = new Entity();
    private MeshRenderer debugFloorMeshRenderer;
    
    private PhysicsBody debugFloorBody;
    
    // object 3
    private Entity debugObject2 = new Entity();
    private MeshRenderer meshRenderer2;
    
    private PhysicsBody debugBody2;
    
    // object 4
    private Entity debugSphere = new Entity();
    private MeshRenderer debugSphereMeshRenderer;
    
    private PhysicsBody debugSphereBody;
    
    // game stuff
    public float moveSpeed = 2.5f;
    public float jumpVelocity = 3f;
    
    public DebugScene()
    {
        // Camera
        camera = new Camera();
        cameraEntity =  new Entity();
        cameraEntity.Name = "Camera";
        cameraEntity.AddTag("Camera");
        cameraEntity.AddComponent(camera);
        
        // Create components and add them
        meshRenderer = new MeshRenderer(new Texture("Assets/Textures/red.png"));
        playerController = new PlayerController();
        debugObject1.AddComponent(meshRenderer);
        debugObject1.AddComponent(playerController);
        
        debugFloorMeshRenderer = new MeshRenderer(new Texture("Assets/Textures/blue.png"));
        debugFloor.AddComponent(debugFloorMeshRenderer);
        
        meshRenderer2 = new MeshRenderer(new Texture("Assets/Textures/green.png"));
        debugObject2.AddComponent(meshRenderer2);
        
        debugSphereMeshRenderer = new MeshRenderer(new Texture("Assets/Textures/sphere.png"));
        debugSphere.AddComponent(debugSphereMeshRenderer);
        
        // --- Add physics and initialise ---
        debugBody = new PhysicsBody(BodyType.Dynamic);
        debugObject1.AddComponent(debugBody);
        
        debugBody2 = new PhysicsBody(BodyType.Dynamic);
        debugObject2.AddComponent(debugBody2);
        
        debugFloorBody = new PhysicsBody(BodyType.Static);
        debugFloor.AddComponent(debugFloorBody);

        debugSphereBody = new PhysicsBody(BodyType.Dynamic);
        debugSphere.AddComponent(debugSphereBody);
        
        // --- Set properties ---
        
        // object 1
        debugObject1.Name = "Player";
        debugObject1.Transform.Scale = new Vector2(0.25f, 0.25f);
        debugObject1.Transform.Position = new Vector2(0.8f, 1f);
        debugBody.Density = 5f;
        debugBody.FixedRotation = true;
        
        // object 2
        debugFloor.Name = "Floor";
        debugFloor.AddTag("Ground");
        debugFloor.Transform.Position = new Vector2(0, -0.8f);
        debugFloor.Transform.Scale = new Vector2(5f, 1f);
        
        // object 3
        debugObject2.Transform.Scale = new Vector2(0.25f, 0.25f);
        debugObject2.Transform.Position = new Vector2(0f, 0f);
        debugBody2.Density = 50f;
        
        // object 4
        debugSphere.Transform.Scale = new Vector2(0.3f, 0.3f);
        debugSphere.Transform.Position = new Vector2(0f, 0f);
        debugSphereBody.Density = 20f;
        debugSphereBody.ShapeType = PhysicsShape.Circle;
        
        // Add entities to the scene
        Entities.Add(cameraEntity);
        Entities.Add(debugObject1);
        Entities.Add(debugFloor);
        Entities.Add(debugObject2);
        Entities.Add(debugSphere);

        // Set this scene as the current active scene
        if (SceneManager.Instance == null)
            throw new InvalidOperationException("SceneManager instance is not initialized.");

        SceneManager.Instance.CurrentScene = this;
    }
}