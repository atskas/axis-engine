using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.Versioning;
using UntitledEngine.engine;

namespace UntitledEngine
{
    internal class Entity
    {
        private Mesh Mesh;

        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Vector4 Color { get; set; }

        public Vector2 Velocity { get; set; }
        private Vector2 prevPosition;

        private Shader shader;

        public Entity(Vector2 size, Vector2 position, Vector4 color, Shader shader)
        {
            this.Size = size;
            this.Position = position;
            this.Color = color;
            this.shader = shader;
            this.Mesh = CreateMesh(shader);
            this.prevPosition = position; // Previous position
            this.Velocity = Vector2.Zero; // Init velocity
        }

        // Creates a rectangle
        private Mesh CreateMesh(Shader shader)
        {
            float[] vertices = {
               -0.5f, 0.5f, 0.0f,
                0.5f, 0.5f, 0.0f,
                0.5f, -0.5f, 0.0f,
               -0.5f, -0.5f, 0.0f
            };

            int[] indices = { 0, 1, 2, 2, 3, 0 };

            return new Mesh(vertices, indices, shader);
        }

        public void Move(Vector2 velocity)
        {
            prevPosition = Position;
            Position += velocity * Engine.deltaTime; // Move the entity by the delta
            this.Velocity = (Position - prevPosition) / Engine.deltaTime;
        }

        public void Resize(Vector2 newSize)
        {
            Size = newSize; // Resize the entity
        }

        public void Render(Shader shader)
        {
            shader.Use();

            shader.SetMatrix4("projection", Engine.Projection);

            Matrix4 model = Matrix4.CreateScale(new Vector3(Size.X, Size.Y, 1)) *
                            Matrix4.CreateTranslation(new Vector3(Position.X, Position.Y, 0));

            shader.SetMatrix4("model", model);

            shader.SetShaderColor(this.Color);

            Mesh.Draw(); // just draw; model matrix is already sent
        }



        public bool CollidesWith(Entity other)
        {
            Vector2 aMin = Position - Size * 0.5f;
            Vector2 aMax = Position + Size * 0.5f;

            Vector2 bMin = other.Position - other.Size * 0.5f;
            Vector2 bMax = other.Position + other.Size * 0.5f;

            return
                aMin.X <= bMax.X && aMax.X >= bMin.X &&
                aMin.Y <= bMax.Y && aMax.Y >= bMin.Y;
        }

        public Vector2 HandleCollisionWith(Entity other)
        {
            if (!CollidesWith(other))
                return Vector2.Zero;

            Vector2 resolution = CollisionResolve(this, other);
            Position += resolution;
            return resolution;
        }

        public static Vector2 CollisionResolve(Entity a, Entity b)
        {
            Vector2 aMin = a.Position - a.Size * 0.5f;
            Vector2 aMax = a.Position + a.Size * 0.5f;

            Vector2 bMin = b.Position - b.Size * 0.5f;
            Vector2 bMax = b.Position + b.Size * 0.5f;

            float dx = MathF.Min(aMax.X - bMin.X, bMax.X - aMin.X);
            float dy = MathF.Min(aMax.Y - bMin.Y, bMax.Y - aMin.Y);

            // Choose smallest axis to resolve on (2D)
            if (dx < dy)
                return new Vector2(a.Position.X < b.Position.X ? -dx : dx, 0);
            else
                return new Vector2(0, a.Position.Y < b.Position.Y ? -dy : dy);
        }

        public void Cleanup()
        {
            Mesh.Cleanup();
        }
    }
}
