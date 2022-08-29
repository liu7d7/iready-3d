using OpenTK.Mathematics;

namespace Iready.Shared.Tweens
{
    public class FromToTween : BaseTween
    {
        public Animations.Animation Animation;
        public float From;
        public float To;

        public FromToTween(Animations.Animation animation, float from, float to, float duration)
        {
            Animation = animation;
            LastActivation = Environment.TickCount;
            From = from;
            To = to;
            Duration = duration;
        }

        public override float Output()
        {
            return MathHelper.Lerp(From, To, Animation(Duration, Environment.TickCount - LastActivation));
        }

        public override float OutputAt(float time)
        {
            if (time < LastActivation)
            {
                return From;
            }

            if (time > LastActivation + Duration)
            {
                return To;
            }

            return MathHelper.Lerp(From, To, Animation(Duration, time - LastActivation));
        }

        public override bool Done()
        {
            return Environment.TickCount - LastActivation > Duration;
        }
    }
}