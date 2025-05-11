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
        private Mesh paddle1;
        private Mesh paddle2;
        private Mesh ball;

        private Vector4 paddle1Color = Vector4.One;
        private Vector4 paddle2Color = Vector4.One;
        private Vector4 ballColor = Vector4.One;

        // Positions
        Vector3 paddle1Pos = new Vector3(-0.8f, 0.2f, 0.0f);
        Vector3 paddle2Pos = new Vector3(0.8f, 0.2f, 0.0f);
        Vector3 ballPos = new Vector3(0.0f, 0.0f, 0.0f);

        // Testing a restriction
        double maximumPaddleY = 0.65f;
        double minimumPaddleY = -0.65f;

        public Scene()
        {
            // Set up scene objects
            shader = new Shader();
            shader2 = new Shader();

            // Paddle object
            float[] paddleVertices = {
           -0.1f,  0.4f, 0.0f,
            0.1f,  0.4f, 0.0f,
            0.1f, -0.4f, 0.0f,
           -0.1f, -0.4f, 0.0f
            };

            int[] paddleIndices = { 0, 1, 2, 2, 3, 0 };
            paddle1 = new Mesh(paddleVertices, paddleIndices, shader);
            paddle2 = new Mesh(paddleVertices, paddleIndices, shader);

            // Ball object
            float[] ballVertices = {
           -0.05f + 0.5f,  0.05f, 0.0f,
            0.05f + 0.5f,  0.05f, 0.0f,
            0.05f + 0.5f, -0.05f, 0.0f,
           -0.05f + 0.5f, -0.05f, 0.0f
            };


            int[] ballIndices = { 0, 1, 2, 2, 3, 0 };
            ball = new Mesh(ballVertices, ballIndices, shader);
        }

        // Input handling
        public void ProcessInput(KeyboardState keyboardState)
        {
            // Handle paddle movement
            if (keyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.W))
            {
                if (paddle1Pos.Y <= maximumPaddleY)
                {
                    paddle1Pos.Y += 0.0005f;
                }
            }
            if (keyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.S))
            {
                if (paddle1Pos.Y >= minimumPaddleY)
                {
                    paddle1Pos.Y -= 0.0005f;
                }
            }

            // Handle paddle2 movement
            if (keyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Up))
            {
                if (paddle2Pos.Y <= maximumPaddleY)
                {
                    paddle2Pos.Y += 0.0005f;
                }
            }
            if (keyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Down))
            {
                if (paddle2Pos.Y >= minimumPaddleY)
                {
                    paddle2Pos.Y -= 0.0005f;
                }
            }
        }

        public void Render()
        {
            // Set colors and draw game objects onto the screen
            shader.SetShapeColor(paddle1Color);
            Matrix4 paddle1Model = Matrix4.CreateTranslation(paddle1Pos);
            paddle1.Draw(paddle1Model);

            shader.SetShapeColor(paddle2Color);
            Matrix4 paddle2Model = Matrix4.CreateTranslation(paddle2Pos);
            paddle2.Draw(paddle2Model);

            shader.SetShapeColor(ballColor);
            Matrix4 ballModel = Matrix4.CreateTranslation(ballPos);
            ball.Draw(ballModel);
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
