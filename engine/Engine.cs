using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace UntitledEngine
{
    public class Engine : GameWindow
    {

        public static float deltaTime; // Make dt global

        private Scene scene;

        // Constructor to set up window size and title
        public Engine(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = new Vector2i(width, height), Title = title })
        {
        }

        // Called once the window is loaded
        protected override void OnLoad()
        {
            // Set the background clear color
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            scene = new Scene(); // Initialise game scene
            Resize += OnWindowResize;
        }

        protected override void OnUnload()
        {
            Console.WriteLine("Window unloaded!");

            base.OnUnload();
            scene.Cleanup();
        }

        private void OnWindowResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        // Called every frame
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            deltaTime = (float)args.Time;

            KeyboardState keyboardState = KeyboardState;

            scene.Update(deltaTime);

            scene.ProcessInput(keyboardState);
        }

        // Called every frame to render the content to the screen
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            // Clear the screen with the background color
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Render everything in scene
            scene.Render();

            // Swap the buffers to display the frame
            SwapBuffers();
        }
    }
}
