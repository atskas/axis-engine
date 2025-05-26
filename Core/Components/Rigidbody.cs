using OpenTK.Mathematics;
using UntitledEngine.Core.Entities;

public class Rigidbody : Component
{
    public Vector2 Velocity;
    public Vector2 Force;
    
    private float mass = 1f;
    public float Mass
    {
        get => mass;
        set => mass = value < 0 ? 0 : value;
    }

    public float InverseMass => Mass == 0 ? 0 : 1f / Mass;
    public bool IsStatic => Mass == 0f;
}