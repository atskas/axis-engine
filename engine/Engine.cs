using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;

namespace UntitledEngine
{
    public class Engine : GameWindow
    {
        private Shader shader; // Shader class

        // Constructor to set up window size and title
        public Engine(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = new Vector2i(width, height), Title = title })
        {
            // Can do additional setup here
        }

        // Called once the window is loaded, I believe that's self-explanatory
        protected override void OnLoad()
        {
            // Set the background color (Temporary)
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            Console.WriteLine("Window loaded!");

            shader = new Shader();

            shader.SetShapeColor(1.0f, 1.0f, 1.0f, 1.0f);

            Resize += OnWindowResize;

        }

        protected override void OnUnload()
        {
            Console.WriteLine("Window unloaded!");

            base.OnUnload();

            shader.Cleanup();

        }

        private void OnWindowResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);

        }

        protected void ProcessInput()
        {
            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Space))
            {
                shader.SetShapeColor(1.0f, 0.0f, 0.0f, 1.0f);
            }
            else
            {
                shader.SetShapeColor(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }

        // Called every frame
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            // Input
            ProcessInput();
        }

        // Called every frame to render the content to the screen
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            // Clear the screen with the background color
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();
            shader.Draw();

            // Swap the buffers to display frame
            SwapBuffers();
        }
    }
}