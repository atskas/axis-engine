using System.Reflection;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using Box2D.NetStandard.Dynamics.World;
using OpenTK.Mathematics;
using UntitledEngine;
using UntitledEngine.Core.Entities;

public class PhysicsBody : Component
{
    public Body Body { get; private set; }
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
        Initialise(Program.Engine.physicsManager.Box2DWorld, bodyType);
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

        Body = _world.CreateBody(bodyDef);

        const float RenderToPhysicsScale = 1f; // example scale factor
        float halfWidth = (Entity.Transform.Scale.X * RenderToPhysicsScale) * 0.5f;
        float halfHeight = (Entity.Transform.Scale.Y * RenderToPhysicsScale) * 0.5f;

        var boxShape = new PolygonShape();
        boxShape.SetAsBox(halfWidth, halfHeight);

        var fixtureDef = CreateFixtureDef(boxShape);
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