
using OpenTK.Graphics.OpenGL4;

namespace Why.Engine
{
    public class Vbo
    {
        private static int _active;
        private readonly int _handle;
        private readonly MemoryStream _vertices;

        public Vbo(int initialCapacity)
        {
            _handle = GL.GenBuffer();
            _vertices = new MemoryStream(initialCapacity);
        }

        public void bind()
        {
            if (_handle == _active)
            {
                return;
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
            _active = _handle;
        }

        public static void unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            _active = 0;
        }

        public void put(float element)
        {
            _vertices.Write(BitConverter.GetBytes(element));
        }

        public void upload(bool unbindAfter = true)
        {
            if (_active != _handle)
            {
                bind();
            }
            GL.BufferData(BufferTarget.ArrayBuffer, (int) _vertices.Length, _vertices.ToArray(), BufferUsageHint.DynamicDraw);
            if (unbindAfter)
            {
                unbind();
            }
        }

        public void clear()
        {
            _vertices.SetLength(0);
        }
    }
}