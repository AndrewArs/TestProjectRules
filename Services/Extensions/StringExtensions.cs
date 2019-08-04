using System.Collections.Generic;

namespace Services.Extensions
{
    internal static class StringExtensions
    {
        public static string FillTemplate(this string template, Dictionary<string, object> placeholders)
        {
            foreach (var placeholder in placeholders)
            {
                template = template.Replace($"%{placeholder.Key}%", placeholder.Value.ToString());
            }

            return template;
        }
    }
}
