using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace RustFishingBot_GUI.Classes.TelegramBot
{
    internal class TgBot
    {
        private readonly ITelegramBotClient _botClient;

        public TgBot(string token)
        {
            _botClient = new TelegramBotClient(token);
        }

        private async Task<List<long>> GetAllChatIds()
        {
            var chatIds = new List<long>();
            var chats = await _botClient.GetUpdatesAsync();
            foreach (var chat in chats)
            {
                if (chat.Message is not null)
                {
                    if (!chatIds.Contains(chat.Message.Chat.Id))
                    {
                        chatIds.Add(chat.Message.Chat.Id);
                    }
                }
            }
            return chatIds;
        }

        public async Task SendMessageToAllUsers(string message)
        {
            var chatIds = await GetAllChatIds();
            foreach (var chatId in chatIds)
            {
                try
                {
                    await _botClient.SendTextMessageAsync(chatId, message);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error sending message to {chatId}: {e.Message}");
                }
            }
        }
    }
}
