using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace UntitledEngine
{
    internal class Scene
    {
        private Shader shader; // You can also use separate shaders for objects

        // Game Objects
        private Entity paddle1;
        private Entity paddle2;

        private Entity ball;

        // Blockers (primarily used for making the ball bounce from the ceiling and limiting paddle Y)
        private Entity blocker1;
        private Entity blocker2;

        // Side colliders (primarily used for detecting when a paddle misses the ball)
        private Entity sideCollider1;
        private Entity sideCollider2;

        // Collidable objects
        private List<Entity> collidables;

        // Game stuff (Not Required)
        public Vector2 ballMoveSpeed = new Vector2(1f, 0.5f);

        public Scene()
        {
            // Set up scene objects
            shader = new Shader();

            // Paddles
            paddle1 = new Entity((0.1f, 0.65f), (-0.85f, 0.0f), Vector4.One, shader);
            paddle2 = new Entity((0.1f, 0.65f), (0.85f, 0.0f), Vector4.One, shader);

            // Walls (to avoid paddles from going out of screen)
            // You could also do this by setting a restriction to the Y position and
            // stopping movement once that restriction is met
            blocker1 = new Entity((5f, 0.2f), (0.0f, 1.1f), (0.0f, 0.0f, 0.0f, 0.0f), shader);
            blocker2 = new Entity((5f, 0.2f), (0.0f, -1.1f), (0.0f, 0.0f, 0.0f, 0.0f), shader);

            sideCollider1 = new Entity((0.2f, 5f), (1.1f, 0.0f), (0.0f, 0.0f, 1.0f, 1.0f), shader);

            // Ball
            ball = new Entity((0.1f, 0.1f), (0.0f, 0.0f), Vector4.One, shader);

            // Set up collidables (Add collidable objects to this list)
            collidables = new List<Entity>
            {
                paddle1,
                paddle2,
                blocker1,
                blocker2,
                sideCollider1,
            };
        }

        // Input handling
        public void ProcessInput(KeyboardState keyboardState, float deltaTime)
        {
            float moveSpeed = 1.3f * deltaTime;

            // p1
            if (keyboardState.IsKeyDown(Keys.W))
                paddle1.Move((0f, moveSpeed));
            if (keyboardState.IsKeyDown(Keys.S))
                paddle1.Move((0f, -moveSpeed));
            // You would do this for 8-Directional movement.
            // if (keyboardState.IsKeyDown(Keys.A))
            // player.Move(new Vector3(-moveSpeed, 0f));
            // if (keyboardState.IsKeyDown(Keys.D))
            // player.Move(new Vector3(moveSpeed, 0f));

            //p2
            if (keyboardState.IsKeyDown(Keys.Up))
                paddle2.Move((0f, moveSpeed));
            if (keyboardState.IsKeyDown(Keys.Down))
                paddle2.Move((0f, -moveSpeed));

            // Resolve collisions against everything
            foreach (var entity in collidables)
            {
                if (entity != paddle1 && paddle1.CollidesWith(entity)) // Probably going to make the self-check automatic at some point
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
            sideCollider1.Render(shader);

        }

        public void Update(float deltaTime)
        {
            // This function runs every frame. To ensure smooth and consistent behavior across different frame rates, 
            // scale any time-dependent calculations (e.g., movement) by deltaTime.

            ball.Move(ballMoveSpeed * deltaTime);
            Console.WriteLine(ball.Position);

            // Resolve collisions against everything
            foreach (var entity in collidables)
            {
                if (entity != ball && ball.CollidesWith(entity))
                {
                    // Get the direction of the collision
                    Vector2 resolution = Entity.CollisionResolve(ball, entity);

                    ball.Move(resolution); // Move out of overlap

                    bool isVertical = Math.Abs(ball.Position.X - entity.Position.X) > Math.Abs(ball.Position.Y - entity.Position.Y);

                    if (isVertical)
                    {
                        ballMoveSpeed.X *= -1; // Bounce horizontally
                        ballMoveSpeed.Y *= -1;
                    }
                    else
                    {
                        ballMoveSpeed.Y *= -1; // Bounce vertically
                        ballMoveSpeed.X *= -1;
                    }
                }
            }
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

            // Shader cleanup
            shader.Cleanup();
        }
    }
}
