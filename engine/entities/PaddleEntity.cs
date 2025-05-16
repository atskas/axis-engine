using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using UntitledEngine.engine.physics;

namespace UntitledEngine.engine.entities
{
    public class PaddleEntity : MeshEntity
    {
        private Vector2 moveSpeed;
        private pPhysics paddlePhysics;

        public PaddleEntity(Shader shader, Vector2 speed)
            : base(Mesh.CreateQuadMesh(shader))
        {
            this.moveSpeed = speed;
            paddlePhysics = new pPhysics(this);

            // Set properties
            this.Transform.Scale = new Vector2(0.1f, 0.5f);
            this.Transform.Position = new Vector2(-0.85f, 0f);
            this.Color = Vector4.One;
        }

        public void ProcessInput(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W))
                paddlePhysics.Move(this, moveSpeed);

            if (keyboardState.IsKeyDown(Keys.S))
                paddlePhysics.Move(this, -moveSpeed);

            // Handle collisions

        }
    }
}
