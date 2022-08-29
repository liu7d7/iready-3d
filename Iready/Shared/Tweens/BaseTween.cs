namespace Iready.Shared.Tweens
{
    public abstract class BaseTween
    {
        public float Duration;
        public float LastActivation = Environment.TickCount;
        public bool Infinite = false;
        
        public abstract float Output();
        public abstract float OutputAt(float time);
        public abstract bool Done();
    }
}