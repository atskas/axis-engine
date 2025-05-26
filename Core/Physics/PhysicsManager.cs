using UntitledEngine.Core.Scenes;

namespace UntitledEngine.Core.Physics;

public class PhysicsManager
{
    // Singleton instance of the PhysicsManager class
    public static PhysicsManager? Instance { get; private set; }
    public PhysicsManager() => Instance = this;
    
    public void UpdatePhysics() // Uses a fixedDeltaTime instead of normal deltaTime for stability
    {
        var entities = SceneManager.Instance?.CurrentScene.Entities;
        if (entities == null) return;
        
        var entityList = entities.ToList();
        
        // Integrate rigidbodies
        foreach (var entity in entityList)
        {
            var rb = entity.GetComponent<Rigidbody>();
            if (rb == null || rb.IsStatic) continue;
            
            rb.Integrate(entity);
        }
        
        // Collision detection & resolution
        for (int i = 0; i < entityList.Count; i++)
        {
            var a = entityList[i];
            var colliderA = a.GetComponent<Collider>();
            if (colliderA == null) continue;
            
            for (int j = i + 1; j < entityList.Count; j++)
            {
                var b = entityList[j];
                var colliderB = b.GetComponent<Collider>();
                if (colliderB == null) continue;
                
                if (Physics.Collides(a, b))
                {
                    Physics.HandleCollision(a, b);
                }
            }
        }
    }
}