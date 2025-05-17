using OpenTK.Mathematics;
using UntitledEngine.engine.entities;
using UntitledEngine.engine.physics;
using UntitledEngine.engine;
using UntitledEngine;

public class BallEntity : MeshEntity
{
    public sPhysics ballPhysics;
    public Vector2 ballMoveSpeed = new Vector2(1f, 1f);
    public float maxBallSpeed = 1.8f;
    public float launchTimer = 2.0f;

    private BaseEntity leftWall;
    private BaseEntity rightWall;

    public BallEntity(Shader shader)
        : base(Mesh.CreateQuadMesh(shader))
    {
        ballPhysics = new sPhysics(this);
        this.Transform.Scale = new Vector2(0.065f, 0.065f);
        this.Transform.Position = Vector2.Zero;
        this.Color = Vector4.One;
    }

    public void BallLoop(float deltaTime, List<BaseEntity> collidables) // Couldn't override think due to the custom arguments
    {
        launchTimer -= deltaTime;

        // Only move if not frozen
        if (launchTimer <= 0)
        {
            Transform.Position += ballMoveSpeed * deltaTime;
            HandleBallCollision(collidables);
        }
    }

    private void HandleBallCollision(List<BaseEntity> collidables)
    {
        foreach (var baseEntity in collidables)
        {
            if (baseEntity == this) continue;

            Vector2 resolution = ballPhysics.HandleCollisionWith(baseEntity);

            if (Math.Abs(resolution.X) > 0)
                ballMoveSpeed.X *= -1.05f;

            if (Math.Abs(resolution.Y) > 0)
                ballMoveSpeed.Y *= -1.05f;

            if (ballMoveSpeed.Length > maxBallSpeed)
                ballMoveSpeed = Vector2.Normalize(ballMoveSpeed) * maxBallSpeed;
        }
    }

    public void Reset()
    {
        this.Transform.Position = Vector2.Zero;
        ballMoveSpeed = new Vector2(1f, 1f);
        launchTimer = 2.0f;
    }
}
