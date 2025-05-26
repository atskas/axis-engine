using OpenTK.Mathematics;
using UntitledEngine;
using UntitledEngine.Core.Entities;

public class Rigidbody : Component
{
    private Entity entity;
    
    public static Vector2 Gravity = new Vector2(0f, -9.81f);
    
    public Vector2 Velocity;
    public Vector2 Acceleration => Mass == 0f ? Vector2.Zero : Force / Mass;
    public Vector2 Force;
    public float Mass { get; set; }
    public bool IsStatic => Mass == 0f;

    public void Integrate(Entity entity)
    {
        if (IsStatic) return;

        this.entity = entity;
        
        Velocity += Acceleration * Program.Engine.deltaTime; // Update velocity based on acceleration
        entity.Transform.Position += Velocity * Program.Engine.deltaTime; // Update position based on velocity
        
        // Reset force for the next frame
        Force = Vector2.Zero;
    }

    // Adds velocity (predictable movement)
    public void AddVelocity(Vector2 velocityDelta)
    {
        if (!IsStatic)
            Velocity += velocityDelta;
    }
    
    // Adds force (realistic movement)
    public void AddForce(Vector2 force)
    {
        if (!IsStatic)
            Force += force;
    }

    public override void Update()
    {
        if (IsStatic) return;
        
        // Apply gravity force automatically
        AddForce(Gravity * Mass);
    }
}