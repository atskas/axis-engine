using Silk.NET.Input;
using Silk.NET.Maths;

namespace UntitledEngine
{
    internal class Scene
    {
        private Shader shader;
        private Shader shader2;

        private Entity paddle1;
        private Entity paddle2;
        private Entity wallTop;
        private Entity wallBottom;
        private Entity ball;

        private List<Entity> collidables;

        public Scene()
        {
            shader = new Shader();
            shader2 = new Shader();

            // Paddles
            paddle1 = new Entity(
                (0.1f, 1.1f, 1f),
                (-0.45f, 0.0f, 0.0f),
                (1f, 1f, 1f, 1f),
                shader
            );

            paddle2 = new Entity(
                (0.1f, 1.1f, 1f),
                (0.45f, 0.0f, 0.0f),
                (1f, 1f, 1f, 1f),
                shader
            );

            // Walls
            wallTop = new Entity(
                (1.0f, 0.1f, 1f),
                (0f, 0.55f, 0f),
                (1f, 1f, 1f, 1f),
                shader
            );

            wallBottom = new Entity(
                (1.0f, 0.1f, 1f),
                (0f, -0.55f, 0f),
                (1f, 1f, 1f, 1f),
                shader
            );

            // Ball
            ball = new Entity(
                (0.1f, 0.1f, 0.1f),
                (0.0f, 0.0f, 0.0f),
                (1f, 1f, 1f, 1f),
                shader2
            );

            collidables = new List<Entity>
            {
                paddle1,
                paddle2,
                wallTop,
                wallBottom
            };
        }

        public void ProcessInput(IKeyboard keyboard, float deltaTime)
        {
            float moveSpeed = 1.2f * deltaTime;

            if (keyboard.IsKeyPressed(Key.W))
                ball.Move(new Vector3D<float>(0f, moveSpeed, 0f));
            if (keyboard.IsKeyPressed(Key.S))
                ball.Move(new Vector3D<float>(0f, -moveSpeed, 0f));
            if (keyboard.IsKeyPressed(Key.A))
                ball.Move(new Vector3D<float>(-moveSpeed, 0f, 0f));
            if (keyboard.IsKeyPressed(Key.D))
                ball.Move(new Vector3D<float>(moveSpeed, 0f, 0f));

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
            shader.SetShapeColor(paddle1.Color);
            paddle1.Render(shader);

            shader.SetShapeColor(paddle2.Color);
            paddle2.Render(shader);

            shader.SetShapeColor(wallTop.Color);
            wallTop.Render(shader);

            shader.SetShapeColor(wallBottom.Color);
            wallBottom.Render(shader);

            shader.SetShapeColor(ball.Color);
            ball.Render(shader);
        }

        public void Update(float deltaTime)
        {
            // Time-based updates (currently unused)
        }

        public void Cleanup()
        {
            paddle1.Cleanup();
            paddle2.Cleanup();
            wallTop.Cleanup();
            wallBottom.Cleanup();
            ball.Cleanup();

            shader.Cleanup();
            shader2.Cleanup();
        }
    }
}
