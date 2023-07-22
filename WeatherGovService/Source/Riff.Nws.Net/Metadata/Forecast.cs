using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Riff.Nws.Net.Metadata
{
    public class Dewpoint
    {
        [JsonProperty("unitCode")]
        [JsonPropertyName("unitCode")]
        public string UnitCode { get; set; }

        [JsonProperty("value")]
        [JsonPropertyName("value")]
        public double Value { get; set; }
    }

    public class Elevation
    {
        [JsonProperty("unitCode")]
        [JsonPropertyName("unitCode")]
        public string UnitCode { get; set; }

        [JsonProperty("value")]
        [JsonPropertyName("value")]
        public double Value { get; set; }
    }

    public class ForecastGeometry
    {
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        [JsonPropertyName("coordinates")]
        public List<List<List<double>>> Coordinates { get; set; }
    }

    public class Period
    {
        [JsonProperty("number")]
        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("startTime")]
        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty("endTime")]
        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; set; }

        [JsonProperty("isDaytime")]
        [JsonPropertyName("isDaytime")]
        public bool IsDaytime { get; set; }

        [JsonProperty("temperature")]
        [JsonPropertyName("temperature")]
        public int Temperature { get; set; }

        [JsonProperty("temperatureUnit")]
        [JsonPropertyName("temperatureUnit")]
        public string TemperatureUnit { get; set; }

        [JsonProperty("temperatureTrend")]
        [JsonPropertyName("temperatureTrend")]
        public object TemperatureTrend { get; set; }

        [JsonProperty("probabilityOfPrecipitation")]
        [JsonPropertyName("probabilityOfPrecipitation")]
        public ProbabilityOfPrecipitation ProbabilityOfPrecipitation { get; set; }

        [JsonProperty("dewpoint")]
        [JsonPropertyName("dewpoint")]
        public Dewpoint Dewpoint { get; set; }

        [JsonProperty("relativeHumidity")]
        [JsonPropertyName("relativeHumidity")]
        public RelativeHumidity RelativeHumidity { get; set; }

        [JsonProperty("windSpeed")]
        [JsonPropertyName("windSpeed")]
        public string WindSpeed { get; set; }

        [JsonProperty("windDirection")]
        [JsonPropertyName("windDirection")]
        public string WindDirection { get; set; }

        [JsonProperty("icon")]
        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonProperty("shortForecast")]
        [JsonPropertyName("shortForecast")]
        public string ShortForecast { get; set; }

        [JsonProperty("detailedForecast")]
        [JsonPropertyName("detailedForecast")]
        public string DetailedForecast { get; set; }
    }

    public class ProbabilityOfPrecipitation
    {
        [JsonProperty("unitCode")]
        [JsonPropertyName("unitCode")]
        public string UnitCode { get; set; }

        [JsonProperty("value")]
        [JsonPropertyName("value")]
        public int? Value { get; set; }
    }

    public class ForecastProperties
    {
        [JsonProperty("updated")]
        [JsonPropertyName("updated")]
        public DateTime Updated { get; set; }

        [JsonProperty("units")]
        [JsonPropertyName("units")]
        public string Units { get; set; }

        [JsonProperty("forecastGenerator")]
        [JsonPropertyName("forecastGenerator")]
        public string ForecastGenerator { get; set; }

        [JsonProperty("generatedAt")]
        [JsonPropertyName("generatedAt")]
        public DateTime GeneratedAt { get; set; }

        [JsonProperty("updateTime")]
        [JsonPropertyName("updateTime")]
        public DateTime UpdateTime { get; set; }

        [JsonProperty("validTimes")]
        [JsonPropertyName("validTimes")]
        public string ValidTimes { get; set; }

        [JsonProperty("elevation")]
        [JsonPropertyName("elevation")]
        public Elevation Elevation { get; set; }

        [JsonProperty("periods")]
        [JsonPropertyName("periods")]
        public List<Period> Periods { get; set; }
    }

    public class RelativeHumidity
    {
        [JsonProperty("unitCode")]
        [JsonPropertyName("unitCode")]
        public string UnitCode { get; set; }

        [JsonProperty("value")]
        [JsonPropertyName("value")]
        public int Value { get; set; }
    }

    public class Forecast
    {
        [JsonProperty("@context")]
        [JsonPropertyName("@context")]
        public List<object> Context { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("geometry")]
        [JsonPropertyName("geometry")]
        public ForecastGeometry Geometry { get; set; }

        [JsonProperty("properties")]
        [JsonPropertyName("properties")]
        public ForecastProperties Properties { get; set; }
    }
}