namespace Iready.Shared
{
    public static class Animations
    {
        public delegate float Animation(float duration, float time);

        public static float EaseInOut(float duration, float time)
        {
            float x1 = time / duration;
            return 6 * MathF.Pow(x1, 5) - 15 * MathF.Pow(x1, 4) + 10 * MathF.Pow(x1, 3);
        }

        public static float Decelerate(float duration, float time)
        {
            float x1 = time / duration;
            return 1 - (x1 - 1) * (x1 - 1);
        }
        
        public static Animation UpAndDown(Animation func)
        {
            return (duration, time) => time > duration / 2 ? func(duration / 2, duration - time) : func(duration / 2, time);
        }

        public static Animation BackHalfFullFrontHalf(Animation func)
        {
            return (duration, time) => time < duration / 4f ? 1 - func(duration / 2f, time + duration / 4f) : time < duration * 3f / 4 ? func(duration / 2, time - duration / 4) : 1 - func(duration / 2, time - duration / 4 * 3);
        }
    }
}