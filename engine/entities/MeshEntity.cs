using OpenTK.Mathematics;
using System.Drawing;

namespace UntitledEngine.engine.entities
{
    public class MeshEntity : BaseEntity
    {
        public Mesh Mesh { get; private set; }
        public Vector4 Color { get; set; } = Vector4.One;

        public MeshEntity(Mesh mesh)
        {
            Mesh = mesh;
        }

        public override void OnLoad()
        {
            base.OnLoad();
            base.Transform.Position = new Vector2(0, 0);
            base.Transform.Scale = new Vector2(0.15f,0.15f);
            base.Transform.Rotation = 45f;
        }

        public override void Think(float deltaTime)
        {
            base.Think(deltaTime);
        }

        protected override void Draw(Shader shader)
        {
            shader.SetShaderColor(Color);
            Mesh.Draw();
        }

        public override void Cleanup()
        {
            Mesh.Cleanup();
        }
    }
}
