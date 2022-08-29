using OpenTK.Mathematics;

namespace Iready.Shared.Tweens
{
    public class Tween : BaseTween
    {
        public Animations.Animation Animation;
        public override float Output() => MathHelper.Clamp(Infinite ? Animation(Duration, (Environment.TickCount - LastActivation) % Duration) : Animation(Duration, Environment.TickCount - LastActivation), 0, 1);
        public override float OutputAt(float time)
        {
            if (time < LastActivation)
            {
                return 0;
            }

            if (time > LastActivation + Duration && !Infinite)
            {
                return 1;
            }

            return MathHelper.Clamp(Infinite ? Animation(Duration, (time - LastActivation) % Duration) : Animation(Duration, time - LastActivation), 0, 1);
        }

        public Tween(Animations.Animation animation, float duration, bool repeating)
        {
            LastActivation = Environment.TickCount;
            Animation = animation;
            Infinite = repeating;
            Duration = duration;
        }

        public override bool Done() => Environment.TickCount - LastActivation > Duration;
        public bool PastHalf => Environment.TickCount - LastActivation > Duration / 2;
    }
}