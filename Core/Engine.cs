using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using UntitledEngine.Core;
using UntitledEngine.Core.Components;
using UntitledEngine.Core.ECS;
using UntitledEngine.Core.Entities;
using UntitledEngine.Core.Physics;
using UntitledEngine.Core.Scenes;

public class Engine : GameWindow
{
    public float DeltaTime { get; private set; } = 0f;
    public float FixedDeltaTime { get;  private set; } = 1f / 60f; // 60 updates per second
    private float accumulator = 0f;
    
    
    SceneManager SceneManager = new SceneManager();
    public PhysicsManager PhysicsManager;

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
        
        // Create physics manager instance
        PhysicsManager = new PhysicsManager();
        
        // Call Scene Start
        SceneManager.OnLoad();
        
        // Call every component's start
        foreach (var entity in SceneManager.CurrentScene.Entities)
        {
            foreach (var component in entity.Components)
            {
                component.Start();
            }
        }
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        if (SceneManager.CurrentScene == null)
        {
            // Skip updating if no scene assigned yet
            return;
        }
        
        DeltaTime = (float)args.Time;
        accumulator += DeltaTime;
        
        while (accumulator >= FixedDeltaTime)
        {
            PhysicsManager.UpdatePhysics();
            accumulator -= FixedDeltaTime;
            
            // Set previous position for each entity
            foreach (var entity in SceneManager.CurrentScene.Entities)
            {
                entity.Transform.PreviousPosition = entity.Transform.Position;
            }
        }
        
        // Call every component's update
        foreach (var entity in SceneManager.CurrentScene.Entities)
        {
            foreach (var component in entity.Components)
            {
                component.Update();
            }
        }
        
        SceneManager.OnUpdate(DeltaTime);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        if (SceneManager.CurrentScene == null)
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
        var cameraObject = SceneManager.CurrentScene.Entities
            .FirstOrDefault(go => go.GetComponent<Camera>() != null);

        var camera = cameraObject?.GetComponent<Camera>();
        shader.SetMatrix4("view", camera?.GetViewMatrix() ?? Matrix4.Identity);

        // Call draw method for each mesh renderer
        foreach (var go in SceneManager.CurrentScene.Entities)
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

        float aspect = Size.X / (float)Size.Y;
        projection = Matrix4.CreateOrthographicOffCenter(
            -aspect, aspect, -1f, 1f, -1f, 1f
        );
    }
    
    // ---Conversion---
    
    // Convert Numerics Vector2 to OpenTK Vector2
    public static OpenTK.Mathematics.Vector2 ToOpenTK(System.Numerics.Vector2 v)
    {
        return new OpenTK.Mathematics.Vector2(v.X, v.Y);
    }
    
    // Convert OpenTK Vector2 to Numerics Vector2
    public static System.Numerics.Vector2 ToNumerics(OpenTK.Mathematics.Vector2 v)
    {
        return new System.Numerics.Vector2(v.X, v.Y);
    }

}
