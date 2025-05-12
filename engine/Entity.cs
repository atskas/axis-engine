using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace UntitledEngine
{
    internal class Entity
    {
        private Mesh Mesh;
        public Vector3D<float> Size { get; set; }
        public Vector3D<float> Position { get; set; }
        public Vector4D<float> Color { get; set; }
        private Shader shader;

        public Entity((float, float, float) size, (float, float, float) position, (float, float, float, float) color, Shader shader)
        {
            this.Size = new Vector3D<float>(size.Item1, size.Item2, size.Item3);
            this.Position = new Vector3D<float>(position.Item1, position.Item2, position.Item3);
            this.Color = new Vector4D<float>(color.Item1, color.Item2, color.Item3, color.Item4);
            this.shader = shader;
            this.Mesh = CreateMesh(shader);
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

            uint[] indices = { 0, 1, 2, 2, 3, 0 };

            return new Mesh(vertices, indices, shader);
        }

        public void Move(Vector3D<float> delta)
        {
            Position += delta;
        }

        public void Resize(Vector3D<float> newSize)
        {
            Size = newSize;
        }

        public void Render(Shader shader)
        {
            shader.SetShapeColor(Color);
            Matrix4X4<float> model = Matrix4X4.CreateScale(Size) * Matrix4X4.CreateTranslation(Position);
            Mesh.Draw(model);
        }

        public bool CollidesWith(Entity other)
        {
            Vector3D<float> aMin = Position - Size * 0.5f;
            Vector3D<float> aMax = Position + Size * 0.5f;

            Vector3D<float> bMin = other.Position - other.Size * 0.5f;
            Vector3D<float> bMax = other.Position + other.Size * 0.5f;

            return
                aMin.X <= bMax.X && aMax.X >= bMin.X &&
                aMin.Y <= bMax.Y && aMax.Y >= bMin.Y;
        }

        public static Vector3D<float> CollisionResolve(Entity a, Entity b)
        {
            Vector3D<float> aMin = a.Position - a.Size * 0.5f;
            Vector3D<float> aMax = a.Position + a.Size * 0.5f;

            Vector3D<float> bMin = b.Position - b.Size * 0.5f;
            Vector3D<float> bMax = b.Position + b.Size * 0.5f;

            float dx = MathF.Min(aMax.X - bMin.X, bMax.X - aMin.X);
            float dy = MathF.Min(aMax.Y - bMin.Y, bMax.Y - aMin.Y);

            // Choose smallest axis to resolve on
            if (dx < dy)
                return new Vector3D<float>(a.Position.X < b.Position.X ? -dx : dx, 0, 0);
            else
                return new Vector3D<float>(0, a.Position.Y < b.Position.Y ? -dy : dy, 0);
        }

        public void Cleanup()
        {
            Mesh.Cleanup();
        }
    }
}
