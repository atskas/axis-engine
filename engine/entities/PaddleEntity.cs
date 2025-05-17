using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using UntitledEngine.engine.physics;

// ------ CUSTOM ENTITY ------
// ---------------------------

namespace UntitledEngine.engine.entities
{
    public class PaddleEntity : MeshEntity
    {
        public Vector2 playerMoveSpeed = new Vector2(0f, 1.35f);
        private sPhysics paddlePhysics;

        public PaddleEntity(Shader shader, Vector2 position)
            : base(Mesh.CreateQuadMesh(shader))
        {
            // Create new physics instance for the entity
            paddlePhysics = new sPhysics(this);

            // Set all properties
            this.Transform.Scale = new Vector2(0.1f, 0.5f);
            this.Transform.Position = position;
            this.Color = Vector4.One;
        }

        // For player movement, will not apply to p2
        public void ProcessInput(KeyboardState keyboardState, List<BaseEntity> collidables, bool IsPlayer)
        {
            // Only handle key movement if paddle is the player's paddle
            if (IsPlayer)
            {
                // Paddle Vertical Movement
                if (keyboardState.IsKeyDown(Keys.W))
                    paddlePhysics.Move(this, playerMoveSpeed);

                if (keyboardState.IsKeyDown(Keys.S))
                    paddlePhysics.Move(this, -playerMoveSpeed);
            }

            // Handle collisions
            foreach (var BaseEntity in collidables)
            {
                if (BaseEntity != this)
                    this.paddlePhysics.HandleCollisionWith(BaseEntity);
            }

        }
    }
}
