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

        public Scene()
        {
            // Set up scene objects
            shader = new Shader();
            shader2 = new Shader();

            // Paddles
            paddle1 = new Entity(new Vector3(0.1f, 1.1f, 1f), new Vector3(-0.45f, 0.0f, 0.0f), Vector4.One, shader);
            paddle2 = new Entity(new Vector3(0.1f, 1.1f, 1f), new Vector3(0.45f, 0.0f, 0.0f), Vector4.One, shader);

            // Horizontal walls / Collision testing scene
            wallTop = new Entity(new Vector3(1.0f, 0.1f, 1f), new Vector3(0f, 0.55f, 0f), Vector4.One, shader);
            wallBottom = new Entity(new Vector3(1.0f, 0.1f, 1f), new Vector3(0f, -0.55f, 0f), Vector4.One, shader);

            // ^ These two form a box / Collision testing scene / Revert changes for PONG

            // Ball (currently a player with 8-dir movement) / Collision testing scene
            ball = new Entity(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.0f, 0.0f, 0.0f), Vector4.One, shader2);
        }

        // Input handling
        public void ProcessInput(KeyboardState keyboardState)
        {
            // Vertical movement
            if (keyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.W))
            {
                if (!Entity.IsCollidingWith(ball, paddle1) && !Entity.IsCollidingWith(ball, wallTop))
                {
                    ball.Move(new Vector3(0f, 0.00035f, 0f));
                }
                else
                {
                    ball.Move(new Vector3(0f, -0.03f, 0f));
                    Console.WriteLine("Player and wall colliding!");
                }
            }

            if (keyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.S))
            {
                if (!Entity.IsCollidingWith(ball, paddle1) && !Entity.IsCollidingWith(ball, wallBottom))
                {
                    ball.Move(new Vector3(0f, -0.00035f, 0f));
                }
                else
                {
                    ball.Move(new Vector3(0f, 0.03f, 0f));
                    Console.WriteLine("Player and wall colliding!");
                }
            }

            // Horizontal movement
            if (keyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.A))
            {

                if (!Entity.IsCollidingWith(ball, paddle1) && !Entity.IsCollidingWith(ball, paddle2))
                {
                    ball.Move(new Vector3(-0.00035f, 0f, 0f));
                }
                else
                {
                    ball.Move(new Vector3(0.03f, 0f, 0f));
                    Console.WriteLine("Player and wall colliding!");
                }

            }

            if (keyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D))
            {
                if (!Entity.IsCollidingWith(ball, paddle1) && !Entity.IsCollidingWith(ball, paddle2))
                {
                    ball.Move(new Vector3(0.00035f, 0f, 0f));
                }
                else
                {
                    ball.Move(new Vector3(-0.03f, 0f, 0f));
                    Console.WriteLine("Player and wall colliding!");
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

        public void Update()
        {
            // Any code in this function runs every frame
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
