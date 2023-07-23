using Discord.Net;
using Discord.WebSocket;
using GunchBot.Contracts;
using Newtonsoft.Json;

namespace GunchBot.Bot.Commands
{
    /// <summary>
    /// Handles creating and running slash commands.
    /// </summary>
    public class SlashCommandManager : ISlashCommandManager
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
        /// TODO: Decouple this from DiscordSocketClient: GUN-19
        /// </remarks>
        public SlashCommandManager(DiscordSocketClient client, IWeatherService weatherService)
        {
            this.weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.commands = new List<ISlashCommandRunner>(); 
        }

        /// <inheritdoc/>
        public async Task CreateCommands()
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
                    // TODO: Also, this should be moved elsewhere to decouple this from the DiscordSocketClient as well. GUN-19
                }
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }

        /// <inheritdoc/>
        public async Task HandleCommands(SocketSlashCommand command)
        {
            var commandRunner = commands.FirstOrDefault(c => c.CommandName == command.CommandName);
            if (commandRunner != null)
            {
                await commandRunner.RunCommand(command);
            }
        }
    }
}
