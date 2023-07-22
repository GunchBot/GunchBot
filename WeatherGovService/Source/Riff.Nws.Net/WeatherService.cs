using System.Net.Http.Headers;
using System.Text.Json;
using Riff.Nws.Net.Metadata;

namespace Riff.Nws.Net
{
    /// <summary>
    /// Provides methods to allow for integration with the NWS Weather.Gov api.
    /// TODO: Once it's more fleshed out, move this project/solution to its own repo and create a nuget package of it.
    /// </summary>
    public class WeatherService
    {
        private const string Url = "https://api.weather.gov/";
        private HttpClient client;

        /// <summary>
        /// Creates an instance of <see cref="WeatherService"/>.
        /// </summary>
        /// <param name="productName">The name of the product to use for the UserAgent for the <see cref="HttpClient"/>.</param>
        public WeatherService(string productName)
        {
            this.client = new HttpClient();
            this.client.Timeout = TimeSpan.FromMinutes(1);
            this.client.BaseAddress = new Uri(Url);
            this.client.DefaultRequestHeaders.Clear();
            this.client.DefaultRequestHeaders.UserAgent.ParseAdd(productName);
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        internal async Task<string> GetAsync(string path)
        {
            var response = await client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Gets the gridpoint for the specified latitude and longitude.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>A <see cref="GridPoint"/>.</returns>
        /// <exception cref="JsonException">Thrown if the API returns missing or malformed data data. Should never happen.</exception>
        public async Task<GridPoint> GetGridpointsAsync(double latitude, double longitude)
        {
            return JsonSerializer.Deserialize<GridPoint>(await this.GetAsync(PathBuilder.BuildPointsPath(latitude, longitude))) ?? throw new JsonException("Invalid json was returned.");
        }

        /// <summary>
        /// Gets the forecast.
        /// </summary>
        /// <param name="gridPoint">The <see cref="GridPoint"/> for the desired location.</param>
        /// <returns>A <see cref="Forecast"/>.</returns>
        /// <exception cref="JsonException">Thrown if the API returns missing or malformed data. Should never happen.</exception>
        public async Task<Forecast> GetForecastAsync(GridPoint gridPoint)
        {
            return JsonSerializer.Deserialize<Forecast>(await this.GetAsync(PathBuilder.BuildForecastPath(gridPoint.Properties.GridId, gridPoint.Properties.GridX, gridPoint.Properties.GridY))) ?? throw new JsonException("Invalid json was returned.");
        }

    }
}
