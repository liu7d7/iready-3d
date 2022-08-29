namespace Iready.Shared.Tweens
{
    public class StaticTween : BaseTween
    {
        private float _output;

        public StaticTween(float output, float duration)
        {
            _output = output;
            Duration = duration;
        }
        
        public override float Output()
        {
            return _output;
        }

        public override float OutputAt(float time)
        {
            return _output;
        }

        public override bool Done()
        {
            return Environment.TickCount - LastActivation > Duration;
        }
    }
}