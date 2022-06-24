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
        private static readonly Shader _outline = new("Resource/Shader/twoD.vert", "Resource/Shader/depthOutline.frag");
        private static readonly uint _light = 0xffffffff;
        private static readonly uint _darker = 0xffdddddd;
        private static readonly uint _darkest = 0xffbbbbbb;
        private static Matrix4 _projection;
        private static Matrix4 _lookAt;

        public static readonly Texture tex0 = Texture.loadFromFile("Resource/Texture/Texture.png");
        public static readonly Mesh mesh = new(Mesh.DrawMode.triangle, _john, Vao.Attrib.float3, Vao.Attrib.float2, Vao.Attrib.float4);
        public static readonly Fbo frame = new(Why.instance.Size.X, Why.instance.Size.Y, true);
        public static bool rendering3d;

        public static void setDefaults(this Shader shader)
        {
            shader.setMatrix4("_proj", _projection);
            shader.setMatrix4("_lookAt", _lookAt);
            shader.setVector2("_tex0Size", Texture.currentBounds());
            shader.setVector2("_screenSize", new(Why.instance.Size.X, Why.instance.Size.Y));
        }

        public static void updateProjection()
        {
            Matrix4.CreateOrthographic(Why.instance.Size.X, Why.instance.Size.Y, -1000, 3000, out _projection);
        }

        public static void updateLookAt(WhyObj cameraObj, bool _rendering3d = true)
        {
            if (!cameraObj.hasComponent<FloatPosComponent>())
            {
                return;
            }

            rendering3d = _rendering3d;
            if (!rendering3d)
            {
                _lookAt = Matrix4.Identity;
                return;
            }

            var comp = cameraObj.getComponent<CameraComponent>();
            _lookAt = comp.getCameraMatrix();
        }

        public static void draw(AABB? _bounds, CubeTexturing texturing, CubeRenderData data, bool beginEnd = false)
        {
            if (beginEnd)
            {
                mesh.begin();
            }

            if (!_bounds.HasValue)
            {
                return;
            }

            AABB bounds = _bounds.Value;

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

            // top (max y)
            if (data.drawTop)  
            {
                int i1 = mesh.float3(minX, maxY, minZ).float2(top.u, top.v).float4(_light).next();
                int i2 = mesh.float3(maxX, maxY, minZ).float2(top.u + top.width, top.v).float4(_light).next();
                int i3 = mesh.float3(maxX, maxY, maxZ).float2(top.u + top.width, top.v + top.height).float4(_light).next();
                int i4 = mesh.float3(minX, maxY, maxZ).float2(top.u, top.v + top.height).float4(_light).next();
                mesh.quad(i1, i2, i3, i4);
            }
            
            // left (min x)
            if (data.drawLeft) 
            {
                int i1 = mesh.float3(minX, minY, minZ).float2(left.u, left.v + left.height).float4(_darker).next();
                int i2 = mesh.float3(minX, maxY, minZ).float2(left.u, left.v).float4(_darker).next();
                int i3 = mesh.float3(minX, maxY, maxZ).float2(left.u + left.width, left.v).float4(_darker).next();
                int i4 = mesh.float3(minX, minY, maxZ).float2(left.u + left.width, left.v + left.height).float4(_darker).next();
                mesh.quad(i1, i2, i3, i4);
            }

            // right (max x)
            if (data.drawRight)
            {
                int i1 = mesh.float3(maxX, minY, minZ).float2(right.u, right.v + right.height).float4(_darkest).next();
                int i2 = mesh.float3(maxX, maxY, minZ).float2(right.u, right.v).float4(_darkest).next();
                int i3 = mesh.float3(maxX, maxY, maxZ).float2(right.u + right.width, right.v).float4(_darkest).next();
                int i4 = mesh.float3(maxX, minY, maxZ).float2(right.u + right.width, right.v + right.height).float4(_darkest).next();
                mesh.quad(i1, i2, i3, i4);
            }

            // front (min z)
            if (data.drawFront) 
            {
                int i1 = mesh.float3(minX, minY, minZ).float2(front.u, front.v + front.height).float4(_darker).next();
                int i2 = mesh.float3(maxX, minY, minZ).float2(front.u + front.width, front.v + front.height).float4(_darker).next();
                int i3 = mesh.float3(maxX, maxY, minZ).float2(front.u + front.width, front.v).float4(_darker).next();
                int i4 = mesh.float3(minX, maxY, minZ).float2(front.u, front.v).float4(_darker).next();
                mesh.quad(i1, i2, i3, i4);
            }

            // back (max z)
            if (data.drawBack)
            { 
                int i1 = mesh.float3(minX, minY, maxZ).float2(back.u, back.v + back.height).float4(_darkest).next();
                int i2 = mesh.float3(maxX, minY, maxZ).float2(back.u + back.width, back.v + back.height).float4(_darkest).next();
                int i3 = mesh.float3(maxX, maxY, maxZ).float2(back.u + back.width, back.v).float4(_darkest).next();
                int i4 = mesh.float3(minX, maxY, maxZ).float2(back.u, back.v).float4(_darkest).next();
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

        public static void drawHat(Vector3 tip, float rad, float lowering, uint color)
        {
            // render circle with center raised but circumference lowered
            for (int i = 0; i < 60; i++)
            {
                float startAngle = i * 6;
                float endAngle = i * 6 + 6;
                float startX = MathF.Cos(startAngle.toRadians()) * rad + tip.X;
                float startZ = MathF.Sin(startAngle.toRadians()) * rad + tip.Z;
                float endX = MathF.Cos(endAngle.toRadians()) * rad + tip.X;
                float endZ = MathF.Sin(endAngle.toRadians()) * rad + tip.Z;
                int i1 = mesh.float3(tip.X, tip.Y, tip.Z).float2(Sprites.rectangle.u, Sprites.rectangle.v).float4(color).next();
                int i2 = mesh.float3(startX, tip.Y - lowering, startZ).float2(Sprites.rectangle.u, Sprites.rectangle.v + Sprites.rectangle.height).float4(color).next();
                int i3 = mesh.float3(endX, tip.Y - lowering, endZ).float2(Sprites.rectangle.u + Sprites.rectangle.width, Sprites.rectangle.v).float4(color).next();
                mesh.tri(i1, i2, i3);
            }
        }
    }
}