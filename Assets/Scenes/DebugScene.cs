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
    
    // test objects (many of them)
    private Entity debugObject2 = new Entity();
    private MeshRenderer meshRenderer2;
    
    private PhysicsBody debugBody2;
    
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
        
        // --- Add physics and initialise ---
        debugBody = new PhysicsBody(BodyType.Dynamic);
        debugObject1.AddComponent(debugBody);
        
        debugBody2 = new PhysicsBody(BodyType.Dynamic);
        debugObject2.AddComponent(debugBody2);
        
        debugFloorBody = new PhysicsBody(BodyType.Static);
        debugFloor.AddComponent(debugFloorBody);
        
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
        
        // gold piece (test object)
        debugObject2.Transform.Scale = new Vector2(0.25f, 0.25f);
        debugObject2.Transform.Position = new Vector2(0f, 0f);
        debugBody2.Density = 50f;
        
        // Add entities to the scene
        Entities.Add(camera);
        Entities.Add(debugObject1);
        Entities.Add(debugFloor);
        Entities.Add(debugObject2);

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
        float jumpVelocity = 3f;

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
            if (keyboard.IsKeyDown(Keys.W) && IsGrounded(debugObject1))
                velocity.Y = jumpVelocity;

        // Set updated velocity
        body.SetLinearVelocity(Engine.ToNumerics(velocity));
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