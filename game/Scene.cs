using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Reflection.Metadata;

namespace UntitledEngine
{
    internal class Scene
    {
        private Shader shader; // You can also use separate shaders for objects

        // Game Objects
        private Entity paddle1;
        private Entity paddle2;

        private Entity ball;

        // Blockers
        private Entity blocker1;
        private Entity blocker2;

        // Collidable objects
        private List<Entity> collidables;

        public Scene()
        {
            // Set up scene objects
            shader = new Shader();

            // Paddles
            paddle1 = new Entity((0.1f, 0.65f, 1f), (-0.85f, 0.0f, 0.0f), Vector4.One, shader);
            paddle2 = new Entity((0.1f, 0.65f, 1f), (0.85f, 0.0f, 0.0f), Vector4.One, shader);

            // Walls (to avoid paddles from going out of screen)
            // You could also do this by setting a restriction to the Y position and
            // stopping movement once that restriction is met
            blocker1 = new Entity((5f, 0.2f, 1f), (0.0f, 1.1f, 0.0f), (0.0f, 0.0f, 0.0f, 0.0f), shader);
            blocker2 = new Entity((5f, 0.2f, 1f), (0.0f, -1.1f, 0.0f), (0.0f, 0.0f, 0.0f, 0.0f), shader);

            // Ball
            ball = new Entity((0.1f, 0.1f, 0.1f), (0.0f, 0.0f, 0.0f), Vector4.One, shader);

            // Set up collidables (Add collidable objects to this list)
            collidables = new List<Entity>
            {
                paddle2,
                blocker1,
                blocker2
            };
        }

        // Input handling
        public void ProcessInput(KeyboardState keyboardState, float deltaTime)
        {
            float moveSpeed = 1.3f * deltaTime;

            if (keyboardState.IsKeyDown(Keys.W))
                paddle1.Move((0f, moveSpeed, 0f));
            if (keyboardState.IsKeyDown(Keys.S))
                paddle1.Move((0f, -moveSpeed, 0f));
            // You would do this for 8-Directional movement.
           // if (keyboardState.IsKeyDown(Keys.A))
               // player.Move(new Vector3(-moveSpeed, 0f, 0f));
           // if (keyboardState.IsKeyDown(Keys.D))
               // player.Move(new Vector3(moveSpeed, 0f, 0f));

            // Resolve collisions against everything
            foreach (var entity in collidables)
            {
                if (paddle1.CollidesWith(entity))
                {
                    paddle1.Move(Entity.CollisionResolve(paddle1, entity));
                }
            }

        }

        public void Render()
        {
            // Render game objects onto the screen
            paddle1.Render(shader);
            paddle2.Render(shader);
            ball.Render(shader);
            blocker1.Render(shader);
            blocker2.Render(shader);

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
            ball.Cleanup();


            // Blocker cleanup
            blocker1.Cleanup();
            blocker2.Cleanup();

            // Shader cleanup
            shader.Cleanup();
        }
    }
}
