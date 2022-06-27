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
        private IntPosComponent? _posi;
        private CameraComponent? _camera;
        
        public PlayerComponent()
        {
            _motion = Vector3.Zero;
        }

        public override void render(WhyObj objIn)
        {
            base.render(objIn);

            var comp = objIn.getComponent<FloatPosComponent>();
            
            float x = comp.x;
            float y = comp.y;
            float z = comp.z;
            
            
            AABB aabb = new(x - 5, y, z - 5, x + 5, y + 65, z + 5);
            RenderSystem.draw(aabb, CubeTexturing.player, CubeRenderData.allTrue);
            AABB aabb1 = new(x - 10, y + 70, z - 10, x + 10, y + 90, z + 10);
            RenderSystem.draw(aabb1, CubeTexturing.playerHead, CubeRenderData.allTrue);
        }

        public override void update(WhyObj objIn)
        {
            
            if (_pos == null)
            {
                _pos = objIn.getComponent<FloatPosComponent>();
            }
            
            if (_posi == null)
            {
                _posi = objIn.getComponent<IntPosComponent>();
            }
            
            if (_camera == null)
            {
                _camera = objIn.getComponent<CameraComponent>();
            }
            
            base.update(objIn);
            
            _motion = Vector3.Zero;
            float yaw = 0.0f;

            KeyboardState kstate = Why.instance.KeyboardState;

            if (kstate.IsKeyDown(Keys.W))
            {
                Vector3 vec = Vector3.Cross(_camera.right, _camera.up).Normalized();
                _motion -= vec;
            }

            if (kstate.IsKeyDown(Keys.S))
            {
                Vector3 vec = Vector3.Cross(_camera.right, _camera.up).Normalized();
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
            
            if (_motion.X != 0 && _motion.Z != 0 && _motion.Y != 0)
            {
                _motion.Normalize();
            }
            
            if (_motion != Vector3.Zero)
            {
                _motion.NormalizeFast();
            }

            _pos.x += _motion.X * 4;
            _pos.y += _motion.Y * 4;
            _pos.z += _motion.Z * 4;
            _pos.yaw += yaw;
            _posi.x = (int) MathF.Floor(_pos.x / 40f);
            _posi.y = (int) MathF.Floor(_pos.y / 40f);
            _posi.z = (int) MathF.Floor(_pos.z / 40f);
            
            MouseState mstate = Why.instance.MouseState;
            
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