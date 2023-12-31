﻿namespace GunchBot.WeatherService.Stub
{
    using GunchBot.Contracts;
    using GunchBot.Utilities;

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

            if (coordinates.IsInvalid())
            {
                return "Location invalid, please try again.";
            }

            return $"It is currently fake°{unit} in {location} ({coordinates.Latitude}, {coordinates.Longitude}).";
        }

        public string Forecast(string location, int days)
        {
            return "butts";
        }
    }
}