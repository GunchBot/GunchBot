namespace Riff.Nws.Net.Cli
{
    internal class Program
    {
        static void Main(string[] args) => RunAsync().GetAwaiter().GetResult();

        static async Task RunAsync()
        {
            var weatherService = new WeatherService("troutmansm");
            var output = await weatherService.GetForecastAsync(34.80754021393779, -86.84609676679759);
            var test = 0;
        }
    }
}