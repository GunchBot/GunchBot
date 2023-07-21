namespace GunchBot.Contracts
{
    /// <summary>
    /// Defines methods to serve as an interface between the user and a weather API.
    /// </summary>
    public interface IWeatherService
    {
        /// <summary>
        /// Gets the current weather for the specified location, using the specified unit.
        /// </summary>
        /// <param name="location">The location, as a string.</param>
        /// <param name="unit">The unit, as a char.</param>
        /// <returns>A formatted string with weather data.</returns>
        public string CurrentWeather(string location, char unit);
    }
}