using System.Numerics;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using Box2D.NetStandard.Dynamics.World;
using UntitledEngine;
using UntitledEngine.Core.Entities;

namespace UntitledEngine.Core.Components;

public enum PhysicsShape
{
    Box,
    Circle,
    // Will add more later
}
public class PhysicsBody : Component
{
    public Body Body { get; private set; }

    private World _world;
    private BodyType _bodyType;
    private PhysicsShape _shapeType = PhysicsShape.Box; // Defaults to box
    private bool _isInitialized = false;
    
    private float _density = 1f; // default density
    private bool _fixedRotation;
    
    public BodyType BodyType
    {
        get => _bodyType;
        set
        {
            _bodyType = value;
            if (Body != null)
            {
                Body.SetType(_bodyType);
                Body.ResetMassData(); // Just to make sure it doesnt cause any issues
            }
        }
    }
    public PhysicsShape ShapeType
    {
        get => _shapeType;
        set
        {
            if (_shapeType == value) return;
            _shapeType = value;
            if (Body != null)
                RebuildFixture();
        }
    }

    public Vector2 BodyPosition
    {
        get => Body.Position;
        set
        {
            if (Body != null)
            {
                Body.SetTransform(value, Body.GetAngle());
                // Update entity's transform to keep in sync
                Entity.Transform.Position = value;
            }
        }
    }
    
    public float BodyRotation
    {
        get => Body.GetAngle();
        set
        {
            if (Body != null)
            {
                Body.SetTransform(Body.Position, value);
                // Update entity's rotation to keep in sync
                Entity.Transform.Rotation = value;
            }
        }
    }
    
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
    public float Friction { get; set; } = 0.5f;
    public float LinearDamping { get; set; } = 2f;
    public float AngularDamping { get; set; } = 0f;
    public bool FixedRotation
    {
        get => _fixedRotation;
        set
        {
            if (_fixedRotation != value)
            {
                _fixedRotation = value;
                if (Body != null)
                {
                    Body.SetFixedRotation(value);
                }
            }
        }
    }
    
    private System.Numerics.Vector2 _entityPosition;
    private System.Numerics.Vector2? _shapeScale = null;
    public System.Numerics.Vector2 ShapeScale // Scale used by the shape
    {
        get => _shapeScale ?? Entity.Transform.Scale;
        set
        {
            if (_shapeScale == value) return;
            _shapeScale = value;
            if (Body != null)
            {
                RebuildFixture();
            }
        }
    }


    public PhysicsBody(BodyType bodyType)
    {
        Initialise(Engine.Instance.PhysicsManager.Box2DWorld, bodyType);
    }
    

    public override void Start()
    {
        if (!_isInitialized)
            throw new InvalidOperationException("PhysicsBody not initialized.");
        
        if (Body != null)
            return; // Already initialized, skip


        var position = Entity.Transform.Position;

        var bodyDef = new BodyDef
        {
            type = _bodyType,
            position = position,
        };
        bodyDef.userData = Entity;

        Body = _world.CreateBody(bodyDef);

        const float RenderToPhysicsScale = 1f;

        Shape shape;
        var physicsScale = ShapeScale * RenderToPhysicsScale;
        float scaleX = physicsScale.X;
        float scaleY = physicsScale.Y;

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
    
    private void RebuildFixture()
    {
        if (Body == null)
            return;

        // Destroy all existing fixtures
        var fixture = Body.GetFixtureList();
        while (fixture != null)
        {
            var next = fixture.GetNext();
            Body.DestroyFixture(fixture);
            fixture = next;
        }

        const float RenderToPhysicsScale = 1f;
        var physicsScale = ShapeScale * RenderToPhysicsScale;
        float scaleX = physicsScale.X;
        float scaleY = physicsScale.Y;

        Shape shape;

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

        // Reapply damping and rotation settings
        Body.SetLinearDampling(LinearDamping);
        Body.SetAngularDamping(AngularDamping);
        Body.SetFixedRotation(FixedRotation);
        Body.SetSleepingAllowed(true);
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
            Entity.Transform.Position = Body.Position;
            Entity.Transform.Rotation = Body.GetAngle();
        }
    }
}