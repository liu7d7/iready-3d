using Iready.Shared;
using OpenTK.Graphics.OpenGL4;
using StbTrueTypeSharp;

namespace Iready.Engine
{

    public class Font
    {
        
        private const float _IPW = 1.0f / 2048f;
        private const float _IPH = _IPW;

        public int Height;
        private float _ascent;
        private StbTrueType.stbtt_packedchar[] _chars;
        public Texture Texture;

        // I have absolutely no idea how to use unsafe :((
        public unsafe Font(byte[] buffer, int height)
        {
            Height = height;
            
            StbTrueType.stbtt_fontinfo fontInfo = StbTrueType.CreateFont(buffer, 0);
            
            _chars = new StbTrueType.stbtt_packedchar[256];
            StbTrueType.stbtt_pack_context packContext = new();
            
            byte[] bitmap = new byte[2048 * 2048];
            fixed (byte* dat = bitmap)
            {
                StbTrueType.stbtt_PackBegin(packContext, dat, 2048, 2048, 0, 1, null);
            }

            StbTrueType.stbtt_PackSetOversampling(packContext, 8, 8);
            fixed (byte* dat = buffer)
            {
                fixed (StbTrueType.stbtt_packedchar* c = _chars)
                {
                    StbTrueType.stbtt_PackFontRange(packContext, dat, 0, height, 32, 256, c);
                }
            }

            StbTrueType.stbtt_PackEnd(packContext);
            
            int asc;
            StbTrueType.stbtt_GetFontVMetrics(fontInfo, &asc, null, null);
            _ascent = asc * StbTrueType.stbtt_ScaleForPixelHeight(fontInfo, height);
            
            Texture = Texture.LoadFromBuffer(bitmap, 2048, 2048, PixelFormat.Red, PixelInternalFormat.R8);
        }

        public void Bind()
        {
            Texture.Bind(TextureUnit.Texture0);
        }

        public static void Unbind()
        {
            Texture.Unbind();
        }

        public void Draw(Mesh mesh, string text, float x, float y, uint color, bool shadow, float scale = 1.0f)
        {
            int length = text.Length;
            float drawX = x;
            float drawY = y - _ascent * scale;
            float alpha = ((color >> 24) & 0xFF) / 255.0f;
            float red = ((color >> 16) & 0xFF) / 255.0f;
            float green = ((color >> 8) & 0xFF) / 255.0f;
            float blue = (color & 0xFF) / 255.0f;
            string lower = text.ToLower();
            for (int i = 0; i < length; i++)
            {
                char charCode = text[i];
                char previous = i > 0 ? text[i - 1] : ' ';
                if (previous == '\u00a7')
                {
                    continue;
                }

                if (charCode == '\u00a7' && i < length - 1)
                {
                    char next = lower[i + 1];
                    Formatting fmt;
                    if (Formatting.Values.TryGetValue(next, out fmt))
                    {
                        uint newColor = fmt.Color;
                        red = ((newColor >> 16) & 0xFF) / 255.0f;
                        green = ((newColor >> 8) & 0xFF) / 255.0f;
                        blue = (newColor & 0xFF) / 255.0f;
                    }
                    continue;
                }

                if (charCode < 32 || charCode > 32 + 256) charCode = ' ';
                
                StbTrueType.stbtt_packedchar c = _chars[charCode - 32];

                if (shadow)
                {
                    int j1 = mesh.Float3(drawX + c.xoff * scale + 1, drawY - c.yoff * scale - 1, 1).Float3(0, 0, 1).Float2(c.x0 * _IPW, c.y0 * _IPH).Float4(red * 0.125f, green * 0.125f, blue * 0.125f, alpha).Next();
                    int j2 = mesh.Float3(drawX + c.xoff * scale + 1, drawY - c.yoff2 * scale - 1, 1).Float3(0, 0, 1).Float2(c.x0 * _IPW, c.y1 * _IPH).Float4(red * 0.125f, green * 0.125f, blue * 0.125f, alpha).Next();
                    int j3 = mesh.Float3(drawX + c.xoff2 * scale + 1, drawY - c.yoff2 * scale - 1, 1).Float3(0, 0, 1).Float2(c.x1 * _IPW, c.y1 * _IPH).Float4(red * 0.125f, green * 0.125f, blue * 0.125f, alpha).Next();
                    int j4 = mesh.Float3(drawX + c.xoff2 * scale + 1, drawY - c.yoff * scale - 1, 1).Float3(0, 0, 1).Float2(c.x1 * _IPW, c.y0 * _IPH).Float4(red * 0.125f, green * 0.125f, blue * 0.125f, alpha).Next();
                    mesh.Quad(j1, j2, j3, j4);
                }
                
                int k1 = mesh.Float3(drawX + c.xoff * scale, drawY - c.yoff * scale, 0).Float3(0, 0, 1).Float2(c.x0 * _IPW, c.y0 * _IPH).Float4(red, green, blue, alpha).Next();
                int k2 = mesh.Float3(drawX + c.xoff * scale, drawY - c.yoff2 * scale, 0).Float3(0, 0, 1).Float2(c.x0 * _IPW, c.y1 * _IPH).Float4(red, green, blue, alpha).Next();
                int k3 = mesh.Float3(drawX + c.xoff2 * scale, drawY - c.yoff2 * scale, 0).Float3(0, 0, 1).Float2(c.x1 * _IPW, c.y1 * _IPH).Float4(red, green, blue, alpha).Next();
                int k4 = mesh.Float3(drawX + c.xoff2 * scale, drawY - c.yoff * scale, 0).Float3(0, 0, 1).Float2(c.x1 * _IPW, c.y0 * _IPH).Float4(red, green, blue, alpha).Next();
                mesh.Quad(k1, k2, k3, k4);

                drawX += c.xadvance * scale;
            }
        }

    }
}