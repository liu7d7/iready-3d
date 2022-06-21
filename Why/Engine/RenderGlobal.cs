using OpenTK.Mathematics;

namespace Why.Engine
{
    public static class RenderGlobal
    {

        private static readonly Shader john = new("Resource/Shader/john.vert", "Resource/Shader/john.frag");

        public static readonly Mesh mesh = new(Mesh.DrawMode.triangle, john, Vao.Attrib.float3, Vao.Attrib.float4);
        public static Matrix4 projection;

        public static void updateProjection()
        {
            // 0, 0 top left
            // Matrix4.CreateOrthographicOffCenter(0f, Why.instance.sizef.X, Why.instance.sizef.Y, 0f, -1.0f, 1.0f, out projection);
            // 0, 0 middle
            Matrix4.CreateOrthographic(Why.instance.sizef.X, Why.instance.sizef.Y, -1.0f, 1.0f, out projection);
        }
        
    }
}