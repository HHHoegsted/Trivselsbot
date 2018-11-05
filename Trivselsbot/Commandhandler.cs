using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Trivselsbot.Core.UserAccounts;
using Trivselsbot.Modules;

namespace Trivselsbot
{
    class Commandhandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();

            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;
            var context = new SocketCommandContext(_client, msg);

            //mute check

            var useraccount = UserAccounts.GetAccount(context.User);

            if (useraccount.IsMuted)
            {
                await context.Message.DeleteAsync();
                return;
            }

            foreach (var profanity in Utilities.profanity)
            {
                if (s.Content.Contains(profanity))
                {
                    await context.Message.DeleteAsync();
                    Global.autoWarn(context.User);
                    return;
                }
            }

            int argPos = 0;
            if (msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
