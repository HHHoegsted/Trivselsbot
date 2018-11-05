using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Trivselsbot
{
    internal static class Global
    {
        internal static string pathAlerts = "SystemLang/alerts.json";
        internal static string pathProfanity = "SystemLang/profanity.json";
        internal static DiscordSocketClient client { get; set; }
        internal static ulong MessageIdToTrack { get; set; }

    }
}
