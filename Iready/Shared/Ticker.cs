namespace Iready.Shared
{
    public static class Ticker
    {
        public static float TickTime;
        public static float TickDelta;
        public static float LastFrame;
        private static long _start;
        private static float _prevTimeMs;

        public static void Init()
        {
            _start = Environment.TickCount;
            TickTime = 50.0f;
        }

        public static int Update()
        {
            float timeMillis = Environment.TickCount - _start;
            LastFrame = (timeMillis - _prevTimeMs) / TickTime;
            _prevTimeMs = timeMillis;
            TickDelta += LastFrame;
            int i = (int) TickDelta;
            TickDelta -= i;
            return i;
        }
    }
}