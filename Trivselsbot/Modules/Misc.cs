using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.CompilerServices;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Newtonsoft.Json;
using NReco.ImageGenerator;
using Trivselsbot.Core.UserAccounts;
using ImageFormat = NReco.ImageGenerator.ImageFormat;

namespace Trivselsbot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task Kick(IGuildUser user, string reason)
        {
            await user.KickAsync(reason);
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task Ban(IGuildUser user, string reason)
        {
            await user.Guild.AddBanAsync(user, 7, reason);
        }

        [Command("warn")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task Warn(IGuildUser user)
        {
            Global.autoWarn((SocketUser) user);
        }

        [Command("mute")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task Mute(IGuildUser user, string reason = "Der er ikke angivet nogen grund")
        {
            var useraccount = UserAccounts.GetAccount((SocketUser)user);
            useraccount.IsMuted = true;
            UserAccounts.SaveAccounts();
            var dmChannel = await user.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.getAlert("Mute")); 
            await dmChannel.SendMessageAsync(reason);
        }

        [Command("unmute")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task Unmute(IGuildUser user)
        {
            var useraccount = UserAccounts.GetAccount((SocketUser)user);
            useraccount.IsMuted = false;
            UserAccounts.SaveAccounts();
            var dmChannel = await user.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.getAlert("Unmute"));
        }

        [Command("react")]
        public async Task HandleReactionMessage()
        {
            RestUserMessage msg = await Context.Channel.SendMessageAsync("React to me!");
            Global.MessageIdToTrack = msg.Id;
        }

        [Command("pokemon")]
        public async Task showPokemonForNumber([Remainder] string Nr)
        {
            string json;
            string query = "https://pokeapi.co/api/v2/pokemon/" + Nr;
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString(query);
            }

            var pokeObject = JsonConvert.DeserializeObject<dynamic>(json);

            string pokeNr = pokeObject.id.ToString();
            string pokeName = pokeObject.name.ToString();
            string pokeImageURL = pokeObject.sprites.front_default.ToString();

            var embed = new EmbedBuilder();
            embed.WithTitle("Pokemon");
            embed.WithThumbnailUrl(pokeImageURL);
            embed.AddInlineField("Nr", pokeNr);
            embed.AddInlineField("Name", char.ToUpper(pokeName[0]) + pokeName.Substring(1));

            await Context.Channel.SendMessageAsync("Pokemon", embed: embed);
        }

        [Command("person")]
        public async Task getRandomPerson()
        {
            string json;
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://randomuser.me/api/?nat=dk");
            }

            var personObject = JsonConvert.DeserializeObject<dynamic>(json);

            string firstName = personObject.results[0].name.first.ToString();
            string lastName = personObject.results[0].name.last.ToString();
            var pictureURL = personObject.results[0].picture.large.ToString();

            var embed = new EmbedBuilder();
            embed.WithTitle("Person");
            embed.WithThumbnailUrl(pictureURL);
            embed.AddInlineField("Fornavn", char.ToUpper(firstName[0]) + firstName.Substring(1));
            embed.AddInlineField("Efternavn", char.ToUpper(lastName[0]) + lastName.Substring(1));

            await Context.Channel.SendMessageAsync("", embed: embed);
        }


        [Command("hej")]
        public async Task Echo()
        {
            string msg = Utilities.getFormattedAlert("Greeting_&USER", Context.User.Username.ToString());
            await Context.Channel.SendMessageAsync(msg);
        }

        [Command("hello")]
        public async Task Hello()
        {
            string css = "<style>h1{color: red;}</style>";
            string html = String.Format("<h1>Hello {0}</h1>", Context.User.Username);
            var converter = new HtmlToImageConverter
            {
                Width = 300,
                Height = 70
            };
            var jpgBytes = converter.GenerateImage(css+html, ImageFormat.Jpeg);
            await Context.Channel.SendFileAsync(new MemoryStream(jpgBytes), "hello.jpg");
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

        [Command("data")]
        public async Task Data()
        {
            await Context.Channel.SendMessageAsync("Data has " + Datastorage.GetPairsCount() + " pairs");
            //Datastorage.AddPairToStorage("TEST", "Dette er en test");

        }

        [Command("stats")]
        public async Task Stats([Remainder]string arg="")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var account = UserAccounts.GetAccount(target);
            await Context.Channel.SendMessageAsync($"{target.Username} har {account.XP} XP og {account.points} points");
        }

        [Command("fødselsdag")]
        public async Task Birthday([Remainder] string birthday)
        {
            if (birthday.Length == 4 && int.TryParse(birthday, out int result))
            {
                uint day = uint.Parse(birthday.Substring(0, 2));
                uint month = uint.Parse(birthday.Substring(2, 2));
                if ((day < 1 || day > 31) || (month < 1 || month > 12))
                {
                    await Context.Channel.SendMessageAsync("Den dato findes ikke");
                    return;
                }
                UserAccounts.setBirthDay(Context.User, day);
                UserAccounts.setBirthMonth(Context.User, month);
                await Context.Channel.SendMessageAsync("Ændret fødselsdag for " + Context.User.Username + " til " + Utilities.formatterDato(day, month));
            }
            else
            {
                await Context.Channel.SendMessageAsync(
                    "Du skal angive din fødselsdag som DDMM, f.eks. 2310 for 23. oktober");
            }
        }

        [Command("bandeord")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Profanity(string word)
        {
             Utilities.profanity.Add(word);
             Datastorage.saveProfanityData(Utilities.profanity);
        }

        [Command("help")]
        public async Task Help()
        {
            await Context.Channel.SendMessageAsync(
                "start alle kommandoer til Trivselsbot med et udråbstegn, fx. !help");
            await Context.Channel.SendMessageAsync(
                "vælg - vælger mellem flere forskellige muligheder (adskil med komma, fx. \"!vælg hund,kat\")");
            await Context.Channel.SendMessageAsync(
                "pokemon - viser info om pokemon ud fra nr. eller navn (fx. !pokemon Pikachu)");
            await Context.Channel.SendMessageAsync(
                "person - finder på et navn og et billede til en fantasi-person der fx kan bruges til en historie");
            await Context.Channel.SendMessageAsync(
                "fødselsdag - fortæller din fødselsdag til Trivselsbot, fx. !fødselsdag 0308 hvis du har fødselsdag 3. august");
        }
    }
}
