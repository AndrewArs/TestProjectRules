namespace Services.Extensions
{
    internal static class ObjectExtensions
    {
        public static object GetPropValue(this object src, string propName)
        {
            return src.GetType().GetProperty(propName)?.GetValue(src);
        }
    }
}
