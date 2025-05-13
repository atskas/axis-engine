using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace UntitledEngine
{
    internal class Scene
    {
        private Shader shader;

        // Collidable objects
        private List<Entity> collidables;

        // Game-specific stuff
        public Vector2 ballMoveSpeed = new Vector2(1f, 1f);
        public float maxBallSpeed = 3f;
        public float speedIncrease = 1.025f;

        // Game Objects
        private Entity paddle1;
        private Entity paddle2;
        private Entity ball;
        private Entity blocker1;    // Blockers (primarily used for making the ball bounce from the ceiling and limiting paddle Y)
        private Entity blocker2;
        private Entity sideCollider1;   // Side colliders (primarily used for detecting when a paddle misses the ball)
        private Entity sideCollider2;

        public Scene()
        {
            // Set up scene objects
            shader = new Shader();

            // Paddles
            paddle1 = new Entity((0.1f, 0.3f), (-0.85f, 0.0f), Vector4.One, shader);
            paddle2 = new Entity((0.1f, 0.3f), (0.85f, 0.0f), Vector4.One, shader);

            // Walls (to avoid paddles from going out of screen)
            // You could also do this by setting a restriction to the Y position and
            // stopping movement once that restriction is met
            blocker1 = new Entity((5f, 0.2f), (0f, 1.1f), (0f, 0f, 0f, 0f), shader);
            blocker2 = new Entity((5f, 0.2f), (0f, -1.1f), (0f, 0f, 0f, 0f), shader);

            sideCollider1 = new Entity((0.2f, 5f), (1f, 0f), (0f, 0f, 0f, 0f), shader);
            sideCollider2 = new Entity((0.2f, 5f), (-1f, 0f), (0f, 0f, 0f, 0f), shader);

            // Ball
            ball = new Entity((0.05f, 0.05f), (0.0f, 0.0f), Vector4.One, shader);

            // Set up collidables (Add collidable objects to this list)
            collidables = new List<Entity>
            {
                paddle1,
                paddle2,
                blocker1,
                blocker2,
                sideCollider1,
                sideCollider2,
            };
        }

        // Input handling
        public void ProcessInput(KeyboardState keyboardState)
        {
            float moveSpeed = 1.3f;

            // p1
            if (keyboardState.IsKeyDown(Keys.W))
                paddle1.Move(new Vector2(0f, moveSpeed));
            if (keyboardState.IsKeyDown(Keys.S))
                paddle1.Move(new Vector2(0f, -moveSpeed));

            // p2
            if (keyboardState.IsKeyDown(Keys.Up))
                paddle2.Move(new Vector2(0f, moveSpeed));
            if (keyboardState.IsKeyDown(Keys.Down))
                paddle2.Move(new Vector2(0f, -moveSpeed));

            // Handle collisions
            foreach (var entity in collidables) // This goes through all the entities in collidables so it only applies to them
            {
                if (entity != paddle1)
                    paddle1.HandleCollisionWith(entity);
            }

            foreach (var entity in collidables)
            {
                if (entity != paddle2)
                    paddle2.HandleCollisionWith(entity);
            }
        }


        // [GAME-SPECIFIC METHOD] You can handle normal collisions using Entity.HandleCollisionWith(...);
        private void HandleBallCollision()
        {
            foreach (var entity in collidables)
            {
                if (entity == ball) continue;

                Vector2 resolution = ball.HandleCollisionWith(entity);

                if (Math.Abs(resolution.X) > 0)
                    ballMoveSpeed.X *= -1 * speedIncrease;

                if (Math.Abs(resolution.Y) > 0)
                    ballMoveSpeed.Y *= -1 * speedIncrease;

                // Clamp speed
                if (ballMoveSpeed.Length > maxBallSpeed)
                    ballMoveSpeed = Vector2.Normalize(ballMoveSpeed) * maxBallSpeed;
            }
        }


        public void Update(float deltaTime)
        {
            // This function runs every frame. To ensure smooth and consistent behavior across different frame rates, 
            // scale any time-dependent calculations (e.g., movement) by deltaTime.

            ball.Move(ballMoveSpeed);

            // Handle collision
            HandleBallCollision();
            // You can implement custom collision handling logic on top of the base HandleCollisionWith method.

        }

        public void Render()
        {
            // Render game objects onto the screen
            paddle1.Render(shader);
            paddle2.Render(shader);
            ball.Render(shader);
            blocker1.Render(shader);
            blocker2.Render(shader);
            sideCollider1.Render(shader);
            sideCollider2.Render(shader);
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
            sideCollider1.Cleanup();
            sideCollider2.Cleanup();

            // Shader cleanup
            shader.Cleanup();
        }
    }
}