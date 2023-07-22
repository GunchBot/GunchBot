using GunchBot.WeatherService.Nws;

namespace GunchBot.Bot
{
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using GunchBot.Bot.Modules;
    using GunchBot.Contracts;
    using GunchBot.LocationService.BingMaps;
    using GunchBot.WeatherService.Stub;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        private const string DiscordBotKeyName = "GUNCHBOT_KEY";
        private const string BingMapsApiKeyName = "BINGMAPS_KEY";
        private const string Prefix = "!";

        private DiscordSocketClient client;
        private CommandService commands;
        private ILocationService locationService;
        private IWeatherService weatherApi;
        private IServiceProvider services;

        public static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private async Task RunBotAsync()
        {
            // TODO: is this the best way to do this? It keeps the keys out of code but also 🤔
            var discordApiKey = Environment.GetEnvironmentVariable(DiscordBotKeyName);
            var bingMapsApiKey = Environment.GetEnvironmentVariable(BingMapsApiKeyName);
            if (discordApiKey != null && bingMapsApiKey != null)
            {
                var config = new DiscordSocketConfig()
                {
                    GatewayIntents = GatewayIntents.All // TODO: he don't need all this
                };

                client = new DiscordSocketClient(config);
                commands = new CommandService();
                locationService = new BingMapsLocationService(bingMapsApiKey);
                weatherApi = new NwsWeatherService(locationService);
                services = new ServiceCollection().AddSingleton(client).AddSingleton(commands).AddSingleton(weatherApi).BuildServiceProvider();
                client.Log += ClientLog;

                await RegisterCommandsAsync();
                await client.LoginAsync(TokenType.Bot, discordApiKey);
                await client.StartAsync();
                await Task.Delay(-1);
            }
            else
            {
                if(discordApiKey == null)
                {
                    Console.WriteLine("Discord API Key was not present!");
                }
                if (bingMapsApiKey == null)
                {
                    Console.WriteLine("Bing Maps API Key was not present!");
                }
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
            var test = message.HasStringPrefix(Prefix, ref argPos, StringComparison.CurrentCulture);
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