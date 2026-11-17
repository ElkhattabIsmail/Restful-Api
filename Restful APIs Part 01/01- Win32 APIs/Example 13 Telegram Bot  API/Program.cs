using System.Threading.Tasks;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    private static readonly string BotToken = "My_Bot_Tocken_Is_Secret";

    static async Task Main(string[] args)
    {
        var botClient = new TelegramBotClient(BotToken);

        // Start polling for updates
        var offset = 0;
        while (true)
        {
            var updates = await botClient.GetUpdatesAsync(offset);

            foreach (var update in updates)
            {
                if (update.Type == UpdateType.Message)
                {
                    var message = update.Message;
                    var chatId = message.Chat.Id;
                    var text = message.Text;

                    Console.WriteLine($"Received: {text} from Chat ID: {chatId}");

                    // Handle the /start command
                    if (text == "/start")
                    {
                        await botClient.SendTextMessageAsync(chatId, "Hello!\n I'm Mr.Quack, How can I help you?");
                    }
                    // Echo user messages
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId, $"Quack!  🦆\n\nYou said: {text}");
                    }
                }

                offset = update.Id + 1;
            }

            await Task.Delay(1000); // Wait for 1 second before polling again
        }
    }
}
