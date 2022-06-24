using OpenTK.Mathematics;
using Why.Game;

namespace Why.Shared.Components
{
    public class FloatPosComponent : WhyObj.Component
    {
        public float x;
        public float y;
        public float z;
        public float yaw;
        public float pitch;

        public FloatPosComponent()
        {
            x = y = z = yaw = pitch = 0;
        }
    }
}