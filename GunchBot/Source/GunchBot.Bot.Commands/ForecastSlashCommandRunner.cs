using Discord;
using GunchBot.Contracts;

namespace GunchBot.Bot.Commands
{
    public class ForecastSlashCommandRunner : ISlashCommandRunner
    {
        private const string name = "forecast";
        private const string description = "Gets the forecast for the specified area.";

        private IWeatherService weatherService;

        /// <summary>
        /// Creates an instance of <see cref="ForecastSlashCommandRunner"/>.
        /// </summary>
        /// <param name="weatherService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ForecastSlashCommandRunner(IWeatherService weatherService)
        {
            this.weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
        }

        /// <inheritdoc/>
        public string CommandName => name;

        /// <inheritdoc/>
        public string CommandDescription => description;

        /// <inheritdoc/>
        public ApplicationCommandProperties Build()
        {
            return new SlashCommandBuilder().WithName(name)
                .WithDescription(description)
                .AddOption("location", ApplicationCommandOptionType.String, "The desired forecast location.", isRequired: true)
                .AddOption("days", ApplicationCommandOptionType.Integer, "The desired number of days.", isRequired: false, minValue: 1, maxValue: 7)
                .Build();
        }


        /// <inheritdoc/>
        public async Task RunCommand(ISlashCommandInteraction slashCommand)
        {
            int defaultDays = 3;
            var locationOption = slashCommand.Data.Options.FirstOrDefault(o => o.Name == "location");
            var daysOption = slashCommand.Data.Options.FirstOrDefault(o => o.Name == "days");

            if (locationOption is { Value: string })
            {
                var location = locationOption.Value;
                var days = defaultDays;
                if (daysOption != null)
                {
                    days = Convert.ToInt32(daysOption.Value);
                }

                await slashCommand.RespondAsync(text: weatherService.Forecast((string)location, days));
            }
        }
    }
}
