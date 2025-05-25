using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using UntitledEngine.Common;
using UntitledEngine.Common.Components;
using UntitledEngine.Common.ECS;
using UntitledEngine.Common.Entities;

public class Engine : GameWindow
{
    public float deltaTime { get; private set; } = 0f;

    private Shader shader;
    public Shader Shader => shader;
    private Matrix4 projection;
    private GameObject cameraObject;
    private GameObject tony;

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

        string vertex = File.ReadAllText("Shaders/vertex_shader.glsl");
        string fragment = File.ReadAllText("Shaders/fragment_shader.glsl");

        shader = new Shader(vertex, fragment);

        // Projection (orthographic)
        projection = Matrix4.CreateOrthographicOffCenter(-1f, 1f, -1f, 1f, 0.1f, 100f);

        // Setup Camera
        cameraObject = new CameraObject();

        // Add test object
        tony = new Tony();

        // Call Start
        foreach (var go in GameObjects)
            foreach (var comp in go.Components)
                comp.Start();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        float deltaTime = (float)args.Time;

        foreach (var go in GameObjects)
            foreach (var comp in go.Components)
                comp.Update();

        // Input (currently here for testing)
        if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.W))
        {
            tony.Transform.Position += new Vector2(0f, deltaTime * 0.5f);
        }
        if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.S))
        {
            tony.Transform.Position += new Vector2(0f, deltaTime * -0.5f);
        }
        if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D))
        {
            tony.Transform.Position += new Vector2(deltaTime * 0.5f, 0f);
        }
        if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.A))
        {
            tony.Transform.Position += new Vector2(deltaTime * -0.5f, 0f);
        }
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        shader.Use();
        shader.SetColor(new Vector4(1f, 1f, 1f, 1f));

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
