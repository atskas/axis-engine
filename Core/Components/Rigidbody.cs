using OpenTK.Mathematics;
using UntitledEngine;
using UntitledEngine.Core.Entities;

public class Rigidbody : Component
{
    private Entity entity;
    
    public Vector2 Gravity = new Vector2(0f, -1f);
    
    public Vector2 Velocity;
    public Vector2 Acceleration => Mass == 0f ? Vector2.Zero : Force / Mass;
    public Vector2 Force;
    public float Mass { get; set; }
    public bool IsStatic => Mass == 0f;

    public float TerminalVelocity = -5f; // Gotta hardcode this one, equation is too difficult.

    public void Integrate(Entity entity)
    {
        if (IsStatic) return;

        this.entity = entity;
        
        // Will soon apply friction alongside this
        if (Force == Vector2.Zero)
            ClearMovement();
        
        Velocity += Acceleration * Program.Engine.FixedDeltaTime; // Update velocity based on acceleration
        if (Velocity.Y < TerminalVelocity) // Clamp after update
            Velocity.Y = TerminalVelocity;
        
        entity.Transform.Position += Velocity * Program.Engine.FixedDeltaTime; // Update position based on velocity
        
        // Reset force for the next frame
        Force = Vector2.Zero;
    }

    // Simple movement
    public void Move(Vector2 velocity)
    {
        if (!IsStatic)
            entity.Transform.Position += velocity * Program.Engine.DeltaTime;
    }
    
    // Better Movement
    public void AddForce(Vector2 force)
    {
        if (!IsStatic)
            Force += force;
    }

    public void ClearMovement()
    {
        Force = Vector2.Zero;
        Velocity = Vector2.Zero;
    }

    public override void Update()
    {
        if (IsStatic) return;
        
        // Apply gravity force automatically (to a point)
        AddForce(Gravity * Mass);
    }
}