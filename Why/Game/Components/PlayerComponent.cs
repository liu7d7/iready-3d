using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Why.Engine;
using Why.Shared;
using Why.Shared.Components;

namespace Why.Game.Components
{
    public class PlayerComponent : WhyObj.Component
    {

        private Vector2 _motion;
        private FloatPosComponent? _component;
        
        public PlayerComponent()
        {
            _motion = Vector2.Zero;
        }

        public override void render(WhyObj objIn)
        {
            base.render(objIn);

            var comp = objIn.getComponent<FloatPosComponent>();

            for (int i = 3; i-- > 0;)
            {
                var x = comp.x - _motion.X * i * 10;
                var z = comp.z - _motion.Y * i * 10;
                int i1 = RenderGlobal.mesh.float3(x - 30, 0f, z - 30).float2(Sprites.grassTop.u, Sprites.grassTop.v).float4(0xffffff, (3f - i) / 3f).next();
                int i2 = RenderGlobal.mesh.float3(x - 30, 0f, z + 30).float2(Sprites.grassTop.u, Sprites.grassTop.v + Sprites.grassTop.height).float4(0xffffff, (3f - i) / 3f).next();
                int i3 = RenderGlobal.mesh.float3(x + 30, 0f, z + 30).float2(Sprites.grassTop.u + Sprites.grassTop.width, Sprites.grassTop.v + Sprites.grassTop.height).float4(0xffffff, (3f - i) / 3f).next();
                int i4 = RenderGlobal.mesh.float3(x + 30, 0f, z - 30).float2(Sprites.grassTop.u + Sprites.grassTop.width, Sprites.grassTop.v).float4(0xffffff, (3f - i) / 3f).next();
                RenderGlobal.mesh.quad(i1, i2, i3, i4);
            }
            // RenderGlobal.draw(new(x - 10, y - 10, z - 10, x + 10, y + 10, z + 10), CubeTexturing.grass, new CubeRenderData());
        }

        public override void update(WhyObj objIn)
        {
            
            if (_component == null)
            {
                _component = objIn.getComponent<FloatPosComponent>();
            }
            
            base.update(objIn);
            
            float forward = 0;
            float strafe = 0;

            var kstate = Why.instance.KeyboardState;

            if (kstate.IsKeyDown(Keys.W))
            {
                forward -= 1;
            }

            if (kstate.IsKeyDown(Keys.S))
            {
                forward += 1;
            }

            if (kstate.IsKeyDown(Keys.A))
            {
                strafe -= 1;
            }

            if (kstate.IsKeyDown(Keys.D))
            {
                strafe += 1;
            }

            _motion = new Vector2(strafe, forward).normalizedFast();

            _component.x += _motion.X;
            _component.z += _motion.Y;
            
            var mstate = Why.instance.MouseState;
            
            if (kstate.IsKeyDown(Keys.LeftShift))
            {
                _component.pitch += MathF.Sign(mstate.ScrollDelta.Y);
            }
            else
            {
                _component.yaw += MathF.Sign(mstate.ScrollDelta.Y);
            }

            if (kstate.IsKeyDown(Keys.R))
            {
                _component.x = _component.y = _component.z = 0;
                _component.pitch = _component.yaw = 90;
                _motion = Vector2.Zero;
            }

        }
    }
}