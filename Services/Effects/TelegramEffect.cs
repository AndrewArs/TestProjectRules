using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Models;
using Dtos.Projects;
using Microsoft.Extensions.Options;
using Services.Extensions;
using Services.Helpers;
using Services.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace Services.Effects
{
    public class TelegramEffect : IEffect<ProjectDto>
    {
        private readonly TelegramBotClient _botClient;
        private readonly TelegramOptions _telegramOptions;

        public TelegramEffect(IOptions<TelegramOptions> options, TelegramBotClient botClient)
        {
            _botClient = botClient;
            _telegramOptions = options.Value;
        }

        public async Task Proceed(Effect effect, ICollection<ProjectDto> projects)
        {
            var chatId = new ChatId(_telegramOptions.ChatId);
            var template = LoadTemplate(effect.TemplateId);

            var result = string.Empty;

            foreach (var project in projects)
            {
                var placeholdersWithValues = GetPlaceholdersWithValues(project, effect.Placeholders);
                result += template.FillTemplate(placeholdersWithValues);
                result += Environment.NewLine;
            }

            await _botClient.SendTextMessageAsync(chatId, result);
        }

        private Dictionary<string, object> GetPlaceholdersWithValues(ProjectDto project, Dictionary<string, string> placeholders)
        {
            var result = new Dictionary<string, object>();

            foreach (var placeholder in placeholders)
            {
                var property = CaseHelper.SnakeCaseToPascalCase(placeholder.Value);
                var value = project.GetPropValue(property);
                result.Add(placeholder.Key, value);
            }

            return result;
        }

        private static string LoadTemplate(int templateId)
        {
            return File.ReadAllText($"~\\..\\..\\Resources\\Templates\\Telegram\\{templateId}.txt");
        }
    }
}
