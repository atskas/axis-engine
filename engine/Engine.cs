using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace UntitledEngine
{
    public class Engine : GameWindow
    {
        // Global projection matrix and delta time accessible throughout the engine
        public static Matrix4 Projection;
        public static float deltaTime { get; private set; }

        private Scene scene;

        // Constructor: Initializes window with given width, height, and title
        public Engine(int width, int height, string title)
            : base(GameWindowSettings.Default,
                   new NativeWindowSettings() { Size = new Vector2i(width, height), Title = title })
        {
            // Initialize the orthographic projection matrix with origin centered
            Projection = Matrix4.CreateOrthographicOffCenter(-1, 1, -1, 1, -1, 1);
        }

        // Called once when the window finishes loading
        protected override void OnLoad()
        {
            GL.ClearColor(0f, 0f, 0f, 1f); // Set background color to black

            scene = new Scene();  // Initialize game scene
            Resize += OnWindowResize; // Register resize event handler
        }

        // Called when the window is resized
        private void OnWindowResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);

            Projection = Matrix4.CreateOrthographicOffCenter(-1, 1, -1, 1, -1, 1);
        }


        // Called every frame to update game logic and process input
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            deltaTime = (float)args.Time;

            // Capture current keyboard state
            KeyboardState keyboardState = KeyboardState;

            scene.Update(deltaTime);
            scene.ProcessInput(keyboardState);
        }

        // Called every frame to render graphics
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            scene.Render(); // Render all scene objects

            SwapBuffers();  // Display rendered frame
        }

        // Called when window is closing/unloading resources
        protected override void OnUnload()
        {
            Console.WriteLine("Window unloaded!");
            scene.Cleanup();

            base.OnUnload();
        }
    }
}
