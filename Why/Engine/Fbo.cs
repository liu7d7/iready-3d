using OpenTK.Graphics.OpenGL4;

namespace Why.Engine
{
    public class Fbo
    {

        private static int _active;
        private static readonly Dictionary<int, Fbo> _frames = new();
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
            init();
            _frames[_handle] = this;
        }

        public Fbo(int width, int height, bool useDepth)
        {
            _width = width;
            _height = height;
            _useDepth = useDepth;
            _handle = -1;
            init();
            _frames[_handle] = this;
        }

        private void dispose()
        {
            GL.DeleteFramebuffer(_handle);
            GL.DeleteTexture(_colorAttachment);
            if (_useDepth)
            {
                GL.DeleteTexture(_depthAttachment);
            }
        }

        private void init()
        {
            if (_handle != -1)
            {
                dispose();
            }
            _handle = GL.GenFramebuffer();
            bind();
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

            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != FramebufferErrorCode.FramebufferComplete)
            {
                throw new Exception($"Incomplete Framebuffer! {status} should be {FramebufferErrorCode.FramebufferComplete}");
            }
            unbind();
        }

        private void _resize(int width, int height)
        {
            _width = width;
            _height = height;
            init();
        }

        public static void resize(int width, int height)
        {
            foreach (var frame in _frames)
            {
                frame.Value._resize(width, height);
            }
        }
        
        public void bind()
        {
            if (_handle == _active)
            {
                return;
            }
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _handle);
            _active = _handle;
        }
        
        public void blit()
        {
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _handle);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            GL.BlitFramebuffer(0, 0, _width, _height, 0, 0, _width, _height, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
            unbind();
        }

        public static void unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            _active = 0;
        }
        
    }
}
