namespace GunchBot.Contracts
{
    public interface IWeatherApiIntegration
    {
        public string CurrentWeather(string location, char unit);
    }
}