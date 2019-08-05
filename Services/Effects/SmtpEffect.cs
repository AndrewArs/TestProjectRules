using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DomainModels.Models;
using Dtos.Projects;
using Microsoft.Extensions.Options;
using Services.Extensions;
using Services.Options;

namespace Services.Effects
{
    public class SmtpEffect : IEffect<ProjectDto>
    {
        private readonly EmailService _emailService;
        private readonly SmtpOptions _smtpOptions;

        public SmtpEffect(IOptions<SmtpOptions> options, EmailService emailService)
        {
            _emailService = emailService;
            _smtpOptions = options.Value;
        }

        public async Task Proceed(Effect effect, ICollection<ProjectDto> projects)
        {
            var template = LoadTemplate(effect.TemplateId);

            var result = string.Empty;

            foreach (var project in projects)
            {
                var placeholdersWithValues = project.GetPlaceholdersWithValues(effect.Placeholders);
                result += template.FillTemplate(placeholdersWithValues);
                result += "<br>";
            }

            await _emailService.SendHtmlEmail(_smtpOptions.EmailRecipient, result);
        }

        private static string LoadTemplate(int templateId)
        {
            return File.ReadAllText($"Resources/Templates/Smtp/{templateId}.html");
        }
    }
}
