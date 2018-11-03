using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Trivselsbot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("hej")]
        public async Task Echo()
        {
            string msg = Utilities.getFormattedAlert("Greeting_&USER", Context.User.Username.ToString());
            await Context.Channel.SendMessageAsync(msg);
        }

        [Command("vælg")]
        public async Task Choose([Remainder]string message)
        {
            string[] options = message.Split(new char[]{'|', ','}, StringSplitOptions.RemoveEmptyEntries);
            string choice, title;
            Random ran = new Random();
            if (options.Length > 1)
            {
                choice = options[ran.Next(0, options.Length)];
                title = "Jeg har valgt";
            }
            else
            {
                choice = "Der er ikke så meget at vælge imellem.....";
                title = "Øhhhhh!";
            }

            var embed = new EmbedBuilder();
            embed.WithTitle(title);
            embed.WithDescription(choice);
            embed.WithThumbnailUrl(Context.User.GetAvatarUrl());
            embed.WithColor(new Color(0, 255, 255));
            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
