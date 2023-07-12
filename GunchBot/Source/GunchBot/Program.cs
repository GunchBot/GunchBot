namespace GunchBot
{
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using GunchBot.Contracts;
    using GunchBot.Core;
    using GunchBot.StubWeatherModule;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        private const string GunchBotKey = "GUNCHBOT_KEY";
        private const string Prefix = "!";

        private DiscordSocketClient client;
        private CommandService commands;
        private IWeatherApiIntegration api;
        private IServiceProvider services;

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
                client = new DiscordSocketClient(config);
                commands = new CommandService();
                api = new StubWeatherApiIntegration();
                services = new ServiceCollection().AddSingleton(client).AddSingleton(commands).AddSingleton(api).BuildServiceProvider();
                client.Log += ClientLog;

                await RegisterCommandsAsync();
                await client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();
                await Task.Delay(-1);
            }
        }

        private async Task RegisterCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModuleAsync<WeatherModule>(services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(client, message);
            if (message.Author.IsBot) return;

            int argPos = 0;
            var content = message.Content;
            var test = message.HasStringPrefix("!", ref argPos, StringComparison.CurrentCulture);
            if (test || message.HasMentionPrefix(client.CurrentUser, ref argPos))
            {
                var result = await commands.ExecuteAsync(context, argPos, services);
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