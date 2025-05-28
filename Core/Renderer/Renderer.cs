using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Maths;
using System.Numerics;
using UntitledEngine.Core.Components;
using UntitledEngine.Core.Scenes;
using Shader = UntitledEngine.Core.Shader;
using ImGuiNET;
using Silk.NET.OpenGL.Extensions.ImGui;
using UntitledEngine.Core.Input;
using UntitledEngine.Core.UI;

namespace UntitledEngine.Core.Renderer;

internal class Renderer
{
    private static Shader? shader;
    private static Matrix4x4 projection;
    
    private static ImGuiController imguiController;
    private static EngineEditor editor;

    public static void OnLoad()
    {
        Engine.Instance.Gl.Enable(GLEnum.DepthTest);

        // Could also handle this part in scene
        string vertex = File.ReadAllText("Assets/Shaders/vertex_shader.glsl");
        string fragment = File.ReadAllText("Assets/Shaders/fragment_shader.glsl");
        shader = new Shader(vertex, fragment);
        Engine.Instance.Shader = shader;
        
        editor = new EngineEditor();
        imguiController = new ImGuiController(Engine.Instance.Gl, Engine.Instance.window, Engine.Instance.InputManager.InputContext);

        projection = Matrix4x4.CreateOrthographicOffCenter(-1f, 1f, -1f, 1f, 0.1f, 100f);
    }

    public static void OnResize(Vector2D<int> size)
    {
        if (Engine.Instance.Gl == null) return;

        Engine.Instance.Gl.Viewport(0, 0, (uint)size.X, (uint)size.Y);

        float aspect = size.X / (float)size.Y;
        projection = Matrix4x4.CreateOrthographicOffCenter(-aspect, aspect, -1f, 1f, 0.1f, 100f);
    }

    public static void OnRender(double deltaTime)
    {
        if (Engine.Instance.Gl == null || shader == null)
            return;

        var scene = Engine.Instance.SceneManager.CurrentScene;
        if (scene == null)
        {
            Engine.Instance.Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            return;
        }

        Engine.Instance.Gl.Viewport(0, 0, (uint)Engine.Instance!.Width, (uint)Engine.Instance.Height);
        Engine.Instance.Gl.ClearColor(0f, 0f, 0f, 1f);
        Engine.Instance.Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        shader.Use();
        shader.SetColor(new Vector4(1f, 1f, 1f, 1f));
        shader.SetMatrix4("projection", projection);

        var cameraObject = scene.Entities.FirstOrDefault(go => go.GetComponent<Camera>() != null);
        var camera = cameraObject?.GetComponent<Camera>();
        shader.SetMatrix4("view", camera?.GetViewMatrix() ?? Matrix4x4.Identity);

        foreach (var go in scene.Entities)
        {
            var transform = go.Transform;
            var model = transform?.GetTransformMatrix() ?? Matrix4x4.Identity;
            shader.SetMatrix4("model", model);

            foreach (var renderer in go.Components.OfType<MeshRenderer>())
                renderer.Draw();
        }
        
        imguiController.Update(Engine.Instance.DeltaTime);
        editor.UpdateUI();
        imguiController.Render();
    }
}
