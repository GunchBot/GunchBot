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
        private const string ForecastCommandName = "forecast";
        private const string ForecastCommandDescription = "Gets the forecast for the specified area.";

        private readonly DiscordSocketClient client;
        private readonly IWeatherService weatherService;

        /// <summary>
        /// Creates a new instance of <see cref="SlashCommandManager"/>.
        /// </summary>
        /// <param name="client">The discord socket client.</param>
        /// <param name="weatherService">The weather service to use.</param>
        /// <remarks>Not a huge fan of this structure but for now it works.</remarks>
        internal SlashCommandManager(DiscordSocketClient client, IWeatherService weatherService)
        {
            this.weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.client.Ready += CreateCommands;
            this.client.SlashCommandExecuted += HandleCommands;
        }

        internal async Task CreateCommands()
        {
            var globalCommand = new SlashCommandBuilder().WithName(ForecastCommandName)
                .WithDescription(ForecastCommandDescription)
                .AddOption("location", ApplicationCommandOptionType.String, "The desired forecast location.", isRequired: true)
                .AddOption("days", ApplicationCommandOptionType.Integer, "The desired number of days.", isRequired: false, minValue: 1, maxValue: 7);

            try
            {
                await client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
                // TODO: Using the ready event is a simple implementation suitable for testing and development.
                // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
                // Probably need to look into guild commands and how to... do that.
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
                switch (command.Data.Name)
                {
                    case ForecastCommandName:
                        await RunForecastCommand(command);
                        break;
                }
            }
        }

        //TODO: make a slashcommand object that handles all of this cleaner. I don't really like having
        //command specific logic within the builder. but for testing this is fine.
        private async Task RunForecastCommand(SocketSlashCommand command)
        {
            int defaultDays = 3;
            var locationOption = command.Data.Options.FirstOrDefault(o => o.Name == "location");
            var daysOption = command.Data.Options.FirstOrDefault(o => o.Name == "days");

            if (locationOption is { Value: string })
            {
                var location = locationOption.Value;
                var days = defaultDays;
                if (daysOption != null)
                {
                    days = Convert.ToInt32(daysOption.Value);
                }

                await command.RespondAsync(text: weatherService.Forecast((string)location, days));
            }
        }
    }
}
