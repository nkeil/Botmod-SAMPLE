using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Discord_BodmodBot
{
    class Program
    {
        public DiscordSocketClient _client;
        CommandHandler _handler;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var config = GetConfiguration(); 
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            _client.UserJoined += AnnounceUserJoined;
            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += MessageReceivedAsync;

            _handler = new CommandHandler();
            await _client.LoginAsync(TokenType.Bot, config["token"]);
            await _client.StartAsync();
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
        }

        public static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
        }

        private async Task AnnounceUserJoined(SocketGuildUser user)
        {
            //announce user join in UCSC server
            if (user.Guild.Id == 473217646884552734)
            {
                var channel = _client.GetChannel(473248037951766563) as SocketTextChannel;
                await channel.SendMessageAsync($"hey {user.Mention}");
                await channel.SendMessageAsync($"say [*major CS, CS:GD, CE, other, or none] to get access to the server");
            }
        }
        
        private Task LogAsync(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");

            return Task.CompletedTask;
        }

        //Test command to make sure bot is running
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            if (message.Content == "!ping")
                await message.Channel.SendMessageAsync("pong!");
        }
    }
}
