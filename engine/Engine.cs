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
        private Shader shader2;
        private Mesh mesh1;
        private Mesh mesh2;

        private Vector4 object1Color = Vector4.One;
        private Vector4 object2Color = Vector4.One;


        // Constructor to set up window size and title
        public Engine(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = new Vector2i(width, height), Title = title })
        {
            // Can do additional setup here
        }

        // Called once the window is loaded
        protected override void OnLoad()
        {
            // Set the background clear color
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            Console.WriteLine("Window loaded!");

            // Set up shared index data
            int[] indices = {
                0, 1, 2,
                2, 3, 0
            };

            // Create first mesh
            shader = new Shader();

            float[] vertices2 = {
               -0.75f,  0.3f, 0.0f,
               -0.75f, -0.3f, 0.0f,
               -0.8f, -0.3f, 0.0f,
               -0.8f,  0.3f, 0.0f
            };

            mesh2 = new Mesh(vertices2, indices, shader);

            shader2 = new Shader();

            // Create second mesh
            float[] vertices1 = {
                0.05f,  0.05f, 0.0f, // top right
                0.05f, -0.05f, 0.0f, // bottom right
               -0.05f, -0.05f, 0.0f, // bottom left
               -0.05f,  0.05f, 0.0f  // top left
            };

            mesh1 = new Mesh(vertices1, indices, shader2);

            // Handle window resizing
            Resize += OnWindowResize;
        }

        protected override void OnUnload()
        {
            Console.WriteLine("Window unloaded!");

            base.OnUnload();

            shader.Cleanup();
            mesh1.Cleanup();
            mesh2.Cleanup(); // Cleanup the second mesh
        }

        private void OnWindowResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected void ProcessInput()
        {
            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Space))
            {
                object2Color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
            }
            else
            {
                object2Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
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

            // Draw the first mesh
            shader.SetShapeColor(object1Color);
            mesh1.Draw();

            // Draw the second mesh 
            shader2.SetShapeColor(object2Color);
            mesh2.Draw();

            // Swap the buffers to display the frame
            SwapBuffers();
        }
    }
}
