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

        private float[] vertices = {
            // X, Y, Z
            0.0f, 0.5f, 0.0f, // Top
            -0.5f, -0.5f, 0.0f, // Bottom Left
            0.5f, -0.5f, 0.0f // Bottom right
        };

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

            Resize += OnWindowResize;

        }

        protected override void OnUnload()
        {
            Console.WriteLine("Window unloaded!");

            // Can add more cleanup as i start adding stuff
            base.OnUnload();
        }

        private void OnWindowResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);

            Console.WriteLine("Window resized!");
        }

        protected void ProcessInput()
        {
            if (KeyboardState.IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Space))
            {
                Console.WriteLine("Space key pressed!");
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

            // Swap the buffers to display frame
            SwapBuffers();
        }
    }
}