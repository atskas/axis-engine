using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace UntitledEngine
{
    internal class Entity
    {
        private Mesh Mesh;
        public Vector3 Position {  get; set; }
        public Vector3 Size { get; set; }
        public Vector4 Color { get; set; }
        private Shader shader;

        public Entity(Vector3 size, Vector3 position, Vector4 color, Shader shader)
        {
            this.Size = size;
            this.Position = position;
            this.Color = color;
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

            int[] indices = { 0, 1, 2, 2, 3, 0 };

            return new Mesh(vertices, indices, shader);
        }

        public void Move(Vector3 delta)
        {
            Position += delta;
        }

        public void Resize(Vector3 newSize)
        {
            Size = newSize;
        }

        public void Render(Shader shader)
        {
            shader.SetShapeColor(Color);
            Matrix4 model = Matrix4.CreateScale(Size) * Matrix4.CreateTranslation(Position);
            Mesh.Draw(model);
        }

        public void Cleanup()
        {
            Mesh.Cleanup();
        }
    }
}
