using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Maths;
using System.Numerics;
using UntitledEngine.Core;
using UntitledEngine.Core.Components;
using UntitledEngine.Core.Input;
using UntitledEngine.Core.Physics;
using UntitledEngine.Core.Scenes;
using Shader = UntitledEngine.Core.Shader;

// ImGUI
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL.Extensions.ImGui;
using UntitledEngine.Core.UI;
using UntitledEngine.Core.Renderer;

public class Engine
{
    // Singleton Instance
    public static Engine? Instance { get; private set; }

    // Privates
    private IWindow window;
    private Matrix4x4 projection;
    private float accumulator = 0f;
    private int width;
    private int height;

    // Publics
    public GL Gl { get; private set; }
    public float DeltaTime { get; private set; } = 0f;
    public float FixedDeltaTime { get; private set; } = 1f / 60f;

    public readonly SceneManager SceneManager = new SceneManager();
    public readonly PhysicsManager PhysicsManager;
    public InputManager InputManager;

    public Shader Shader { get; private set; }

    // ImGui & Editor
    private ImGuiController imguiController;
    private EngineEditor editor;

    public Engine(int width, int height, string title)
    {
        this.width = width;
        this.height = height;

        Instance = this;

        // Create a window
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(width, height);
        options.Title = title;
        options.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Default, new APIVersion(4, 6));

        window = Window.Create(options);

        // Hook into lifecycle events
        window.Load += OnLoad;
        window.Update += OnUpdateFrame;
        window.Render += OnRenderFrame;
        window.Resize += OnResize;

        window.Load += Renderer.OnLoad;
        window.Render += Renderer.OnRender;

        PhysicsManager = new PhysicsManager();
    }

    public void Run()
    {
        window.Run();
    }

    private void OnLoad()
    {
        Gl = GL.GetApi(window);
        Gl.Enable(GLEnum.DepthTest);

        Console.WriteLine("Working Directory: " + System.IO.Directory.GetCurrentDirectory());

        string vertex = File.ReadAllText("Assets/Shaders/vertex_shader.glsl");
        string fragment = File.ReadAllText("Assets/Shaders/fragment_shader.glsl");

        Shader = new Shader(vertex, fragment);
        InputManager = new InputManager(window);
        editor = new EngineEditor();
        imguiController = new ImGuiController(Gl, window, InputManager.InputContext);

        // Setup orthographic projection matrix
        projection = Matrix4x4.CreateOrthographicOffCenter(-1f, 1f, -1f, 1f, 0.1f, 100f);

        SceneManager.OnLoad();

        foreach (var entity in SceneManager.CurrentScene.Entities)
            foreach (var component in entity.Components)
                component.Start();
    }

    private void OnUpdateFrame(double deltaTime)
    {
        if (SceneManager.CurrentScene == null)
            return;

        DeltaTime = (float)deltaTime;
        accumulator += DeltaTime;

        InputManager.Update(); // Update InputManager early

        while (accumulator >= FixedDeltaTime)
        {
            PhysicsManager.UpdatePhysics();
            accumulator -= FixedDeltaTime;
        }

        foreach (var entity in SceneManager.CurrentScene.Entities)
        {
            foreach (var component in entity.Components)
                component.Update();
        }

        SceneManager.OnUpdate(DeltaTime);
        imguiController.Update(DeltaTime);
    }

    private void OnRenderFrame(double deltaTime)
    {
        if (SceneManager.CurrentScene == null)
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            return;
        }

        Gl.Viewport(0, 0, (uint)width, (uint)height);
        Gl.ClearColor(0f, 0f, 0f, 1f);
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Shader.Use();
        Shader.SetColor(new Vector4(1f, 1f, 1f, 1f));
        Shader.SetMatrix4("projection", projection);

        var cameraObject = SceneManager.CurrentScene.Entities.FirstOrDefault(go => go.GetComponent<Camera>() != null);
        var camera = cameraObject?.GetComponent<Camera>();
        Shader.SetMatrix4("view", camera?.GetViewMatrix() ?? Matrix4x4.Identity);

        foreach (var go in SceneManager.CurrentScene.Entities)
        {
            var transform = go.Transform;
            var model = transform?.GetTransformMatrix() ?? Matrix4x4.Identity;
            Shader.SetMatrix4("model", model);

            foreach (var renderer in go.Components.OfType<MeshRenderer>())
                renderer.Draw();
        }

        // ImGui UI
        editor.UpdateUI();
        imguiController.Render();
    }

    private void OnResize(Vector2D<int> size)
    {
        width = size.X;
        height = size.Y;

        Gl.Viewport(0, 0, (uint)width, (uint)height);

        float aspect = width / (float)height;
        projection = Matrix4x4.CreateOrthographicOffCenter(
            -aspect, aspect, -1f, 1f, 0.1f, 100f
        );
    }
}
