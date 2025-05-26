using OpenTK.Mathematics;
using UntitledEngine.Core.Entities;

namespace UntitledEngine.Core.Physics;

public class Physics
{
    public static bool CollidesWith(Entity a, Entity b)
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
        return aMin.X <= bMax.Y && aMax.X >= bMin.X &&
               aMin.Y <= bMax.Y && aMax.Y >= bMin.Y;
    }
}