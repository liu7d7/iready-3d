using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Why.Engine;
using Why.Shared;
using Why.Shared.Components;

namespace Why.Game.Components
{
    public class PlayerComponent : WhyObj.Component
    {

        private Vector3 _motion;
        private FloatPosComponent? _pos;
        private CameraComponent? _camera;
        
        public PlayerComponent()
        {
            _motion = Vector3.Zero;
        }

        public override void render(WhyObj objIn)
        {
            base.render(objIn);

            var comp = objIn.getComponent<FloatPosComponent>();
            
            var x = comp.x;
            var y = comp.y;
            var z = comp.z;
            int i1 = RenderSystem.mesh.float3(x - 30, y, z - 30).float2(Sprites.rectangle.u, Sprites.rectangle.v).float4(0xffffffff).next();
            int i2 = RenderSystem.mesh.float3(x - 30, y, z + 30).float2(Sprites.rectangle.u, Sprites.rectangle.v + Sprites.rectangle.height).float4(0xffffffff).next();
            int i3 = RenderSystem.mesh.float3(x + 30, y, z + 30).float2(Sprites.rectangle.u + Sprites.rectangle.width, Sprites.rectangle.v + Sprites.rectangle.height).float4(0xffffffff).next();
            int i4 = RenderSystem.mesh.float3(x + 30, y, z - 30).float2(Sprites.rectangle.u + Sprites.rectangle.width, Sprites.rectangle.v).float4(0xffffffff).next();
            RenderSystem.mesh.quad(i1, i2, i3, i4);
        }

        public override void update(WhyObj objIn)
        {
            
            if (_pos == null)
            {
                _pos = objIn.getComponent<FloatPosComponent>();
            }
            
            if (_camera == null)
            {
                _camera = objIn.getComponent<CameraComponent>();
            }
            
            base.update(objIn);
            
            _motion = Vector3.Zero;
            var yaw = 0.0f;

            var kstate = Why.instance.KeyboardState;

            if (kstate.IsKeyDown(Keys.W))
            {
                var vec = Vector3.Cross(_camera.right, _camera.up).Normalized();
                _motion -= vec;
            }

            if (kstate.IsKeyDown(Keys.S))
            {
                var vec = Vector3.Cross(_camera.right, _camera.up).Normalized();
                _motion += vec;
            }

            if (kstate.IsKeyDown(Keys.A))
            {
                _motion -= Vector3.Cross(_camera.front, _camera.up).Normalized();
            }

            if (kstate.IsKeyDown(Keys.D))
            {
                _motion += Vector3.Cross(_camera.front, _camera.up).Normalized();
            }

            if (kstate.IsKeyDown(Keys.Q))
            {
                yaw -= 1.0f;
            }
            
            if (kstate.IsKeyDown(Keys.E))
            {
                yaw += 1.0f;
            }
            
            _pos.x += _motion.X;
            _pos.y += _motion.Y;
            _pos.z += _motion.Z;
            _pos.yaw += yaw;
            
            var mstate = Why.instance.MouseState;
            
            if (kstate.IsKeyDown(Keys.LeftShift))
            {
                _pos.pitch += MathF.Sign(mstate.ScrollDelta.Y);
            }
            else
            {
                _pos.yaw += MathF.Sign(mstate.ScrollDelta.Y);
            }
            
            if (kstate.IsKeyDown(Keys.R))
            {
                _pos.x = _pos.y = _pos.z = 0;
                _pos.pitch = 215;
                _pos.yaw = 45;
                _motion = Vector3.Zero;
            }

        }
    }
}