namespace GunchBot.Bot
{
    using Discord;
    using Discord.Commands;
    using Discord.Net;
    using Discord.WebSocket;
    using GunchBot.Bot.Commands;
    using GunchBot.Bot.Modules;
    using GunchBot.Contracts;
    using GunchBot.LocationService.BingMaps;
    using GunchBot.WeatherService.Nws;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        private const string DiscordBotKeyName = "GUNCHBOT_KEY";
        private const string BingMapsApiKeyName = "BINGMAPS_KEY";

        private IServiceProvider services;

        private Program()
        {
            // TODO: is this the best way to do this? It keeps the keys out of code but also 🤔
            var bingMapsApiKey = Environment.GetEnvironmentVariable(BingMapsApiKeyName) ?? throw new KeyNotFoundException("Bing Maps API Key not present in Environment Variables."); ;
            services = new ServiceCollection()
                .AddSingleton<DiscordSocketClient>(_ => new DiscordSocketClient(new DiscordSocketConfig()
                {
                    GatewayIntents = GatewayIntents.All // TODO: he don't need all this. Issue ID: GUN-6
                    // Send messages, read messages, send emojis, create slash commands...
                    // once the bot is closer to done I'll make a point to cull the
                    // permissions down to what he needs.
                }))
                .AddSingleton<CommandService>()
                .AddSingleton<ILocationService>(_ => new BingMapsLocationService(bingMapsApiKey))
                .AddSingleton<IWeatherService, NwsWeatherService>()
                .AddSingleton<ISlashCommandManager, SlashCommandManager>()
                .BuildServiceProvider();
        }

        public static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private async Task RunBotAsync()
        {
            var discordApiKey = Environment.GetEnvironmentVariable(DiscordBotKeyName) ?? throw new KeyNotFoundException("Discord API Key not present in Environment Variables.");
            var client = services.GetRequiredService<DiscordSocketClient>();
            var commands = services.GetRequiredService<ISlashCommandManager>();

            client.Log += ClientLog;
            client.Ready += commands.CreateCommands;
            client.SlashCommandExecuted += commands.HandleCommands;

            await client.LoginAsync(TokenType.Bot, discordApiKey);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private Task ClientLog(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
    }
}