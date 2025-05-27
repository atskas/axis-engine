using System.Reflection;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using Box2D.NetStandard.Dynamics.World;
using OpenTK.Mathematics;
using UntitledEngine;
using UntitledEngine.Core.Entities;

public enum PhysicsShape
{
    Box,
    Circle,
    // Will add more later
}
public class PhysicsBody : Component
{
    public Body Body { get; private set; }
    public PhysicsShape ShapeType { get; set; } = PhysicsShape.Box; // Default to Box
    private World _world;
    private BodyType _bodyType;
    private bool _isInitialized = false;
    
    // Physics properties
    private float _density = 1f; // default density

    public float Density
    {
        get => _density;
        set
        {
            if (_density == value) return; // avoid unnecessary changes
            _density = value;

            if (Body != null)
            {
                var fixture = Body.GetFixtureList();
                while (fixture != null)
                {
                    fixture.Density = _density;
                    fixture = fixture.GetNext();
                }
                Body.ResetMassData();
            }
        }
    }
    
    
    
    public float Friction { get; set; } = 0.3f;
    public float LinearDamping { get; set; } = 2f;
    public float AngularDamping { get; set; } = 0f;
    public bool FixedRotation { get; set; } = false;
    
    private System.Numerics.Vector2 _entityPosition;

    public PhysicsBody(BodyType bodyType)
    {
        Initialise(Program.Engine.PhysicsManager.Box2DWorld, bodyType);
    }
    

    public override void Start()
    {
        if (!_isInitialized)
            throw new InvalidOperationException("PhysicsBody not initialized.");
        
        if (Body != null)
            return; // Already initialized, skip


        var position = Engine.ToNumerics(Entity.Transform.Position);

        var bodyDef = new BodyDef
        {
            type = _bodyType,
            position = position,
        };
        bodyDef.userData = Entity;

        Body = _world.CreateBody(bodyDef);

        const float RenderToPhysicsScale = 1f;

        Shape shape;
        float scaleX = Entity.Transform.Scale.X * RenderToPhysicsScale;
        float scaleY = Entity.Transform.Scale.Y * RenderToPhysicsScale;

        switch (ShapeType)
        {
            case PhysicsShape.Circle:
                var circleShape = new CircleShape();
                circleShape.Radius = scaleX * 0.5f;  // radius from X scale
                shape = circleShape;
                break;

            case PhysicsShape.Box:
            default:
                var boxShape = new PolygonShape();
                boxShape.SetAsBox(scaleX * 0.5f, scaleY * 0.5f);
                shape = boxShape;
                break;
        }
        
        var fixtureDef = CreateFixtureDef(shape);
        Body.CreateFixture(fixtureDef);
        Body.ResetMassData();

        Body.SetLinearDampling(LinearDamping);
        Body.SetAngularDamping(AngularDamping);
        Body.SetFixedRotation(FixedRotation);
        Body.SetSleepingAllowed(true);
    }
    
    private FixtureDef CreateFixtureDef(Shape shape)
    {
        return new FixtureDef
        {
            shape = shape,
            density = Density,
            friction = Friction
        };
    }
    

    // This is called externally before Start()
    public void Initialise(World world, BodyType bodyType)
    {
        _world = world;
        _bodyType = bodyType;
        _isInitialized = true;
    }

    public override void Update()
    {
        if (Body != null)
        {
            Entity.Transform.Position = Engine.ToOpenTK(Body.Position);
            Entity.Transform.Rotation = Body.GetAngle();
        }
    }
}