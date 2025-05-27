using Box2D.NetStandard.Dynamics.World;
using System.Numerics;
using UntitledEngine.Core.Scenes;

namespace UntitledEngine.Core.Physics;

public class PhysicsManager
{
    // Singleton instance of the PhysicsManager class
    public static PhysicsManager? Instance { get; private set; }
    
    public World Box2DWorld { get; private set; }

    private Vector2 gravity = new Vector2(0, -1);

    public PhysicsManager()
    {
        Instance = this;
        Box2DWorld = new World(gravity);
    }
    
    public void UpdatePhysics() // Uses a fixedDeltaTime instead of normal deltaTime for stability
    {
        const int velocityIterations = 8;
        const int positionIterations = 3;

        Box2DWorld.Step(Program.Engine.FixedDeltaTime, velocityIterations, positionIterations);
    }
}