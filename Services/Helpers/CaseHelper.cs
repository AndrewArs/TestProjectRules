using System;
using System.Linq;

namespace Services.Helpers
{
    public static class CaseHelper
    {
        public static string SnakeCaseToPascalCase(string name)
        {
            return name.Split(new[] {"_"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }
    }
}
