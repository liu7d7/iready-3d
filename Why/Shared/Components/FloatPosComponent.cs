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
        private float _pitch;

        public float pitch
        {
            get => _pitch;
            set => _pitch = MathHelper.Clamp(value, -90, 90);
        }

        public FloatPosComponent()
        {
            
        }
    }
}