using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Dynamics.Contacts;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.World.Callbacks;
using UntitledEngine.Core.Entities;

namespace UntitledEngine.Core.Physics;

public class PhysicsContactListener : ContactListener
{
    public event Action<Entity, Entity> OnBeginContact;
    public event Action<Entity, Entity> OnEndContact;
    
    public override void BeginContact(in Contact contact)
    {
        var entityA = contact.FixtureA.Body.UserData as Entity;
        var entityB = contact.FixtureB.Body.UserData as Entity;
        
        if (entityA != null && entityB != null)
            OnBeginContact?.Invoke(entityA, entityB);
    }

    public override void EndContact(in Contact contact)
    {
        var entityA = contact.FixtureA.Body.UserData as Entity;
        var entityB = contact.FixtureB.Body.UserData as Entity;
        
        if (entityA != null && entityB != null)
            OnEndContact?.Invoke(entityA, entityB);
    }
    
    // Called before the physics engine solves the contact constraints for the current time step.
    public override void PreSolve(in Contact contact, in Manifold oldManifold)
    {
    }
    
    // Called after the physics engine has solved the contact and calculated the impulse applied to resolve the collision.
    public override void PostSolve(in Contact contact, in ContactImpulse impulse)
    {
    }
}