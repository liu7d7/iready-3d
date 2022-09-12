using Iready.Shared;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Iready.Engine
{
    public class Mesh
    {

        private readonly Vao _vao;
        private readonly Vbo _vbo;
        private readonly Ibo _ibo;
        private readonly DrawMode _drawMode;
        private readonly Shader _shader;
        private int _vertex;
        private int _index;
        private bool _building;

        public Mesh(DrawMode drawMode, Shader shader, params Vao.Attrib[] attribs)
        {
            _drawMode = drawMode;
            _shader = shader;
            var stride = attribs.Sum(attrib => (int) attrib * sizeof(float));
            _vbo = new Vbo(stride * drawMode.Size * 256 * sizeof(float));
            _vbo.Bind();
            _ibo = new Ibo(drawMode.Size * 512 * sizeof(float));
            _ibo.Bind();
            _vao = new Vao(attribs);
            Vbo.Unbind();
            Ibo.Unbind();
            Vao.Unbind();
        }

        public int Next()
        {
            return _vertex++;
        }

        public Mesh Float1(float p0)
        {
            _vbo.Put(p0);
            return this;
        }

        public Mesh Float2(float p0, float p1)
        {
            _vbo.Put(p0);
            _vbo.Put(p1);
            return this;
        }
        
        public Mesh Float2(Vector2 p0)
        {
            _vbo.Put(p0.X);
            _vbo.Put(p0.Y);
            return this;
        }

        public Mesh Float3(float p0, float p1, float p2)
        {
            _vbo.Put(p0);
            _vbo.Put(p1);
            _vbo.Put(p2);
            return this;
        }

        public Mesh Float3(Matrix4 transform, float p0, float p1, float p2)
        {
            Vector4 pos = new(p0, p1, p2, 1);
            pos.Transform(transform);
            _vbo.Put(pos.X);
            _vbo.Put(pos.Y);
            _vbo.Put(pos.Z);
            return this;
        }
        
        public Mesh Float3(Vector3 p0)
        {
            _vbo.Put(p0.X);
            _vbo.Put(p0.Y);
            _vbo.Put(p0.Z);
            return this;
        }

        public Mesh Float4(float p0, float p1, float p2, float p3)
        {
            _vbo.Put(p0);
            _vbo.Put(p1);
            _vbo.Put(p2);
            _vbo.Put(p3);
            return this;
        }

        public Mesh Float4(uint color)
        {
            return Float4(((color >> 16) & 0xff) * 0.003921569f, ((color >> 8) & 0xff) * 0.003921569f, (color & 0xff) * 0.003921569f, ((color >> 24) & 0xff) * 0.003921569f);
        }

        public Mesh Float4(uint color, float alpha)
        {
            return Float4(((color >> 16) & 0xff) * 0.003921569f, ((color >> 8) & 0xff) * 0.003921569f, (color & 0xff) * 0.003921569f, alpha);
        }
        
        public void Line(int p0, int p1)
        {
            _ibo.Put(p0);
            _ibo.Put(p1);
            _index += 2;
        }

        public void Tri(int p0, int p1, int p2)
        {
            _ibo.Put(p0);
            _ibo.Put(p1);
            _ibo.Put(p2);
            _index += 3;
        }
        
        public void Quad(int p0, int p1, int p2, int p3)
        {
            _ibo.Put(p0);
            _ibo.Put(p1);
            _ibo.Put(p2);
            _ibo.Put(p2);
            _ibo.Put(p3);
            _ibo.Put(p0);
            _index += 6;
        }

        public void Begin()
        {
            if (_building)
            {
                throw new Exception("Already building");
            }
            _vbo.Clear();
            _ibo.Clear();
            _vertex = 0;
            _index = 0;
            _building = true;
        }

        public void End()
        {
            if (!_building)
            {
                throw new Exception("Not building");
            }

            if (_index > 0)
            {
                _vbo.Upload();
                _ibo.Upload();
            }

            _building = false;
        }

        public void Render()
        {
            if (_building)
            {
                End();
            }

            if (_index > 0)
            {
                GlStateManager.SaveState();
                GlStateManager.EnableBlend();
                if (RenderSystem.Rendering3d)
                {
                    GlStateManager.EnableDepth();
                }
                else
                {
                    GlStateManager.DisableDepth();
                }
                _shader?.Bind();
                _shader?.SetDefaults();
                _vao.Bind();
                _ibo.Bind();
                _vbo.Bind();
                GL.DrawElements(_drawMode.AsGl(), _index, DrawElementsType.UnsignedInt, 0);
                Ibo.Unbind();
                Vbo.Unbind();
                Vao.Unbind();
                GlStateManager.RestoreState();
            }
        }

        public sealed class DrawMode
        {
            private static int _cidCounter;
            private readonly int _cid;
            public readonly int Size;

            private DrawMode(int size)
            {
                Size = size;
                _cid = _cidCounter++;
            }

            public override bool Equals(object obj)
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

            public BeginMode AsGl()
            {
                return _cid switch
                {
                    0 => BeginMode.Lines,
                    1 => BeginMode.Triangles,
                    2 => BeginMode.TriangleFan,
                    _ => throw new Exception("wtf is going on?")
                };
            }

            public static readonly DrawMode LINE = new(2);
            public static readonly DrawMode TRIANGLE = new(3);
            public static readonly DrawMode TRIANGLE_FAN = new(3);
        }
    }
}