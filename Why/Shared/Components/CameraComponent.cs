using OpenTK.Mathematics;
using Why.Game;

namespace Why.Shared.Components
{
    public class CameraComponent : WhyObj.Component
    {
        public Vector3 front;
        public Vector3 right;
        public Vector3 up;
        private FloatPosComponent? _pos;

        public CameraComponent()
        {
            front = Vector3.Zero;
            right = Vector3.Zero;
            up = Vector3.UnitY;
        }

        public override void update(WhyObj objIn)
        {
            base.update(objIn);

            if (_pos == null)
            {
                _pos = objIn.getComponent<FloatPosComponent>();
            }
            
            front = new Vector3(MathF.Cos(_pos.pitch.toRadians()) * MathF.Cos(_pos.yaw.toRadians()), MathF.Sin(_pos.pitch.toRadians()), MathF.Cos(_pos.pitch.toRadians()) * MathF.Sin(_pos.yaw.toRadians())).Normalized();
            right = Vector3.Cross(front, up).Normalized();
        }
        
        public Matrix4 getCameraMatrix()
        {
            if (_pos == null)
            {
                return Matrix4.Identity;
            }
            Vector3 pos = new(_pos.x, _pos.y, _pos.z);
            Console.WriteLine($"{_pos.x} {_pos.y} {_pos.z} {_pos.yaw} {_pos.pitch}");
            return Matrix4.LookAt(pos, pos + front, up);
        }
    }
}