namespace GunchBot
{
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using GunchBot.Modules;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    public class Program
    {
        private const string GunchBotKey = "GUNCHBOT_KEY";
        private const char Prefix = '!';

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private async Task RunBotAsync()
        {
            var token = Environment.GetEnvironmentVariable(GunchBotKey);
            if(token != null)
            {
                var config = new DiscordSocketConfig()
                {
                    GatewayIntents = GatewayIntents.All // TODO: he don't need all this
                };
                _client = new DiscordSocketClient(config);
                _commands = new CommandService();
                _services = new ServiceCollection().AddSingleton(_client).AddSingleton(_commands).BuildServiceProvider();
                _client.Log += ClientLog;

                await RegisterCommandsAsync();
                await _client.LoginAsync(TokenType.Bot, token);
                await _client.StartAsync();
                await Task.Delay(-1);
            }
        }

        private async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModuleAsync<Commands>(_services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;

            int argPos = 0;
            var content = message.Content;
            var test = message.HasStringPrefix("!", ref argPos, StringComparison.CurrentCulture);
            if (test || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if(!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }

        private Task ClientLog(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
    }
}