namespace Why
{
    public static class Extensions
    {
        public static string contentToString<T>(this T[] arr)
        {
            string o = arr.ToString() ?? string.Empty;
            o = o[..^1];
            foreach (var item in arr)
            {
                o += item + ", ";
            }
            o = o[..^2];
            o += "]";
            return o;
        }
    }
}