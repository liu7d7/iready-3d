using System.Drawing.Drawing2D;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Why.Game;
using Why.Shared;
using Why.Shared.Components;

namespace Why.Engine
{
    public static class RenderSystem
    {

        private static readonly Shader _john = new("Resource/Shader/john.vert", "Resource/Shader/john.frag");
        private static Matrix4 _projection;
        private static Matrix4 _lookAt;

        public static readonly Texture tex0 = Texture.loadFromFile("Resource/Texture/Texture.png");
        public static readonly Mesh mesh = new(Mesh.DrawMode.triangle, _john, Vao.Attrib.float3, Vao.Attrib.float2, Vao.Attrib.float4);
        public static readonly Fbo frame = new(Why.instance.Size.X, Why.instance.Size.Y, true);

        public static void setDefaults(this Shader shader)
        {
            shader.setMatrix4("_proj", _projection);
            shader.setMatrix4("_lookAt", _lookAt);
            shader.setVector2("_tex0Size", Texture.currentBounds());
        }

        public static void updateProjection()
        {
            Matrix4.CreateOrthographic(Why.instance.Size.X, Why.instance.Size.Y, -1000, 3000, out _projection);
        }

        public static void updateLookAt(WhyObj cameraObj, bool twoD = false)
        {
            if (!cameraObj.hasComponent<FloatPosComponent>())
            {
                return;
            }

            if (twoD)
            {
                _lookAt = Matrix4.Identity;
            }

            var comp = cameraObj.getComponent<CameraComponent>();
            _lookAt = comp.getCameraMatrix();
        }
        
        public static void draw(AABB bounds, CubeTexturing texturing, CubeRenderData data, bool beginEnd = false)
        {
            if (beginEnd)
            {
                mesh.begin();
            }

            var minX = bounds.minX;
            var minY = bounds.minY;
            var minZ = bounds.minZ;
            var maxX = bounds.maxX;
            var maxY = bounds.maxY;
            var maxZ = bounds.maxZ;

            var top = texturing.top;
            var left = texturing.left;
            var right = texturing.right;
            var front = texturing.front;
            var back = texturing.back;

            if (data.drawTop) { // top (max y)
                int i1 = mesh.float3(minX, maxY, minZ).float2(top.u, top.v).float4(0xffffffff).next();
                int i2 = mesh.float3(maxX, maxY, minZ).float2(top.u + top.width, top.v).float4(0xffffffff).next();
                int i3 = mesh.float3(maxX, maxY, maxZ).float2(top.u + top.width, top.v + top.height).float4(0xffffffff).next();
                int i4 = mesh.float3(minX, maxY, maxZ).float2(top.u, top.v + top.height).float4(0xffffffff).next();
                mesh.quad(i1, i2, i3, i4);
            }
            
            if (data.drawLeft) { // left (min x)
                int i1 = mesh.float3(minX, minY, minZ).float2(left.u, left.v + left.height).float4(0xffffffff).next();
                int i2 = mesh.float3(minX, maxY, minZ).float2(left.u, left.v).float4(0xffffffff).next();
                int i3 = mesh.float3(minX, maxY, maxZ).float2(left.u + left.width, left.v).float4(0xffffffff).next();
                int i4 = mesh.float3(minX, minY, maxZ).float2(left.u + left.width, left.v + left.height).float4(0xffffffff).next();
                mesh.quad(i1, i2, i3, i4);
            }

            if (data.drawRight) { // right (max x)
                int i1 = mesh.float3(maxX, minY, minZ).float2(right.u, right.v + right.height).float4(0xffffffff).next();
                int i2 = mesh.float3(maxX, maxY, minZ).float2(right.u, right.v).float4(0xffffffff).next();
                int i3 = mesh.float3(maxX, maxY, maxZ).float2(right.u + right.width, right.v).float4(0xffffffff).next();
                int i4 = mesh.float3(maxX, minY, maxZ).float2(right.u + right.width, right.v + right.height).float4(0xffffffff).next();
                mesh.quad(i1, i2, i3, i4);
            }

            if (data.drawFront) { // front (min z)
                int i1 = mesh.float3(minX, minY, minZ).float2(front.u, front.v + front.height).float4(0xffffffff).next();
                int i2 = mesh.float3(maxX, minY, minZ).float2(front.u + front.width, front.v + front.height).float4(0xffffffff).next();
                int i3 = mesh.float3(maxX, maxY, minZ).float2(front.u + front.width, front.v).float4(0xffffffff).next();
                int i4 = mesh.float3(minX, maxY, minZ).float2(front.u, front.v).float4(0xffffffff).next();
                mesh.quad(i1, i2, i3, i4);
            }

            if (data.drawBack) { // back (max z)
                int i1 = mesh.float3(minX, minY, maxZ).float2(back.u, back.v + back.height).float4(0xffffffff).next();
                int i2 = mesh.float3(maxX, minY, maxZ).float2(back.u + back.width, back.v + back.height).float4(0xffffffff).next();
                int i3 = mesh.float3(maxX, maxY, maxZ).float2(back.u + back.width, back.v).float4(0xffffffff).next();
                int i4 = mesh.float3(minX, maxY, maxZ).float2(back.u, back.v).float4(0xffffffff).next();
                mesh.quad(i1, i2, i3, i4);
            }

            if (beginEnd)
            {
                mesh.end();
            }
        }

        public static void draw(Rect? _bounds, Sprite sprite, bool beginEnd = false)
        {
            if (!_bounds.HasValue)
            {
                return;
            }

            if (beginEnd)
            {
                mesh.begin();
            }
            
            var bounds = _bounds.Value;
            int i1 = mesh.float3(bounds.x, 0, bounds.y).float2(sprite.u, sprite.v).float4(0xffffffff).next();
            int i2 = mesh.float3(bounds.x + bounds.width, 0, bounds.y).float2(sprite.u + sprite.width, sprite.v).float4(0xffffffff).next();
            int i3 = mesh.float3(bounds.x + bounds.width, 0, bounds.y + bounds.height).float2(sprite.u + sprite.width, sprite.v + sprite.height).float4(0xffffffff).next();
            int i4 = mesh.float3(bounds.x, 0, bounds.y + bounds.height).float2(sprite.u, sprite.v + sprite.height).float4(0xffffffff).next();
            mesh.quad(i1, i2, i3, i4);
            
            if (beginEnd)
            {
                mesh.end();
            }
        }
    }
}