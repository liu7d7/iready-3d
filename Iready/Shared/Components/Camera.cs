using OpenTK.Mathematics;

namespace Iready.Shared.Components
{
    public class Camera : IreadyObj.Component
    {
        public Vector3 Front;
        public Vector3 Right;
        public Vector3 Up;
        private FloatPos _pos;

        public Camera()
        {
            Front = Vector3.Zero;
            Right = Vector3.Zero;
            Up = Vector3.UnitY;
        }

        public override void Update(IreadyObj objIn)
        {
            base.Update(objIn);

            if (_pos == null)
            {
                _pos = objIn.Get<FloatPos>();
            }
            
            Front = new Vector3(MathF.Cos(_pos.Pitch.ToRadians()) * MathF.Cos(_pos.Yaw.ToRadians()), MathF.Sin(_pos.Pitch.ToRadians()), MathF.Cos(_pos.Pitch.ToRadians()) * MathF.Sin(_pos.Yaw.ToRadians())).Normalized();
            Right = Vector3.Cross(Front, Up).Normalized();
        }
        
        public Matrix4 GetCameraMatrix()
        {
            if (_pos == null)
            {
                return Matrix4.Identity;
            }
            Vector3 pos = new(_pos.LerpedX, _pos.LerpedY, _pos.LerpedZ);
            Matrix4 lookAt = Matrix4.LookAt(pos - Front, pos, Up);
            lookAt.Scale(50f);
            return lookAt;
        }
    }
}