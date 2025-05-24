using OpenTK.Mathematics;
using UntitledEngine.Common.entities;

namespace UntitledEngine.Common.physics
{
    public class Physics
    {
        public Vector2 Velocity { get; set; }
        private BaseEntity entity;            // The entity this physics instance controls
        private Vector2 prevPosition;         // Stores the entity's previous position for velocity calculation

        // Constructor binds this physics instance to a specific entity
        public Physics(BaseEntity entity)
        {
            this.entity = entity;
        }

        // Moves the entity by a velocity vector scaled by deltaTime, updates Velocity property
        public void Move(Vector2 velocity)
        {
            prevPosition = entity.Transform.Position;                          // Cache previous position
            entity.Transform.Position += velocity * Engine.deltaTime;
            this.Velocity = (entity.Transform.Position - prevPosition) / Engine.deltaTime;
        }

        // Checks if this entity is colliding with another
        public bool CollidesWith(BaseEntity other)
        {
            Vector2 aMin = entity.Transform.Position - entity.Transform.Scale * 0.5f;
            Vector2 aMax = entity.Transform.Position + entity.Transform.Scale * 0.5f;

            Vector2 bMin = other.Transform.Position - other.Transform.Scale * 0.5f;
            Vector2 bMax = other.Transform.Position + other.Transform.Scale * 0.5f;

            // Return true if the two AABBs overlap on both axes
            return
                aMin.X <= bMax.X && aMax.X >= bMin.X &&
                aMin.Y <= bMax.Y && aMax.Y >= bMin.Y;
        }

        // Resolves collision with another entity by adjusting position minimally to separate them
        public Vector2 HandleCollisionWith(BaseEntity other)
        {
            if (!CollidesWith(other))
                return Vector2.Zero;

            Vector2 resolution = CollisionResolve(entity, other);  // Calculate resolution vector to separate entities
            entity.Transform.Position += resolution;               // Adjust position to resolve overlap
            return resolution;
        }

        // Static method to calculate the minimal translation vector to separate two overlapping AABBs
        public static Vector2 CollisionResolve(BaseEntity a, BaseEntity b)
        {
            Vector2 aMin = a.Transform.Position - a.Transform.Scale * 0.5f;
            Vector2 aMax = a.Transform.Position + a.Transform.Scale * 0.5f;

            Vector2 bMin = b.Transform.Position - b.Transform.Scale * 0.5f;
            Vector2 bMax = b.Transform.Position + b.Transform.Scale * 0.5f;

            // Calculate overlap distances on each axis
            float dx = MathF.Min(aMax.X - bMin.X, bMax.X - aMin.X);
            float dy = MathF.Min(aMax.Y - bMin.Y, bMax.Y - aMin.Y);

            // Resolve collision along the axis with the smallest overlap to avoid jitter
            if (dx < dy)
                return new Vector2(a.Transform.Position.X < b.Transform.Position.X ? -dx : dx, 0);
            else
                return new Vector2(0, a.Transform.Position.Y < b.Transform.Position.Y ? -dy : dy);
        }
    }
}
