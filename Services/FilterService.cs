using System;
using System.IO;
using System.Linq.Expressions;
using DomainModels.Models;
using Dtos.Projects;
using Newtonsoft.Json;

namespace Services
{
    public class FilterService
    {
        public ProjectDto[] FilterProjects(ProjectsDto projects)
        {
            var rules = LoadRules();

            projects.Projects[0].Categories.

            return projects.Projects;
        }

        public RulesList LoadRules()
        {
            var rulesJson = File.ReadAllText(@"~\..\..\rules.json");
            var rules = JsonConvert.DeserializeObject<RulesList>(rulesJson);

            return rules;
        }

        public void CreateFilter(Rule rule)
        {
            foreach (var condition in rule.Conditions)
            {
                var left = Expression.Parameter(typeof(object), condition.Key);
                var right = Expression.Constant(condition.Val);

                switch (condition.Condition)
                {
                    case "equal":
                        Expression.Equal(left, right);
                        break;
                    case "inArray":
                        Expression.Call(typeof(Array), "Contains", new Type[] { },left, right);
                        break;
                    case "moreThan":
                        Expression.GreaterThan(left, right);
                        break;
                    case "lessThan":
                        Expression.LessThan(left, right);
                        break;
                    default: throw new ArgumentException($"Wrong condition: {condition.Condition}");
                }
            }
        }
    }
}
