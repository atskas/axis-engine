using OpenTK.Mathematics;
using UntitledEngine.engine.entities;
using UntitledEngine.engine.physics;
using UntitledEngine.engine;
using UntitledEngine;
using UntitledEngine.engine.audio;

public class BallEntity : MeshEntity
{
    public Physics ballPhysics;
    public Vector2 ballMoveSpeed = new Vector2(1f, 1f);
    public float maxBallSpeed = 1.8f;
    public float launchTimer = 5.0f;

    private BaseEntity leftWall;
    private BaseEntity rightWall;

    private AudioManager audioManager;

    public BallEntity(Shader shader)
        : base(Mesh.CreateQuadMesh(shader))
    {
        ballPhysics = new Physics(this);
        audioManager = new AudioManager();
        this.Transform.Scale = new Vector2(0.065f, 0.065f);
        this.Transform.Position = Vector2.Zero;
        this.Color = Vector4.One;
    }

    public void BallLoop(float deltaTime, List<BaseEntity> collidables, ref Camera camera) // Couldn't override think due to the custom arguments
    {
        launchTimer -= deltaTime;

        // Only move if not frozen
        if (launchTimer <= 0)
        {
            this.ballPhysics.Move(ballMoveSpeed);
            HandleBallStuff(collidables, ref camera);
        }
    }

    private void HandleBallStuff(List<BaseEntity> collidables, ref Camera camera)
    {
        foreach (var baseEntity in collidables)
        {
            if (baseEntity == this) continue;

            Vector2 resolution = ballPhysics.HandleCollisionWith(baseEntity);

            if (Math.Abs(resolution.X) > 0)
            {
                ballMoveSpeed.X *= -1.05f;
                audioManager.Play("sounds/hit.wav");
                camera.Shake(0.1f, 0.01f);
            }

            if (Math.Abs(resolution.Y) > 0)
            {
                ballMoveSpeed.Y *= -1.05f;
                audioManager.Play("sounds/hit.wav");
                camera.Shake(0.1f, 0.01f);
            }

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
