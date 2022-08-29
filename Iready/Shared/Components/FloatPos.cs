using OpenTK.Mathematics;

namespace Iready.Shared.Components
{
    public class FloatPos : IreadyObj.Component
    {
        public float X;
        public float PrevX;
        public float Y;
        public float PrevY;
        public float Z;
        public float PrevZ;
        public float Yaw;
        public float PrevYaw;
        public float Pitch;
        public float PrevPitch;
        public float LerpedX => Util.Lerp(PrevX, X, Ticker.TickDelta);
        public float LerpedY => Util.Lerp(PrevY, Y, Ticker.TickDelta);
        public float LerpedZ => Util.Lerp(PrevZ, Z, Ticker.TickDelta);

        public FloatPos()
        {
            X = PrevX = Y = PrevY = Z = PrevZ = Yaw = PrevYaw = Pitch = PrevPitch = 0;
        }

        public Vector3 ToVec3()
        {
            return new(X, Y, Z);
        }

        public Vector3 ToLerpedVec3(float xOff, float yOff, float zOff)
        {
            return new(LerpedX + xOff, LerpedY + yOff, LerpedZ + zOff);
        }

        public void SetPrev()
        {
            PrevX = X;
            PrevY = Y;
            PrevZ = Z;
            PrevYaw = Yaw;
            PrevPitch = Pitch;
        }
    }
}