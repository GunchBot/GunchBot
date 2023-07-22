using System.Net;
using GunchBot.Contracts;
using GunchBot.Utilities;

namespace GunchBot.WeatherService.Nws
{
    /// <summary>
    /// An adapter for the <see cref="Riff.Nws.Net.WeatherService"/>.
    /// </summary>
    public class NwsWeatherService : IWeatherService
    {
        private ILocationService locationService;
        private Riff.Nws.Net.WeatherService weatherService;

        /// <summary>
        /// Creates an instance of <see cref="NwsWeatherService"/>.
        /// </summary>
        /// <param name="locationService">The <see cref="ILocationService"/> that handles converting user specified location into latitude/longitude coordinates.</param>
        public NwsWeatherService(ILocationService locationService)
        {
            this.locationService = locationService;
            this.weatherService = new Riff.Nws.Net.WeatherService("GunchBot"); //todo: move this to a constant
        }

        /// <inheritdoc/>
        public string CurrentWeather(string location, char unit)
        {
            return "Scott's a dumb dumb and implemented forecasts and not current weather first lol";
        }

        /// <inheritdoc/>
        public string Forecast(string location, int days)
        {
            var coordinates = locationService.Geocode(location);

            if (coordinates.IsInvalid())
                return "Location invalid, please try again.";

            try
            {
                var gridpoint = weatherService.GetGridpointsAsync(coordinates.Latitude, coordinates.Longitude).Result;
                var forecast = weatherService.GetForecastAsync(gridpoint).Result;

                if (forecast == null)
                    return "Something went wrong with the NWS request. Try again later.";

                var output = $"Forecast for {gridpoint.Properties.RelativeLocation.Properties.City}, {gridpoint.Properties.RelativeLocation.Properties.State}:\n\n";
                
                //TODO: Move forecast formatting to its own class. Maybe extension methods?
                //The formatting is intended to be discord specific so it might go into... GunchBot.Utilities?
                for (int i = 0; i < days && i < forecast.Properties.Periods.Count; i++)
                {
                    var day = forecast.Properties.Periods[i];
                    output += $"{day.Name}\n\t{(day.IsDaytime ? "⬆️ Highs" : "⬇️ Lows")} around {day.Temperature}°{day.TemperatureUnit}." +
                              $"\n\t💨 Winds {day.WindDirection} at {day.WindSpeed}." + // TODO: need to see what happens if there's... no winds?
                              $"\n\t{GetEmojiFromForecast(day.ShortForecast, day.IsDaytime)} {day.ShortForecast} {(day.ProbabilityOfPrecipitation.Value.HasValue ? $"({day.ProbabilityOfPrecipitation.Value}%)." : "")}\n\n";
                }

                return output;
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
                {
                    return $"Oh no! 🙁\nThe specified location (\"{location}\" ({coordinates.Latitude.ToString("0.####")}, {coordinates.Longitude.ToString("0.####")})) " +
                           $"was not recognized by the NWS api, possibly because it is not within the US.";
                }
                return "Something went wrong with the NWS request. Try again later.";
            }
        }

        private static string GetEmojiFromForecast(string shortForecast, bool daytime)
        {
            //TODO: add logic to split and provide multiple emojis when weather lists changes
            //e.g. "Chance Showers And Thunderstorms then Mostly Sunny"
            if (shortForecast.ToLower().Contains("thunderstorms"))
            {
                return "⛈️";
            }
            if (shortForecast.ToLower().Contains("rain") || shortForecast.Contains("showers"))
            {
                return "🌧️";
            }
            if(shortForecast.ToLower().Contains("cloudy"))
            {
                if (shortForecast.ToLower().Contains("partly"))
                {
                    return daytime ? "⛅" : "☁️";
                }
                return "☁️";
            }
            return daytime ? "🌞" : "🌝"; //todo: gimme them moonphases
        }
    }
}