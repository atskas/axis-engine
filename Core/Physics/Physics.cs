using OpenTK.Mathematics;
using UntitledEngine.Core.Entities;

namespace UntitledEngine.Core.Physics;

public static class Physics
{
    public static bool Collides(Entity a, Entity b)
    {
        // Check if the two entities have Collider components
        var aCollider = a.GetComponent<Collider>();
        var bCollider = b.GetComponent<Collider>();
        // Return if any do not
        if (aCollider == null || bCollider == null)
            return false;
        
        // Calculate world position of each collider (also accounts for any offset)
        Vector2 aPos = a.Transform.Position + aCollider.Offset;
        Vector2 bPos = b.Transform.Position + bCollider.Offset;
        
        // Calculate min and max points of each collider's AABB
        Vector2 aMin = aPos - aCollider.Size * 0.5f;
        Vector2 aMax = aPos + aCollider.Size * 0.5f;
        Vector2 bMin = bPos - bCollider.Size * 0.5f;
        Vector2 bMax = bPos + bCollider.Size * 0.5f;
        
        // Return true if the two AABBs overlap on both the X and Y axes, false otherwise
        return aMin.X <= bMax.X && aMax.X >= bMin.X &&
               aMin.Y <= bMax.Y && aMax.Y >= bMin.Y;
    }
    
    // Resolve collision with another entity by adjusting position minimally to separate them
    public static Vector2 HandleCollision(Entity a, Entity b)
    {
        if (!Collides(a, b))
            return Vector2.Zero;
        
        var rbA = a.GetComponent<Rigidbody>();
        var rbB = b.GetComponent<Rigidbody>();

        Vector2 resolution = CollisionResolve(a, b);
        
        // If a Rigidbody is missing, let's treat as static
        bool aStatic = rbA == null || rbA.IsStatic;
        bool bStatic = rbB == null || rbB.IsStatic;

        if (!aStatic && !bStatic)
        {
            float totalMass = rbA.Mass + rbB.Mass;
            a.Transform.Position += resolution * (rbB.Mass / totalMass);
            b.Transform.Position -= resolution * (rbA.Mass / totalMass);
        }
        else if (!aStatic)
        {
            a.Transform.Position += resolution;
        }
        else if (!bStatic)
        {
            b.Transform.Position -= resolution;
        }
        // if both are static, no movement
        
        return resolution;
    }
    
    // Static method to calculate the minimal translation vector to separate two overlapping AABBs
    private static Vector2 CollisionResolve(Entity a, Entity b)
    {
        // Check if the two entities have Collider components
        var aCollider = a.GetComponent<Collider>();
        var bCollider = b.GetComponent<Collider>();
        // Return if any do not
        if (aCollider == null || bCollider == null)
            return Vector2.Zero;

        Vector2 aPos = a.Transform.Position + aCollider.Offset;
        Vector2 bPos = b.Transform.Position + bCollider.Offset;
        
        Vector2 aMin = aPos - aCollider.Size * 0.5f;
        Vector2 aMax = aPos + aCollider.Size * 0.5f;
        Vector2 bMin = bPos - bCollider.Size * 0.5f;
        Vector2 bMax = bPos + bCollider.Size * 0.5f;
        
        // Calculate overlap distances on each axis
        float dx = MathF.Min(aMax.X - bMin.X, bMax.X - aMin.X);
        float dy = MathF.Min(aMax.Y - bMin.Y, bMax.Y - aMin.Y);
        
        // Resolve collision along the axis with the smallest overlap to avoid jitter
        if (dx < dy)
            return new Vector2(aPos.X < bPos.X ? -dx : dx, 0);
        else
            return new Vector2(0, aPos.Y < bPos.Y ? -dy : dy);
    }
}