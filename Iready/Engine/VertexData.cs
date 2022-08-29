using OpenTK.Mathematics;

namespace Iready.Engine
{
    public class VertexData
    {
        public Vector3 Pos;
        public Vector3 Normal;
        public Vector2 Uv;
        public uint Color = 0xffffffff;
        
        public VertexData(Vector3 pos, Vector2 uv)
        {
            Pos = pos;
            Uv = uv;
        }
    }
}