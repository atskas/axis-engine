using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;
using PixelFormat = Silk.NET.OpenGL.PixelFormat;
using PixelType = Silk.NET.OpenGL.PixelType;
using TextureMagFilter = Silk.NET.OpenGL.TextureMagFilter;
using TextureMinFilter = Silk.NET.OpenGL.TextureMinFilter;
using TextureParameterName = Silk.NET.OpenGL.TextureParameterName;
using TextureTarget = Silk.NET.OpenGL.TextureTarget;
using TextureUnit = Silk.NET.OpenGL.TextureUnit;
using TextureWrapMode = Silk.NET.OpenGL.TextureWrapMode;

namespace UntitledEngine.Core.Assets
{
    public class Texture : IDisposable
    {
        private uint handle;

        public Texture(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File {path} does not exist");

            using Image<Rgba32> image = Image.Load<Rgba32>(path);
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            var pixels = new byte[image.Width * image.Height * 4];
            image.CopyPixelDataTo(pixels);

            handle = Engine.Instance.Gl.GenTexture();
            Engine.Instance.Gl.BindTexture(TextureTarget.Texture2D, handle);

            unsafe
            {
                fixed (byte* pixelPtr = pixels)
                {
                    Engine.Instance.Gl.TexImage2D(TextureTarget.Texture2D,
                        0,
                        InternalFormat.Rgba,
                        (uint)image.Width,
                        (uint)image.Height,
                        0,
                        PixelFormat.Rgba,
                        PixelType.UnsignedByte,
                        pixelPtr);
                }
            }

            Engine.Instance.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            Engine.Instance.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            Engine.Instance.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            Engine.Instance.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            Engine.Instance.Gl.GenerateMipmap(GLEnum.Texture2D);
        }

        public void Bind(TextureUnit unit = TextureUnit.Texture0)
        {
            Engine.Instance.Gl.ActiveTexture(unit);
            Engine.Instance.Gl.BindTexture(TextureTarget.Texture2D, handle);
        }

        public void Dispose()
        {
            Engine.Instance.Gl.DeleteTexture(handle);
        }
    }

}
