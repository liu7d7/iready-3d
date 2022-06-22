using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Why
{
    public static class Program
    {
        // ReSharper disable once InconsistentNaming
        [STAThread]
        public static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings
            {
                Size = new Vector2i(1152, 720),
                Title = "Why",
                Flags = ContextFlags.ForwardCompatible
            };

            using var window = new Why(GameWindowSettings.Default, nativeWindowSettings);
            window.Run();
        }
    }
}