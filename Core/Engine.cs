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
using UntitledEngine.Core.Rendering;

internal class Engine
{
    // Singleton Instance
    public static Engine? Instance { get; private set; }

    // Privates
    private float accumulator = 0f;

    // Publics
    public float DeltaTime { get; private set; } = 0f;
    public float FixedDeltaTime { get; private set; } = 1f / 60f;
    public int Width;
    public int Height;
    public IWindow window;

    public readonly SceneManager SceneManager = new();
    public readonly PhysicsManager PhysicsManager;
    public InputManager InputManager;

    public Shader Shader { get; set; }

    public Engine(int width, int height, string title)
    {
        Instance = this;

        this.Width = width;
        this.Height = height;
        
        // Create a window
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(width, height);
        options.Title = title;
        options.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Default, new APIVersion(4, 6));

        window = Window.Create(options);

        // Hook into lifecycle events
        window.Load += OnLoad;
        window.Render += Renderer.OnRender;
        window.Resize += Renderer.OnResize;
        
        window.Update += OnUpdateFrame;
        window.Resize += OnResize;

        PhysicsManager = new PhysicsManager();
    }

    public void Run()
    {
        window.Run();
    }

    private void OnLoad()
    {
        InputManager = new InputManager(window);
        
        Renderer.OnLoad();
        Console.WriteLine("Working Directory: " + System.IO.Directory.GetCurrentDirectory());
        
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
    }

    private void OnResize(Vector2D<int> size)
    {
        Width = size.X;
        Height = size.Y;

        Renderer.OnResize(size);
    }
}
