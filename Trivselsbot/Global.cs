using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Trivselsbot.Core.UserAccounts;

namespace Trivselsbot
{
    internal static class Global
    {
        internal static string pathAlerts = "SystemLang/alerts.json";
        internal static string pathProfanity = "SystemLang/profanity.json";
        internal static DiscordSocketClient client { get; set; }
        internal static ulong MessageIdToTrack { get; set; }

        internal static async Task autoWarn(SocketUser user)
        {
            var useraccount = UserAccounts.GetAccount(user);
            var dmChannel = await user.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(
                "Du har skrevet et ord der er forbudt i chatten og har fået en advarsel");
            useraccount.NoOfWarnings++;
            UserAccounts.SaveAccounts();
            await dmChannel.SendMessageAsync("Du har nu " + useraccount.NoOfWarnings + " advarsler");

            if (useraccount.NoOfWarnings % 5 == 0)
            {
                //TODO send email to parents
                await dmChannel.SendMessageAsync("Jeg har sendt en mail til dine forældre!");
            }
        }

    }
}
