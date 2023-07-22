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
                output += forecast.GetFormattedForecast(days);
                
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
    }
}