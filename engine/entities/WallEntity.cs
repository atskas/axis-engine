using OpenTK.Mathematics;
using UntitledEngine.engine.physics;

namespace UntitledEngine.engine.entities
{
    public class WallEntity : MeshEntity
    {
        // In WallEntity we have pos and scale as arguments because the game has both vertical and horizontal blockers to make sure nothing's off screen
        public WallEntity(Shader shader, Vector2 position, Vector2 scale)
            : base(Mesh.CreateQuadMesh(shader))
        {
            // Set all properties
            this.Transform.Scale = scale;
            this.Transform.Position = position;
            this.Color = new Vector4(0f, 0f, 0f, 0f);
        }

        public Vector2 HandleCollisionWithBall(BallEntity ball, ref Vector2 ballVelocity, float speedIncrease, float maxBallSpeed)
        {
            Vector2 resolution = ball.ballPhysics.HandleCollisionWith(this);

            if (Math.Abs(resolution.Y) > 0)
            {
                ballVelocity.Y *= -1 * speedIncrease;

                if (ballVelocity.Length > maxBallSpeed)
                    ballVelocity = Vector2.Normalize(ballVelocity) * maxBallSpeed;
            }

            return resolution;
        }
    }
}
