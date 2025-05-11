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
        private Entity ball;

        // Testing a restriction
        double maximumPaddleY = 0.65f;
        double minimumPaddleY = -0.65f;

        public Scene()
        {
            // Set up scene objects
            shader = new Shader();
            shader2 = new Shader();

            // Create paddle objects
            paddle1 = new Entity(new Vector3(0.1f, 0.8f, 1f), new Vector3(-0.8f, 0.2f, 0.0f), Vector4.One, shader);
            paddle2 = new Entity(new Vector3(0.1f, 0.8f, 1f), new Vector3(0.8f, 0.2f, 0.0f), Vector4.One, shader);
            ball = new Entity(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.0f, 0.0f, 0.0f), Vector4.One, shader2);
        }

        // Input handling
        public void ProcessInput(KeyboardState keyboardState)
        {
            // Handle paddle movement
            if (keyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.W))
            {
                if (paddle1.Position.Y <= maximumPaddleY)
                {
                    paddle1.Move(new Vector3(0f, 0.0005f, 0f));
                }
            }
            if (keyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.S))
            {
                if (paddle1.Position.Y >= minimumPaddleY)
                {
                    paddle1.Move(new Vector3(0f, -0.0005f, 0f));
                }
            }

            // Handle paddle2 movement
            if (keyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Up))
            {
                if (paddle2.Position.Y <= maximumPaddleY)
                {
                    paddle2.Move(new Vector3(0f, 0.0005f, 0f));
                }
            }
            if (keyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Down))
            {
                if (paddle2.Position.Y >= minimumPaddleY)
                {
                    paddle2.Move(new Vector3(0f, -0.0005f, 0f));
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
            ball.Cleanup();

            // Shader cleanup
            shader.Cleanup();
            shader2.Cleanup();
        }
    }
}
