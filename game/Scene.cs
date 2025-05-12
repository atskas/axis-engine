using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Reflection.Metadata;

namespace UntitledEngine
{
    internal class Scene
    {
        private Shader shader;
        private Shader shader2; // Example shader to show that you can use separate shaders for meshes

        // Game Objects
        private Entity paddle1;
        private Entity paddle2;

        // Walls, for collision testing purposes / Collision testing scene
        private Entity wallTop;
        private Entity wallBottom;

        private Entity ball;

        // Collidable objects
        private List<Entity> collidables;

        public Scene()
        {
            // Set up scene objects
            shader = new Shader();
            shader2 = new Shader();

            // Paddles
            paddle1 = new Entity((0.1f, 1.1f, 1f), (-0.45f, 0.0f, 0.0f), Vector4.One, shader);
            paddle2 = new Entity((0.1f, 1.1f, 1f), (0.45f, 0.0f, 0.0f), Vector4.One, shader);

            // Horizontal walls / Collision testing scene
            wallTop = new Entity((1.0f, 0.1f, 1f), (0f, 0.55f, 0f), Vector4.One, shader);
            wallBottom = new Entity((1.0f, 0.1f, 1f), (0f, -0.55f, 0f), Vector4.One, shader);

            // ^ These two form a box / Collision testing scene / Revert changes for PONG

            // Ball (currently a player with 8-dir movement) / Collision testing scene
            ball = new Entity((0.1f, 0.1f, 0.1f), (0.0f, 0.0f, 0.0f), Vector4.One, shader2);

            // Set up collidables (Add collidable objects to this list)
            collidables = new List<Entity>
            {
                paddle1,
                paddle2,
                wallTop,
                wallBottom,
            };
        }

        // Input handling
        public void ProcessInput(KeyboardState keyboardState, float deltaTime)
        {
            float moveSpeed = 1.2f * deltaTime;

            if (keyboardState.IsKeyDown(Keys.W))
                ball.Move(new Vector3(0f, moveSpeed, 0f));
            if (keyboardState.IsKeyDown(Keys.S))
                ball.Move(new Vector3(0f, -moveSpeed, 0f));
            if (keyboardState.IsKeyDown(Keys.A))
                ball.Move(new Vector3(-moveSpeed, 0f, 0f));
            if (keyboardState.IsKeyDown(Keys.D))
                ball.Move(new Vector3(moveSpeed, 0f, 0f));

            // Resolve collisions against everything
            foreach (var entity in collidables)
            {
                if (ball.CollidesWith(entity))
                {
                    ball.Move(Entity.CollisionResolve(ball, entity));
                }
            }

        }

        public void Render()
        {
            // Set colors and draw game objects onto the screen
            shader.SetShapeColor(paddle1.Color);
            paddle1.Render(shader);

            shader.SetShapeColor(paddle2.Color);
            paddle2.Render(shader);

            // Render walls (top and bottom) / Collision testing scene
            shader.SetShapeColor(wallTop.Color);
            wallTop.Render(shader);

            shader.SetShapeColor(wallBottom.Color);
            wallBottom.Render(shader);

            shader.SetShapeColor(ball.Color);
            ball.Render(shader);
        }

        public void Update(float deltaTime)
        {
            // This function runs every frame. To ensure smooth and consistent behavior across different frame rates, 
            // scale any time-dependent calculations (e.g., movement, animations) by deltaTime.
        }

        public void Cleanup()
        {
            // Mesh cleanup
            paddle1.Cleanup();
            paddle2.Cleanup();
            wallTop.Cleanup(); // Collision testing scene
            wallBottom.Cleanup(); // Collision testing scene
            ball.Cleanup();

            // Shader cleanup
            shader.Cleanup();
            shader2.Cleanup();
        }
    }
}
