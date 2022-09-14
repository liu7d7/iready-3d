using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Mathematics;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace Iready.Engine
{
    // taken from https://github.com/opentk/LearnOpenTK/blob/master/Common/Texture.cs
    public class Texture
    {
        private static int _active;
        private static readonly Dictionary<int, Texture> _TEXTURES = new();
        private readonly int _handle;

        public readonly float Width;
        public readonly float Height;

        public static Texture LoadFromFile(string path)
        {
            int handle = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);
            
#pragma warning disable CA1416
            using Bitmap image = new Bitmap(path);
            
            BitmapData data = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                image.Width,
                image.Height,
                0,
                PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapNearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture(handle, image.Width, image.Height);
#pragma warning restore CA1416
        }
        
        public static Texture LoadFromBuffer(byte[] buffer, int width, int height, PixelFormat format, PixelInternalFormat internalFormat, TextureMinFilter minFilter = TextureMinFilter.LinearMipmapLinear, TextureMagFilter magFilter = TextureMagFilter.Linear)
        {
            int handle = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);
            
#pragma warning disable CA1416
            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                internalFormat,
                width,
                height,
                0,
                format,
                PixelType.UnsignedByte,
                buffer);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture(handle, width, height);
#pragma warning restore CA1416
        }

        private Texture(int glHandle, int width, int height)
        {
            _handle = glHandle;
            Width = width;
            Height = height;
            _TEXTURES[glHandle] = this;
        }
        
        public void Bind(TextureUnit unit)
        {
            if (_handle == _active)
            {
                return;
            }
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
            _active = _handle;
        }
        
        public static void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            _active = 0;
        }

        public static Vector2 CurrentBounds()
        {
            if (!_TEXTURES.ContainsKey(_active))
            {
                return new Vector2(1, 1);
            }
            Texture current = _TEXTURES[_active];
            return new Vector2(current.Width, current.Height);
        }
    }
}