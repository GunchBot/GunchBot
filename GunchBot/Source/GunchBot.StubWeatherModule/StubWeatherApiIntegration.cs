namespace GunchBot.StubWeatherModule
{
    using GunchBot.Contracts;

    public class StubWeatherApiIntegration : IWeatherApiIntegration
    {
        private Random random = new Random();

        public string CurrentWeather(string location, char unit)
        {
            return $"It is currently {random.Next() % 120}°{unit} in {location}.\n(this is fake)";
        }
    }
}