using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TelegramBot
{
    class Program
    {
        private static ITelegramBotClient _botClient;

        static void Main(string[] args)
        {
            _botClient = new TelegramBotClient("891321612:AAF5pSS3qlTYx74vV588pFXQ-3EgzKap2ic");
            _botClient.OnMessage += _botClient_OnMessage;
            _botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        private static async void _botClient_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                await _botClient.SendTextMessageAsync(
                    e.Message.Chat,
                    $"Your chat id: {e.Message.Chat.Id}"
                );
            }
        }
    }
}
