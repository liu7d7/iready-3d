using OpenTK.Graphics.OpenGL4;

namespace Why.Engine
{
    public class Vao
    {
        private static int _active;
        private readonly int _handle;

        public Vao(params Attrib[] attribs)
        {
            _handle = GL.GenVertexArray();
            bind();
            var stride = attribs.Sum(attrib => (int) attrib);
            int offset = 0;
            for (int i = 0; i < attribs.Length; i++)
            {
                GL.EnableVertexAttribArray(i);
                GL.VertexAttribPointer(i, (int) attribs[i], VertexAttribPointerType.Float, false, stride * sizeof(float), offset);
                offset += (int) attribs[i] * sizeof(float);
            }
            unbind();
        }

        public void bind()
        {
            if (_handle == _active)
            {
                return;
            }
            GL.BindVertexArray(_handle);
            _active = _handle;
        }

        public static void unbind()
        {
            GL.BindVertexArray(0);
            _active = 0;
        }

        public enum Attrib
        {
            float1 = 1, float2 = 2, float3 = 3, float4 = 4
        }
    }
}