using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using UntitledEngine.Common;
using UntitledEngine.Common.Components;
using UntitledEngine.Common.Entities;

public class Engine : GameWindow
{
    private Shader shader;
    private Matrix4 projection;
    private GameObject cameraObject;
    private GameObject object1;

    public static List<GameObject> GameObjects = new();

    public Engine(int width, int height, string title)
        : base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            ClientSize = new Vector2i(width, height),
            Title = title
        })
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(Color4.CornflowerBlue);
        GL.Enable(EnableCap.DepthTest);

        Console.WriteLine("Working Directory: " + System.IO.Directory.GetCurrentDirectory());

        string vertex = File.ReadAllText("shaders/vertex_shader.glsl");
        string fragment = File.ReadAllText("shaders/fragment_shader.glsl");

        shader = new Shader(vertex, fragment);

        // Projection (orthographic)
        projection = Matrix4.CreateOrthographicOffCenter(-1f, 1f, -1f, 1f, 0.1f, 100f);

        // Setup Camera
        cameraObject = new GameObject();
        var camera = new Camera();
        cameraObject.AddComponent(camera);
        GameObjects.Add(cameraObject);

        // Add test object
        object1 = new Object1();
        GameObjects.Add(object1);

        // Call Start
        foreach (var go in GameObjects)
            foreach (var comp in go.Components)
                comp.Start();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        foreach (var go in GameObjects)
            foreach (var comp in go.Components)
                comp.Update();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        shader.Use();
        shader.SetShaderColor(new Vector4(1f, 1f, 1f, 1f));

        // Set global uniforms
        shader.SetMatrix4("projection", projection);
        var camera = cameraObject.GetComponent<Camera>();
        shader.SetMatrix4("view", camera?.GetViewMatrix() ?? Matrix4.Identity);

        foreach (var go in GameObjects)
        {
            var transform = go.Transform;
            var model = transform?.GetTransformMatrix() ?? Matrix4.Identity;
            shader.SetMatrix4("model", model);

            foreach (var renderer in go.Components.OfType<MeshRenderer>())
                renderer.Draw();
        }

        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, Size.X, Size.Y);
        projection = Matrix4.CreateOrthographicOffCenter(-1f, 1f, -1f, 1f, -1f, 1f);
    }
}
