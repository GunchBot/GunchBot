using Riff.Nws.Net.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GunchBot.WeatherService.Nws
{
    public static class StringFormatter
    {
        internal static string GetFormattedForecast(this Forecast forecast, int days)
        {
            var output = string.Empty;

            var periods = forecast.Properties.Periods.ToList();

            var firstEntry = periods.FirstOrDefault(); // should never be null. If it is, the exception gets caught elsewhere.
            if (!firstEntry.IsDaytime)
            {
                output +=
                    $"{firstEntry.Name}\n\t⬇️ Lows around {firstEntry.Temperature}°{firstEntry.TemperatureUnit}." +
                    $"\n\tTonight:\n\t\t{GetFormattedShortForecast(firstEntry)}" +
                    $"\n\n";

                periods.Remove(firstEntry);
                days--;
            }

            for (int i = 0; i < days && i * 2 + 1 < forecast.Properties.Periods.Count; i++) // this logic is a mess
            {
                var day = forecast.Properties.Periods[i * 2];
                var evening = forecast.Properties.Periods[i * 2 + 1];
                var date = day.StartTime.ToString("dd MMMM");
                output +=
                    $"{day.Name} ({date}):" +
                    $"\n\t⬆️ High: {day.Temperature}°{day.TemperatureUnit}, ⬇️ Low: {evening.Temperature}°{evening.TemperatureUnit}." +
                    $"\n\tDuring the day:\n\t\t{GetFormattedShortForecast(day)}" +
                    $"\n\tAt night:\n\t\t{GetFormattedShortForecast(evening)}\n\n";
            }

            return output;
        }

        private static string GetFormattedShortForecast(Period period)
        {
            return $"{GetEmojiFromForecast(period.ShortForecast, period.IsDaytime)} {period.ShortForecast}.{(period.ProbabilityOfPrecipitation.Value.HasValue ? $" ({period.ProbabilityOfPrecipitation.Value}%)." : "")}" +
                $" Winds {period.WindDirection} at {period.WindSpeed}.";
        }

        private static string GetEmojiFromForecast(string shortForecast, bool daytime)
        {
            //TODO: add logic to split and provide multiple emojis when weather lists changes
            //e.g. "Chance Showers And Thunderstorms then Mostly Sunny"
            if (shortForecast.ToLower().Contains("thunderstorms"))
            {
                return "⛈️";
            }
            if (shortForecast.ToLower().Contains("rain") || shortForecast.Contains("showers"))
            {
                return "🌧️";
            }
            if (shortForecast.ToLower().Contains("cloudy"))
            {
                if (shortForecast.ToLower().Contains("partly"))
                {
                    return daytime ? "⛅" : "☁️";
                }
                return "☁️";
            }
            return daytime ? "🌞" : "🌝"; //todo: gimme them moonphases
        }
    }
}
