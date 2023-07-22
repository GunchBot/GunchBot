using System.Net;
using GunchBot.Contracts;
using GunchBot.Utilities;
using Riff.Nws.Net.Metadata;

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
                output += GetFormattedForecast(forecast, days);
                
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

        //TODO: Move forecast formatting to its own class. Maybe extension methods?
        //The formatting is intended to be discord specific so it might go into... GunchBot.Utilities?
        private static string GetFormattedForecast(Forecast forecast, int days)
        {
            var output = string.Empty;

            var periods = forecast.Properties.Periods.ToList();

            var firstEntry = periods.FirstOrDefault(); // should never be null. If it is, the exception gets caught elsewhere.
            if (!firstEntry.IsDaytime)
            {
                output +=
                    $"{firstEntry.Name}\n\t⬇️ Lows around {firstEntry.Temperature}°{firstEntry.TemperatureUnit}." +
                    $"\n\t{GetEmojiFromForecast(firstEntry.ShortForecast, firstEntry.IsDaytime)} {firstEntry.ShortForecast}.{(firstEntry.ProbabilityOfPrecipitation.Value.HasValue ? $" ({firstEntry.ProbabilityOfPrecipitation.Value}%)." : "")}" +
                    $" Winds {firstEntry.WindDirection} at {firstEntry.WindSpeed}." + // TODO: need to see what happens if there's... no winds?
                    $"\n\n";

                periods.Remove(firstEntry);
                days--;
            }

            for (int i = 0; i < days && i * 2 + 1< forecast.Properties.Periods.Count; i++) // this logic is a mess
            {
                var day = forecast.Properties.Periods[i * 2];
                var evening = forecast.Properties.Periods[i * 2 + 1];
                var date = day.StartTime.ToString("dd MMMM");
                output += 
                    $"{day.Name} ({date}):" +
                    $"\n\t⬆️ High: {day.Temperature}°{day.TemperatureUnit}, ⬇️ Low: {evening.Temperature}°{evening.TemperatureUnit}." +
                    $"\n\tDuring the day:" +
                    $"\n\t\t{GetEmojiFromForecast(day.ShortForecast, day.IsDaytime)} {day.ShortForecast}.{(day.ProbabilityOfPrecipitation.Value.HasValue ? $" ({day.ProbabilityOfPrecipitation.Value}%)." : "")}" +
                    $" Winds {day.WindDirection} at {day.WindSpeed}." + // TODO: need to see what happens if there's... no winds?
                    $"\n\tAt night:" +
                    $"\n\t\t{GetEmojiFromForecast(evening.ShortForecast, evening.IsDaytime)} {evening.ShortForecast}.{(evening.ProbabilityOfPrecipitation.Value.HasValue ? $" ({evening.ProbabilityOfPrecipitation.Value}%)." : "")}" +
                    $" Winds {evening.WindDirection} at {evening.WindSpeed}." + // TODO: need to see what happens if there's... no winds?
                    $"\n\n";
            }

            return output;
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