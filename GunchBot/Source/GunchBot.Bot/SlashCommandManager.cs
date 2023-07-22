using Discord;
using Discord.Net;
using Discord.WebSocket;
using GunchBot.Contracts;
using Newtonsoft.Json;

namespace GunchBot.Bot
{
    /// <summary>
    /// Handles creating and running slash commands.
    /// </summary>
    internal class SlashCommandManager
    {
        private readonly DiscordSocketClient client;
        private readonly IWeatherService weatherService;
        private readonly IList<ISlashCommandRunner> commands;

        /// <summary>
        /// Creates a new instance of <see cref="SlashCommandManager"/>.
        /// </summary>
        /// <param name="client">The discord socket client.</param>
        /// <param name="weatherService">The weather service to use.</param>
        /// <remarks>
        /// Not a huge fan of this structure but for now it works.
        /// TODO: Dependency Injection? Issue ID: GUN-17
        /// </remarks>
        internal SlashCommandManager(DiscordSocketClient client, IWeatherService weatherService)
        {
            this.weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.commands = new List<ISlashCommandRunner>(); 

            this.client.Ready += CreateCommands;
            this.client.SlashCommandExecuted += HandleCommands;
        }

        internal async Task CreateCommands()
        {
            commands.Add(new ForecastSlashCommandRunner(weatherService));
            try
            {
                foreach (var command in commands)
                {
                    // TODO: Using the ready event is a simple implementation suitable for testing and development.
                    // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
                    // Probably need to look into guild commands and how to... do that.
                    await client.CreateGlobalApplicationCommandAsync(command.Build());
                }
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }

        private async Task HandleCommands(SocketSlashCommand command)
        {
            if (command != null)
            {
                var commandRunner = commands.FirstOrDefault(c => c.CommandName == command.CommandName);
                if (commandRunner != null)
                {
                    await commandRunner.RunCommand(command);
                }
            }
        }
    }
}
