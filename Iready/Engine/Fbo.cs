using OpenTK.Graphics.OpenGL4;

namespace Iready.Engine
{
    public class Fbo
    {

        private static int _active;
        private static readonly Dictionary<int, Fbo> _FRAMES = new();
        private readonly bool _useDepth;
        private int _handle;
        private int _colorAttachment;
        private int _depthAttachment;
        private int _width;
        private int _height;

        public Fbo(int width, int height)
        {
            _width = width;
            _height = height;
            _useDepth = false;
            _handle = -1;
            Init();
            _FRAMES[_handle] = this;
        }

        public Fbo(int width, int height, bool useDepth)
        {
            _width = width;
            _height = height;
            _useDepth = useDepth;
            _handle = -1;
            Init();
            _FRAMES[_handle] = this;
        }

        private void Dispose()
        {
            GL.DeleteFramebuffer(_handle);
            GL.DeleteTexture(_colorAttachment);
            if (_useDepth)
            {
                GL.DeleteTexture(_depthAttachment);
            }
        }

        private void Init()
        {
            if (_handle != -1)
            {
                Dispose();
            }

            _handle = GL.GenFramebuffer();
            Bind();
            _colorAttachment = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _colorAttachment);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToBorder);
            GL.TexStorage2D(TextureTarget2d.Texture2D, 1, SizedInternalFormat.Rgba8, _width, _height);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _colorAttachment, 0);
            if (_useDepth)
            {
                _depthAttachment = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, _depthAttachment);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareMode, (int) TextureCompareMode.None);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, _width, _height, 0, PixelFormat.DepthComponent, PixelType.UnsignedInt, IntPtr.Zero);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, _depthAttachment, 0);
            }

            FramebufferErrorCode status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != FramebufferErrorCode.FramebufferComplete)
            {
                throw new Exception($"Incomplete Framebuffer! {status} should be {FramebufferErrorCode.FramebufferComplete}");
            }

            Unbind();
        }

        private void _resize(int width, int height)
        {
            _width = width;
            _height = height;
            Init();
        }

        public static void Resize(int width, int height)
        {
            foreach (KeyValuePair<int, Fbo> frame in _FRAMES)
            {
                frame.Value._resize(width, height);
            }
        }

        public void BindColor(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _colorAttachment);
        }
        
        public int BindDepth(TextureUnit unit)
        {
            if (!_useDepth)
            {
                throw new Exception("Trying to bind depth texture of a framebuffer without depth!");
            }
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _depthAttachment);
            return _depthAttachment;
        }

        public void Bind()
        {
            if (_handle == _active)
            {
                return;
            }
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _handle);
            _active = _handle;
        }
        
        public void Blit()
        {
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _handle);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            GL.BlitFramebuffer(0, 0, _width, _height, 0, 0, _width, _height, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
            Unbind();
        }

        public static void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            _active = 0;
        }
        
    }
}
