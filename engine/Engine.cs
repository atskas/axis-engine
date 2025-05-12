using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Input;
using System;
using Silk.NET.Vulkan;

namespace UntitledEngine
{
    internal static class GLContext
    {
        public static GL Instance { get; private set; }

        // Initialise GL context, called once during the setup
        public static void Init(GL gl)
        {
            Instance = gl;
        }
    }

    public class Engine
    {
        private IWindow window;
        private GL gl;
        private IInputContext input;
        private IKeyboard keyboard;

        private Scene scene;

        public Engine(int  width, int height, string title)
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(width, height);
            options.Title = title;
            options.VSync = true;

            window = Window.Create(options);

            // Hook up event methods
            window.Load += OnLoad;
            window.Render += OnRender;
            window.Update += OnUpdate;
            window.FramebufferResize += OnResize;
            window.Closing += OnUnload;
        }

        public void Run()
        {
            window.Run();
        }

        private void OnLoad()
        {
            // Load OpenGL API
            gl = GL.GetApi(window);
            Console.WriteLine("GL gotten");

            // Initialise GLContext with the OpenGL context
            GLContext.Init(gl);

            // Setup input
            input = window.CreateInput();
            if (input.Keyboards.Count > 0)
                keyboard = input.Keyboards[0];

            gl.ClearColor(0f, 0f, 0f, 1f);

            scene = new Scene();
        }

        private void OnResize(Vector2D<int> size)
        {
            gl.Viewport(0, 0, (uint)size.X, (uint)size.Y);
        }
        

        private void OnUnload()
        {
            Console.WriteLine("Window unloaded!");
            scene.Cleanup();
        }

        // Called every frame
        private void OnUpdate(double deltaTime)
        {
            scene.Update((float)deltaTime);

            if (keyboard != null)
                scene.ProcessInput(keyboard, (float)deltaTime);

        }

        // Called every frame to render the content to the screen
        private void OnRender(double deltaTime)
        {
            // Clear the screen with the background color
            gl.Clear((uint)ClearBufferMask.ColorBufferBit);

            // Render everything in scene
            scene.Render();

            // Swap the buffers to display the frame
            window.SwapBuffers();
        }
    }
}
