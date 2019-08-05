using System;
using System.Collections.Generic;
using System.Linq;
using Dtos.Projects;

namespace Services.Extensions
{
    public static class ProjectExtensions
    {
        private static readonly string[] PlaceholderstranslateToDate = {"created_at", "modified_at"};

        public static Dictionary<string, object> GetPlaceholdersWithValues(this ProjectDto project, Dictionary<string, string> placeholders)
        {
            var result = new Dictionary<string, object>();

            foreach (var placeholder in placeholders)
            {
                var property = placeholder.Value.SnakeCaseToPascalCase();
                var value = project.GetPropValue(property);

                if (PlaceholderstranslateToDate.Contains(placeholder.Value))
                {
                    value = DateTimeOffset.FromUnixTimeSeconds((long)value)
                        .DateTime.ToLocalTime();
                }

                result.Add(placeholder.Key, value);
            }

            return result;
        }
    }
}
