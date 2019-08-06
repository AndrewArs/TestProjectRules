using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Models;
using DomainModels.Models.Templates;
using Dtos.Projects;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
            var subject = string.Empty;

            foreach (var project in projects)
            {
                var placeholdersWithValues = project.GetPlaceholdersWithValues(effect.Placeholders);
                subject += $"{template.Subject.FillTemplate(placeholdersWithValues)};";
                result += $"{template.Body.FillTemplate(placeholdersWithValues)}<br>";
            }

            await _emailService.SendHtmlEmail(_smtpOptions.EmailRecipient, subject, result);
        }

        private static SmtpTemplate LoadTemplate(int templateId)
        {
             var templates = File.ReadAllText("Resources/Templates/smtp.json");

            return JsonConvert.DeserializeObject<IEnumerable<SmtpTemplate>>(templates).FirstOrDefault(t => t.Id == templateId);
        }
    }
}
