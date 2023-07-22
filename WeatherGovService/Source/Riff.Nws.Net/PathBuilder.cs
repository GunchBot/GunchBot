using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riff.Nws.Net
{
    internal static class PathBuilder
    {
        private const char Separator = '/';
        private const string LatLonFormat = "0.####";

        private const string PointsPart = "points";
        private const string GridPointsPart = "gridpoints";
        private const string ForecastPart = "forecast";

        internal static string BuildPointsPath(double latitude, double longitude)
        {
            return BuildPath(PointsPart, $"{latitude.ToString(LatLonFormat)},{longitude.ToString(LatLonFormat)}");
        }

        internal static string BuildForecastPath(string gridId, int gridX, int gridY)
        {
            return BuildPath(GridPointsPart, gridId, $"{gridX},{gridY}", ForecastPart);
        }

        private static string BuildPath(params string[] parts)
        {
            return string.Join(Separator, parts);
        }
    }
}
