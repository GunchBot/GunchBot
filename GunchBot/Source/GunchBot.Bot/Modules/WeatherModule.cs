namespace GunchBot.Bot.Modules
{
    using Discord.Commands;
    using GunchBot.Contracts;

    /// <summary>
    /// Handles interpreting weather-based commands.
    /// </summary>
    public class WeatherModule : ModuleBase<SocketCommandContext>
    {
        private IWeatherService weatherApi;

        /// <summary>
        /// Creates an instance of <see cref="WeatherModule"/>.
        /// </summary>
        /// <param name="api">The weather API to make calls to.</param>
        public WeatherModule(IWeatherService api)
        {
            this.weatherApi = api;
        }

        [Command("currentweather")]
        public async Task CurrentWeather(params string[] parameters)
        {
            var location = string.Join(' ', parameters);
            await ReplyAsync(weatherApi.CurrentWeather(location, 'F'));
        }

        [Command("forecast")]
        public async Task Forecast(params string[] parameters)
        {
            var location = string.Join(' ', parameters);
            await ReplyAsync(weatherApi.Forecast(location, 3)); // TODO: un-hardcode the number of days.
        }
    }
}