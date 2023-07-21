namespace GunchBot.WeatherService.Stub
{
    using GunchBot.Contracts;

    /// <summary>
    /// A basic integration of the weather service to serve as a test bed.
    /// </summary>
    public class StubWeatherApiIntegration : IWeatherService
    {
        private readonly ILocationService locationService;

        /// <summary>
        /// Creates an instance of <see cref="StubWeatherApiIntegration"/>.
        /// </summary>
        /// <param name="locationService">The <see cref="ILocationService"/> that handles converting user specified location into latitude/longitude coordinates.</param>
        public StubWeatherApiIntegration(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        /// <inheritdoc/>
        public string CurrentWeather(string location, char unit)
        {
            var coordinates = locationService.Geocode(location);

            return $"It is currently fake°{unit} in {location} ({coordinates.latitude}, {coordinates.longitude}).";
        }
    }
}