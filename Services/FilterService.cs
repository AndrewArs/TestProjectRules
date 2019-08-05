using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Models;
using Dtos.Projects;
using Newtonsoft.Json;
using Services.Effects;

namespace Services
{
    public class FilterService
    {
        private readonly ExpressionBuilder<ProjectDto> _expressionBuilder;
        private readonly TelegramEffect _telegramEffect;
        private readonly SmtpEffect _smtpEffect;

        public FilterService(ExpressionBuilder<ProjectDto> expressionBuilder, 
            TelegramEffect telegramEffect, 
            SmtpEffect smtpEffect)
        {
            _expressionBuilder = expressionBuilder;
            _telegramEffect = telegramEffect;
            _smtpEffect = smtpEffect;
        }

        public async Task FilterProjects(ProjectsDto projects)
        {
            var rules = LoadRules();
            var effects = new List<Task>();

            foreach (var rule in rules.Rules)
            {
                var exp = _expressionBuilder.BuildExpression(rule);
                var func = exp.Compile();
                var filteredProjects = projects.Projects.Where(func).ToList();

                foreach (var effect in rule.Effects)
                {
                    effects.Add(ApplyEffect(effect, filteredProjects));
                }
            }

            await Task.WhenAll(effects);
        }

        private static RulesList LoadRules()
        {
            var rulesJson = File.ReadAllText(@"Resources/rules.json");
            var rules = JsonConvert.DeserializeObject<RulesList>(rulesJson);

            return rules;
        }

        private async Task ApplyEffect(Effect effect, ICollection<ProjectDto> projects)
        {
            switch (effect.Type)
            {
                case Constants.TelegramEffect:
                    await _telegramEffect.Proceed(effect, projects);
                    break;
                case Constants.SmtpEffect:
                    await _smtpEffect.Proceed(effect, projects);
                    break;
                default: throw new ArgumentException($"Wrong effect type: {effect.Type}");
            }
        }
    }
}
