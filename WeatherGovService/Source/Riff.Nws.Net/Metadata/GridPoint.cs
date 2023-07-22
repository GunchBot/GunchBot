using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Riff.Nws.Net.Metadata
{
    public class Bearing
    {
        [JsonProperty("unitCode")]
        [JsonPropertyName("unitCode")]
        public string UnitCode { get; set; }

        [JsonProperty("value")]
        [JsonPropertyName("value")]
        public int Value { get; set; }
    }

    public class Distance
    {
        [JsonProperty("unitCode")]
        [JsonPropertyName("unitCode")]
        public string UnitCode { get; set; }

        [JsonProperty("value")]
        [JsonPropertyName("value")]
        public double Value { get; set; }
    }

    public class GridPointGeometry
    {
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        [JsonPropertyName("coordinates")]
        public List<double> Coordinates { get; set; }
    }

    public class GridPointProperties
    {
        [JsonProperty("@id")]
        [JsonPropertyName("@id")]
        public string Id { get; set; }

        [JsonProperty("@type")]
        [JsonPropertyName("@type")]
        public string Type { get; set; }

        [JsonProperty("cwa")]
        [JsonPropertyName("cwa")]
        public string Cwa { get; set; }

        [JsonProperty("forecastOffice")]
        [JsonPropertyName("forecastOffice")]
        public string ForecastOffice { get; set; }

        [JsonProperty("gridId")]
        [JsonPropertyName("gridId")]
        public string GridId { get; set; }

        [JsonProperty("gridX")]
        [JsonPropertyName("gridX")]
        public int GridX { get; set; }

        [JsonProperty("gridY")]
        [JsonPropertyName("gridY")]
        public int GridY { get; set; }

        [JsonProperty("forecast")]
        [JsonPropertyName("forecast")]
        public string Forecast { get; set; }

        [JsonProperty("forecastHourly")]
        [JsonPropertyName("forecastHourly")]
        public string ForecastHourly { get; set; }

        [JsonProperty("forecastGridData")]
        [JsonPropertyName("forecastGridData")]
        public string ForecastGridData { get; set; }

        [JsonProperty("observationStations")]
        [JsonPropertyName("observationStations")]
        public string ObservationStations { get; set; }

        [JsonProperty("relativeLocation")]
        [JsonPropertyName("relativeLocation")]
        public RelativeLocation RelativeLocation { get; set; }

        [JsonProperty("forecastZone")]
        [JsonPropertyName("forecastZone")]
        public string ForecastZone { get; set; }

        [JsonProperty("county")]
        [JsonPropertyName("county")]
        public string County { get; set; }

        [JsonProperty("fireWeatherZone")]
        [JsonPropertyName("fireWeatherZone")]
        public string FireWeatherZone { get; set; }

        [JsonProperty("timeZone")]
        [JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("radarStation")]
        [JsonPropertyName("radarStation")]
        public string RadarStation { get; set; }

        [JsonProperty("city")]
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonProperty("distance")]
        [JsonPropertyName("distance")]
        public Distance Distance { get; set; }

        [JsonProperty("bearing")]
        [JsonPropertyName("bearing")]
        public Bearing Bearing { get; set; }
    }

    public class RelativeLocation
    {
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("geometry")]
        [JsonPropertyName("geometry")]
        public GridPointGeometry Geometry { get; set; }

        [JsonProperty("properties")]
        [JsonPropertyName("properties")]
        public RelativePointProperties Properties { get; set; }
    }

    public class RelativePointProperties
    {
        [JsonProperty("city")]
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        [JsonPropertyName("state")]
        public string State { get; set; }
    }

    public class GridPoint
    {
        [JsonProperty("@context")]
        [JsonPropertyName("@context")]
        public List<object> Context { get; set; }

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("geometry")]
        [JsonPropertyName("geometry")]
        public GridPointGeometry Geometry { get; set; }

        [JsonProperty("properties")]
        [JsonPropertyName("properties")]
        public GridPointProperties Properties { get; set; }
    }
}