namespace Iready.Shared
{
    public sealed class Formatting
    {

        public static readonly Formatting BLACK = new(0, '0');
        public static readonly Formatting DARKBLUE = new(0xff0000aa, '1');
        public static readonly Formatting DARKGREEN = new(0xff00aa00, '2');
        public static readonly Formatting DARKCYAN = new(0xff00aaaa, '3');
        public static readonly Formatting DARKRED = new(0xffaa0000, '4');
        public static readonly Formatting DARKPURPLE = new(0xffaa00aa, '5');
        public static readonly Formatting GOLD = new(0xffffaa00, '6');
        public static readonly Formatting GRAY = new(0xffaaaaaa, '7');
        public static readonly Formatting DARKGRAY = new(0xff555555, '8');
        public static readonly Formatting BLUE = new(0xff5555ff, '9');
        public static readonly Formatting GREEN = new(0xff55ff55, 'a');
        public static readonly Formatting CYAN = new(0xff55ffff, 'b');
        public static readonly Formatting RED = new(0xffff5555, 'c');
        public static readonly Formatting PURPLE = new(0xffff55ff, 'd');
        public static readonly Formatting YELLOW = new(0xffffff55, 'e');
        public static readonly Formatting WHITE = new(0xffffffff, 'f');
        public static readonly Formatting RESET = new(0, 'r');

        public uint Color;
        public uint Code;

        public static Dictionary<char, Formatting> Values = new();

        Formatting(uint color, char code)
        {
            Color = color;
            Code = code;
            Values[code] = this;
        }
    }
}