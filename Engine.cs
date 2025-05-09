using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;

namespace UntitledEngine
{
    public class Engine : GameWindow
    {
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
            GL.ClearColor(0.1f, 0.2f, 0.3f, 1.0f);
        }

        // Called every frame
        protected override void OnUpdateFrame(FrameEventArgs args)
        {

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