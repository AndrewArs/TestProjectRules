using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Extensions
{
    internal static class StringExtensions
    {
        public static string FillTemplate(this string template, Dictionary<string, object> placeholders)
        {
            foreach (var placeholder in placeholders)
            {
                template = template.Replace($"%{placeholder.Key}%", placeholder.Value?.ToString() ?? string.Empty);
            }

            return template;
        }

        public static string SnakeCaseToPascalCase(this string name)
        {
            return name.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }
    }
}
