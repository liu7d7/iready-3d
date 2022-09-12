
using OpenTK.Graphics.OpenGL4;

namespace Iready.Engine
{
    public class Vbo
    {
        private static int _active;
        private readonly int _handle;
        private readonly MemoryStream _vertices;
        private byte[] _bitDest = new byte[4];

        public Vbo(int initialCapacity)
        {
            _handle = GL.GenBuffer();
            _vertices = new MemoryStream(initialCapacity);
        }

        public void Bind()
        {
            if (_handle == _active)
            {
                return;
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
            _active = _handle;
        }

        public static void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            _active = 0;
        }

        public void Put(float element)
        {
            BitConverter.TryWriteBytes(_bitDest, element);
            _vertices.Write(_bitDest);
        }

        public void Upload(bool unbindAfter = true)
        {
            if (_active != _handle)
            {
                Bind();
            }
            GL.BufferData(BufferTarget.ArrayBuffer, (int) _vertices.Length, _vertices.GetBuffer(), BufferUsageHint.DynamicDraw);
            if (unbindAfter)
            {
                Unbind();
            }
        }

        public void Clear()
        {
            _vertices.SetLength(0);
        }
    }
}