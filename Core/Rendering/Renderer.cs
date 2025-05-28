using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Maths;
using System.Numerics;
using UntitledEngine.Core.Components;
using UntitledEngine.Core.Scenes;
using Shader = UntitledEngine.Core.Shader;
using ImGuiNET;
using Silk.NET.OpenGL.Extensions.ImGui;
using UntitledEngine.Core.Assets;
using UntitledEngine.Core.Input;
using UntitledEngine.Core.UI;

namespace UntitledEngine.Core.Rendering;

internal class Renderer
{
    public static GL Gl { get; private set; }
    
    private static Shader? shader;
    private static Matrix4x4 projection;
    
    private static Dictionary<MeshRenderer, (uint vao, uint vbo, uint ebo)> meshData = new();
    
    private static ImGuiController imguiController;
    private static EngineEditor editor;

    public static void OnLoad()
    {
        Gl = GL.GetApi(Engine.Instance.window);
        
        Gl.Enable(GLEnum.DepthTest);

        // Could also handle this part in scene
        string vertex = File.ReadAllText("Assets/Shaders/vertex_shader.glsl");
        string fragment = File.ReadAllText("Assets/Shaders/fragment_shader.glsl");
        shader = new Shader(vertex, fragment);
        Engine.Instance.Shader = shader;
        
        editor = new EngineEditor();
        imguiController = new ImGuiController(Gl, Engine.Instance.window, Engine.Instance.InputManager.InputContext);
        Engine.Instance.SceneManager.OnLoad();

        projection = Matrix4x4.CreateOrthographicOffCenter(-1f, 1f, -1f, 1f, 0.1f, 100f);
    }

    public static void OnResize(Vector2D<int> size)
    {
        if (Gl == null) return;

        Gl.Viewport(0, 0, (uint)size.X, (uint)size.Y);

        float aspect = size.X / (float)size.Y;
        projection = Matrix4x4.CreateOrthographicOffCenter(-aspect, aspect, -1f, 1f, 0.1f, 100f);
    }

    public unsafe static void OnRender(double deltaTime)
    {
        if (Gl == null || shader == null)
            return;

        var scene = Engine.Instance.SceneManager.CurrentScene;
        if (scene == null)
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            return;
        }

        Gl.Viewport(0, 0, (uint)Engine.Instance!.Width, (uint)Engine.Instance.Height);
        Gl.ClearColor(0f, 0f, 0f, 1f);
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        shader.Use();
        shader.SetColor(new Color(1f, 1f, 1f, 1f));
        shader.SetMatrix4("projection", projection);

        var cameraObject = scene.Entities.FirstOrDefault(go => go.GetComponent<Camera>() != null);
        var camera = cameraObject?.GetComponent<Camera>();
        shader.SetMatrix4("view", camera?.GetViewMatrix() ?? Matrix4x4.Identity);

        foreach (var go in scene.Entities)
        {
            var transform = go.Transform;
            var model = transform?.GetTransformMatrix() ?? Matrix4x4.Identity;
            shader.SetMatrix4("model", model);

            foreach (var meshRenderer in go.Components.OfType<MeshRenderer>())
            {
                // Bind texture
                meshRenderer.Texture?.Bind(TextureUnit.Texture0);
                shader.SetTexture("uTexture", 0);

                // Set mesh color
                shader.SetColor(meshRenderer.Color);

                // Bind VAO and draw
                if (!meshData.TryGetValue(meshRenderer, out var data))
                    continue;

                Gl.BindVertexArray(data.vao);
                Gl.DrawElements(PrimitiveType.Triangles, (uint)meshRenderer.Mesh.Indices.Length, DrawElementsType.UnsignedInt, (void*)0);
                Gl.BindVertexArray(0);
            }
        }
        
        imguiController.Update(Engine.Instance.DeltaTime);
        editor.UpdateUI();
        imguiController.Render();
    }
    
    public unsafe static void SetupMeshRenderer(MeshRenderer meshRenderer)
    {
        var gl = Gl;

        uint vao = gl.GenVertexArray();
        uint vbo = gl.GenBuffer();
        uint ebo = gl.GenBuffer();

        meshData[meshRenderer] = (vao, vbo, ebo);

        gl.BindVertexArray(vao);
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
        unsafe
        {
            fixed (float* vertexPtr = meshRenderer.Mesh.Vertices)
            {
                gl.BufferData(BufferTargetARB.ArrayBuffer,
                    (nuint)(meshRenderer.Mesh.Vertices.Length * sizeof(float)),
                    vertexPtr,
                    BufferUsageARB.StaticDraw);
            }
        }

        gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);
        unsafe
        {
            fixed (uint* indexPtr = meshRenderer.Mesh.Indices)
            {
                gl.BufferData(BufferTargetARB.ElementArrayBuffer,
                    (nuint)(meshRenderer.Mesh.Indices.Length * sizeof(uint)),
                    indexPtr,
                    BufferUsageARB.StaticDraw);
            }
        }

        gl.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, (uint)meshRenderer.Mesh.VertexStride, (void*)0);
        gl.EnableVertexAttribArray(0);
        gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, (uint)meshRenderer.Mesh.VertexStride, (void*)(2 * sizeof(float)));
        gl.EnableVertexAttribArray(1);

        gl.BindVertexArray(0);
    }
    
    public static void DestroyMeshRenderer(MeshRenderer meshRenderer)
    {
        if (meshData.TryGetValue(meshRenderer, out var data))
        {
            Gl.DeleteVertexArray(data.vao);
            Gl.DeleteBuffer(data.vbo);
            Gl.DeleteBuffer(data.ebo);
            meshData.Remove(meshRenderer);
        }
    }
}
