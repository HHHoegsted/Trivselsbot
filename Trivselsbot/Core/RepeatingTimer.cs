using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Discord.WebSocket;

namespace Trivselsbot.Core
{
    internal static class RepeatingTimer
    {
        private static Timer _loopingTimer;
        private static SocketTextChannel channel;

        internal static Task StartTimer()
        {
            channel = Global.client.GetGuild(507928384584548352).GetTextChannel(507928384584548354);
            _loopingTimer = new Timer()
            {
                Interval = 5000,
                AutoReset = true,
                Enabled = true
            };
            Console.WriteLine("Timer Started");
            _loopingTimer.Elapsed += OnTimerTicked;

            return Task.CompletedTask;
        }

        private static async void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            //await channel.SendMessageAsync("Ping!");
        }
    }
}
