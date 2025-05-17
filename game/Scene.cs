using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using UntitledEngine.engine;
using UntitledEngine.engine.entities;
using System.IO;

namespace UntitledEngine
{
    internal class Scene
    {
        private Shader shader;

        // Collidable objects
        private List<BaseEntity> collidables;

        // Game Objects
        private PaddleEntity paddle1;
        private PaddleEntity paddle2;
        private BallEntity ball;
        private WallEntity blocker1;    // Blockers (primarily used for making the ball bounce from the ceiling and limiting paddle Y)
        private WallEntity blocker2;
        private WallEntity sideCollider1;   // Side colliders (primarily used for detecting when a paddle misses the ball)
        private WallEntity sideCollider2;

        public Scene()
        {
            // Set up scene objects
            string vertexShaderSource = File.ReadAllText("shaders/vertex_shader.glsl");
            string fragmentShaderSource = File.ReadAllText("shaders/fragment_shader.glsl");
            shader = new Shader(vertexShaderSource, fragmentShaderSource);

            // Paddles
            paddle1 = new PaddleEntity(shader, new Vector2(-0.85f, 0f));
            paddle2 = new PaddleEntity(shader, new Vector2(0.85f, 0f));

            // Walls (to avoid paddles from going out of screen)
            // You could also do this by setting a restriction to the Y position and
            // stopping movement once that restriction is met
            blocker1 = new WallEntity(shader, (0f, 1f), (5f, 0.2f));
            blocker2 = new WallEntity(shader, (0f, -1f), (5f, 0.2f));

            sideCollider1 = new WallEntity(shader, (1f, 0f), (0.2f, 5f));
            sideCollider2 = new WallEntity(shader, (-1f, 0f), (0.2f, 5f));

            // Ball
            ball = new BallEntity(shader);

            // Set up collidables (Add collidable objects to this list)
            collidables = new List<BaseEntity>
            {
                paddle1,
                paddle2,
                blocker1,
                blocker2,
                sideCollider1,
                sideCollider2,
                ball
            };
        }

        // Input handling
        public void ProcessInput(KeyboardState keyboardState)
        {
            paddle1.ProcessInput(keyboardState, collidables, true);
            paddle2.ProcessInput(keyboardState, collidables, false);
        }

        public void Update(float deltaTime)
        {
            // This function runs every frame. To ensure smooth and consistent behavior across different frame rates, 
            // scale any time-dependent calculations (e.g., movement) by deltaTime.
            
            // Handle side collisions to trigger ball reset
            if (ball.ballPhysics.CollidesWith(sideCollider1))
            {
                Console.WriteLine("P1 lost!");
                ball.Reset();
            }
            else if (ball.ballPhysics.CollidesWith(sideCollider2))
            {
                Console.WriteLine("P2 lost!");
                ball.Reset();
            }


            // Non-player paddle AI
            if (ball.ballPhysics.Velocity.X > 0f)
            {
                if (ball.Transform.Position.Y > paddle2.Transform.Position.Y)
                    paddle2.paddlePhysics.Move(paddle2.playerMoveSpeed);
                else if (ball.Transform.Position.Y < paddle2.Transform.Position.Y)
                    paddle2.paddlePhysics.Move(-paddle2.playerMoveSpeed);
            }
            // Run main ball loop
            ball.BallLoop(deltaTime, collidables);
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
