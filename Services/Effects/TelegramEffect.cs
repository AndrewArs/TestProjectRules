using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Models;
using DomainModels.Models.Templates;
using Dtos.Projects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Services.Extensions;
using Services.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace Services.Effects
{
    public class TelegramEffect : IEffect<ProjectDto>
    {
        private readonly TelegramBotClient _botClient;
        private readonly ILogger<TelegramEffect> _logger;
        private readonly TelegramOptions _telegramOptions;

        public TelegramEffect(IOptions<TelegramOptions> options, 
            TelegramBotClient botClient, 
            ILogger<TelegramEffect> logger)
        {
            _botClient = botClient;
            _logger = logger;
            _telegramOptions = options.Value;
        }

        public async Task Proceed(Effect effect, ICollection<ProjectDto> projects)
        {
            var chatId = new ChatId(_telegramOptions.ChatId);
            var template = LoadTemplate(effect.TemplateId);

            var result = string.Empty;

            foreach (var project in projects)
            {
                var placeholdersWithValues = project.GetPlaceholdersWithValues(effect.Placeholders);
                result += template.Body.FillTemplate(placeholdersWithValues);
                result += Environment.NewLine;
                result += Environment.NewLine;
            }

            _logger.LogInformation($"Sending message to chat {chatId}. {result}");
            await _botClient.SendTextMessageAsync(chatId, result);
        }

        private static TelegramTemplate LoadTemplate(int templateId)
        {
            var templates = File.ReadAllText("Resources/Templates/telegram.json");

            return JsonConvert.DeserializeObject<IEnumerable<TelegramTemplate>>(templates).FirstOrDefault(t => t.Id == templateId);
        }
    }
}
