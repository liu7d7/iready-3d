using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;

namespace Why.Engine
{
    // taken from https://github.com/opentk/LearnOpenTK/blob/master/Common/Texture.cs
    public class Texture
    {

        private static int _active;
        private readonly int _handle;

        public readonly float invWidth;
        public readonly float invHeight;

        public static Texture loadFromFile(string path)
        {
            int handle = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);
            
#pragma warning disable CA1416
            using var image = new Bitmap(path);

            image.RotateFlip(RotateFlipType.RotateNoneFlipY);
            
            var data = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                image.Width,
                image.Height,
                0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapNearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture(handle, image.Width, image.Height);
#pragma warning restore CA1416
        }

        private Texture(int glHandle, int width, int height)
        {
            _handle = glHandle;
            invWidth = 1.0f / width;
            invHeight = 1.0f / height;
        }
        
        public void bind(TextureUnit unit)
        {
            if (_handle == _active)
            {
                return;
            }
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
            _active = _handle;
        }
        
        public static void unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            _active = 0;
        }
    }
}