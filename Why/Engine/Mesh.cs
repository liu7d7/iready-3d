using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Why.Engine
{
    public class Mesh
    {

        private readonly Vao _vao;
        private readonly Vbo _vbo;
        private readonly Ibo _ibo;
        private readonly DrawMode _drawMode;
        private readonly Shader? _shader;
        private int _vertex;
        private int _index;
        private bool _building;

        public Mesh(DrawMode drawMode, Shader? shader, params Vao.Attrib[] attribs)
        {
            _drawMode = drawMode;
            _shader = shader;
            var stride = attribs.Sum(attrib => (int) attrib * sizeof(float));
            _vbo = new Vbo(stride * drawMode.size * 256 * sizeof(float));
            _vbo.bind();
            _ibo = new Ibo(drawMode.size * 512 * sizeof(float));
            _ibo.bind();
            _vao = new Vao(attribs);
            Vbo.unbind();
            Ibo.unbind();
            Vao.unbind();
        }

        public int next()
        {
            return _vertex++;
        }

        public Mesh float1(float p0)
        {
            _vbo.put(p0);
            return this;
        }

        public Mesh float2(float p0, float p1)
        {
            _vbo.put(p0);
            _vbo.put(p1);
            return this;
        }

        public Mesh float3(float p0, float p1, float p2)
        {
            _vbo.put(p0);
            _vbo.put(p1);
            _vbo.put(p2);
            return this;
        }

        public Mesh float3(Matrix4 transform, float p0, float p1, float p2)
        {
            Vector4 vec = new(p0, p1, p2, 1);
            vec *= transform;
            float3(vec.X, vec.Y, vec.Z);
            return this;
        }
        
        public Mesh float4(float p0, float p1, float p2, float p3)
        {
            _vbo.put(p0);
            _vbo.put(p1);
            _vbo.put(p2);
            _vbo.put(p3);
            return this;
        }

        public Mesh float4(uint color)
        {
            return float4(((color >> 16) & 0xff) * 0.003921569f, ((color >> 8) & 0xff) * 0.003921569f, (color & 0xff) * 0.003921569f, ((color >> 24) & 0xff) * 0.003921569f);
        }

        public Mesh float4(uint color, float alpha)
        {
            return float4(((color >> 16) & 0xff) * 0.003921569f, ((color >> 8) & 0xff) * 0.003921569f, (color & 0xff) * 0.003921569f, alpha);
        }

        public void tri(int p0, int p1, int p2)
        {
            _ibo.put(p0);
            _ibo.put(p1);
            _ibo.put(p2);
            _index += 3;
        }
        
        public void quad(int p0, int p1, int p2, int p3)
        {
            _ibo.put(p0);
            _ibo.put(p1);
            _ibo.put(p2);
            _ibo.put(p2);
            _ibo.put(p3);
            _ibo.put(p0);
            _index += 6;
        }

        public void begin()
        {
            if (_building)
            {
                throw new Exception("Already building");
            }
            _vbo.clear();
            _ibo.clear();
            _vertex = 0;
            _index = 0;
            _building = true;
        }

        public void end()
        {
            if (!_building)
            {
                throw new Exception("Not building");
            }

            if (_index > 0)
            {
                _vbo.upload();
                _ibo.upload();
            }

            _building = false;
        }

        public void render()
        {
            if (_building)
            {
                end();
            }

            if (_index > 0)
            {
                GlStateManager.saveState();
                GlStateManager.enableBlend();
                if (RenderSystem.rendering3d)
                {
                    GlStateManager.enableDepth();
                }
                else
                {
                    GlStateManager.disableDepth();
                }
                _shader?.bind();
                _shader?.setDefaults();
                _vao.bind();
                _ibo.bind();
                _vbo.bind();
                GL.DrawElements(_drawMode.asGl(), _index, DrawElementsType.UnsignedInt, 0);
                Ibo.unbind();
                Vbo.unbind();
                Vao.unbind();
                GlStateManager.restoreState();
            }
        }

        public sealed class DrawMode
        {
            private static int _cidCounter;
            private readonly int _cid;
            public readonly int size;

            private DrawMode(int size)
            {
                this.size = size;
                _cid = _cidCounter++;
            }

            public override bool Equals(object? obj)
            {
                if (obj is DrawMode mode)
                {
                    return _cid == mode._cid;
                }

                return false;
            }
        
            public override int GetHashCode()
            {
                return _cid;
            }

            public BeginMode asGl()
            {
                return _cid switch
                {
                    0 => BeginMode.Lines,
                    1 => BeginMode.Triangles,
                    2 => BeginMode.TriangleFan,
                    _ => throw new Exception("wtf is going on?")
                };
            }

            public static readonly DrawMode line = new(2);
            public static readonly DrawMode triangle = new(3);
            public static readonly DrawMode triangleFan = new(3);
        }
    }
}