using Box2D.NetStandard.Dynamics.World;
using System.Numerics;
using Box2D.NetStandard.Dynamics.World.Callbacks;
using UntitledEngine.Core.Entities;
using UntitledEngine.Core.Scenes;

namespace UntitledEngine.Core.Physics;

public class PhysicsManager
{
    // Singleton instance of the PhysicsManager class
    public static PhysicsManager? Instance { get; private set; }
    
    // Expose contact listener events
    private readonly PhysicsContactListener ContactListener = new PhysicsContactListener();
    
    public event Action<Entity, Entity>? OnBeginContact
    {
        add => ContactListener.OnBeginContact += value;
        remove => ContactListener.OnBeginContact -= value;
    }

    public event Action<Entity, Entity>? OnEndContact
    {
        add => ContactListener.OnEndContact += value;
        remove => ContactListener.OnEndContact -= value;
    }

    
    public World Box2DWorld { get; private set; }
    public Vector2 gravity = new Vector2(0, -7f);
    public PhysicsManager()
    {
        Instance = this;
        Box2DWorld = new World(gravity);
        Box2DWorld.SetContactListener(ContactListener); // Set the world's contact listener
    }
    
    public void UpdatePhysics() // Uses a fixedDeltaTime instead of normal deltaTime for stability
    {
        const int velocityIterations = 8;
        const int positionIterations = 3;

        Box2DWorld.Step(Engine.Instance.FixedDeltaTime, velocityIterations, positionIterations);
    }
}