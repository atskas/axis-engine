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

    // Private Fields
    private IWindow _window;
    private Matrix4x4 _projection;
    private float _accumulator = 0f;
    private int _width;
    private int _height;

    // Globals and managers
    public GL gl { get; private set; }
    public float DeltaTime { get; private set; } = 0f;
    public float FixedDeltaTime { get; private set; } = 1f / 60f;

    public readonly SceneManager SceneManager = new SceneManager();
    public readonly PhysicsManager PhysicsManager;
    public InputManager InputManager;

    public Shader Shader { get; private set; }

    // ImGui & Editor
    private ImGuiController _imguiController;
    private EngineEditor _editor;

    public Engine(int width, int height, string title)
    {
        _width = width;
        _height = height;

        Instance = this;

        // Create a window
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(width, height);
        options.Title = title;
        options.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Default, new APIVersion(4, 6));

        _window = Window.Create(options);

        // Hook into lifecycle events
        _window.Load += OnLoad;
        _window.Update += OnUpdateFrame;
        _window.Render += OnRenderFrame;
        _window.Resize += OnResize;

        _window.Load += Renderer.OnLoad;
        _window.Render += Renderer.OnRender;

        PhysicsManager = new PhysicsManager();
    }

    public void Run()
    {
        _window.Run();
    }

    private void OnLoad()
    {
        gl = GL.GetApi(_window);
        gl.Enable(GLEnum.DepthTest);

        Console.WriteLine("Working Directory: " + System.IO.Directory.GetCurrentDirectory());

        string vertex = File.ReadAllText("Assets/Shaders/vertex_shader.glsl");
        string fragment = File.ReadAllText("Assets/Shaders/fragment_shader.glsl");

        Shader = new Shader(vertex, fragment);
        InputManager = new InputManager(_window);
        _editor = new EngineEditor();
        _imguiController = new ImGuiController(gl, _window, InputManager.InputContext);

        // Setup orthographic projection matrix
        _projection = Matrix4x4.CreateOrthographicOffCenter(-1f, 1f, -1f, 1f, 0.1f, 100f);

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
        _accumulator += DeltaTime;

        InputManager.Update(); // Update InputManager early

        while (_accumulator >= FixedDeltaTime)
        {
            PhysicsManager.UpdatePhysics();
            _accumulator -= FixedDeltaTime;

            foreach (var entity in SceneManager.CurrentScene.Entities)
                entity.Transform.PreviousPosition = entity.Transform.Position;
        }

        foreach (var entity in SceneManager.CurrentScene.Entities)
        {
            foreach (var component in entity.Components)
                component.Update();
        }

        SceneManager.OnUpdate(DeltaTime);
        _imguiController.Update(DeltaTime);
    }

    private void OnRenderFrame(double deltaTime)
    {
        if (SceneManager.CurrentScene == null)
        {
            gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            return;
        }

        gl.Viewport(0, 0, (uint)_width, (uint)_height);
        gl.ClearColor(0f, 0f, 0f, 1f);
        gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Shader.Use();
        Shader.SetColor(new Vector4(1f, 1f, 1f, 1f));
        Shader.SetMatrix4("projection", _projection);

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
        _editor.UpdateUI();
        _imguiController.Render();
    }

    private void OnResize(Vector2D<int> size)
    {
        _width = size.X;
        _height = size.Y;

        gl.Viewport(0, 0, (uint)_width, (uint)_height);

        float aspect = _width / (float)_height;
        _projection = Matrix4x4.CreateOrthographicOffCenter(
            -aspect, aspect, -1f, 1f, 0.1f, 100f
        );
    }
}
