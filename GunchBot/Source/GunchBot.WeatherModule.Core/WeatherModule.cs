namespace GunchBot.Core
{
    using Discord.Commands;
    using GunchBot.Contracts;

    public class WeatherModule : ModuleBase<SocketCommandContext>
    {
        private IWeatherApiIntegration api;

        public WeatherModule(IWeatherApiIntegration api)
        {
            this.api = api;
        }

        [Command("currentweather")]
        public async Task CurrentWeather(string location, char unit)
        {
            await ReplyAsync(api.CurrentWeather(location, unit));
        }
    }
}