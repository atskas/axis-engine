using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using UntitledEngine.Core;
using UntitledEngine.Core.Components;
using UntitledEngine.Core.ECS;
using UntitledEngine.Core.Entities;
using UntitledEngine.Core.Scenes;

public class Engine : GameWindow
{
    public float deltaTime { get; private set; } = 0f;
    
    // Creates the singleton SceneManager
    SceneManager sceneManager = new SceneManager();

    private Shader shader;
    public Shader Shader => shader;
    private Matrix4 projection;
    
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

        string vertex = File.ReadAllText("Assets/Shaders/vertex_shader.glsl");
        string fragment = File.ReadAllText("Assets/Shaders/fragment_shader.glsl");

        shader = new Shader(vertex, fragment);

        // Projection (orthographic)
        projection = Matrix4.CreateOrthographicOffCenter(-1f, 1f, -1f, 1f, 0.1f, 100f);

        // Call Scene Start
        sceneManager.OnLoad();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        if (sceneManager.CurrentScene == null)
        {
            // Skip updating if no scene assigned yet
            return;
        }
        
        float deltaTime = (float)args.Time;

        sceneManager.OnUpdate(deltaTime);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        if (sceneManager.CurrentScene == null)
        {
            // Clear screen and swap buffers to avoid freezing
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SwapBuffers();
            return;
        }
        
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.ClearColor(Color4.Black);
        shader.Use();
        shader.SetColor(new Vector4(1f, 1f, 1f, 1f));

        // Set global uniforms
        shader.SetMatrix4("projection", projection);
        var cameraObject = sceneManager.CurrentScene.Entities
            .FirstOrDefault(go => go.GetComponent<Camera>() != null);

        var camera = cameraObject?.GetComponent<Camera>();
        shader.SetMatrix4("view", camera?.GetViewMatrix() ?? Matrix4.Identity);


        foreach (var go in sceneManager.CurrentScene.Entities)
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
