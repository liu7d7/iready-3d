using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Iready
{
    public static class Program
    {
        // ReSharper disable once InconsistentNaming
        [STAThread]
        public static void Main(string[] args)
        {
            NativeWindowSettings nativeWindowSettings = new()
            {
                Size = new Vector2i(1152, 720),
                Title = "Iready",
                Flags = ContextFlags.ForwardCompatible
            };

            using Iready window = new(GameWindowSettings.Default, nativeWindowSettings);
            window.Run();
        }
    }
}