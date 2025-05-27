using System.Numerics;
using Box2D.NetStandard.Dynamics.Bodies;
using Silk.NET.Input;
using UntitledEngine.Core.Assets;
using UntitledEngine.Core.Components;
using UntitledEngine.Core.ECS;
using UntitledEngine.Core.Entities;
using UntitledEngine.Core.Physics;
using UntitledEngine.Core.Scenes;

namespace UntitledEngine.Assets.Scenes;

internal class DebugScene : Scene
{
    // camera
    private Entity camera;
    
    // object 1
    private Entity debugObject1 = new Entity();
    private MeshRenderer meshRenderer;

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
        camera = new CameraEntity();
        
        // Create components and add them
        meshRenderer = new MeshRenderer(new Texture("Assets/Textures/geometry.jpg"));
        debugObject1.AddComponent(meshRenderer);
        
        debugFloorMeshRenderer = new MeshRenderer(new Texture("Assets/Textures/floor.png"));
        debugFloor.AddComponent(debugFloorMeshRenderer);
        
        meshRenderer2 = new MeshRenderer(new Texture("Assets/Textures/spike.png"));
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
        debugObject1.Name = "Test";
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
        Entities.Add(camera);
        Entities.Add(debugObject1);
        Entities.Add(debugFloor);
        Entities.Add(debugObject2);
        Entities.Add(debugSphere);

        // Set this scene as the current active scene
        if (SceneManager.Instance == null)
            throw new InvalidOperationException("SceneManager instance is not initialized.");

        SceneManager.Instance.CurrentScene = this;
    }

    public override void UpdateScene()
    {
        base.UpdateScene();

        // Get current linear velocity
        var velocity = debugBody.Body.GetLinearVelocity();
        
        if (Engine.Instance.InputManager.KeyDown(Key.A))
        {
            velocity.X = -moveSpeed;
        }
        else if (Engine.Instance.InputManager.KeyDown(Key.D))
        {
            velocity.X = moveSpeed;
        }
        else
        {
            velocity.X = 0;
        }

        if (Engine.Instance.InputManager.KeyDown(Key.W))
        {
            if (IsGrounded(debugObject1))
                velocity.Y = jumpVelocity;
        }
    }
    
    // Grounded check
    public bool IsGrounded(Entity entity)
    {
        var body = entity.GetComponent<PhysicsBody>().Body;
        var position = body.GetPosition(); // This is already System.Numerics.Vector2

        bool isGrounded = false;
        float offset = 0.15f;
        float rayLength = 0.15f;

        void CastRay(System.Numerics.Vector2 origin)
        {
            var start = origin;
            var end = origin + new System.Numerics.Vector2(0, -rayLength);

            PhysicsManager.Instance.Box2DWorld.RayCast((fixture, point, normal, fraction) =>
            {
                if (fixture.Body != body)
                    isGrounded = true;

            }, start, end);
        }

        CastRay(position);
        CastRay(position + new System.Numerics.Vector2(-offset, 0));
        CastRay(position + new System.Numerics.Vector2(+offset, 0));

        return isGrounded;
    }

}