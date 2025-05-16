using OpenTK.Mathematics;

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
        }

        public override void Think(float deltaTime)
        {
            base.Think(deltaTime);
        }

        public virtual void Render(Shader shader)
        {
            shader.Use();
            shader.SetMatrix4("projection", Engine.Projection);

            Matrix4 model = Transform.GetTransformMatrix();
            shader.SetMatrix4("model", model);

            shader.SetShaderColor(Color);
            Mesh.Draw();
        }

        public virtual void Cleanup()
        {
            Mesh.Cleanup();
        }
    }
}
