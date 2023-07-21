using GunchBot.Contracts;

namespace GunchBot.Utilities
{
    /// <summary>
    /// Extension methods for the <see cref="Coordinates"/> struct.
    /// </summary>
    public static class CoordinatesExtensions
    {
        /// <summary>
        /// Checks if coordinate is invalid.
        /// </summary>
        /// <param name="coordinates">The coordinates to check.</param>
        /// <returns>True if invalid, false otherwise.</returns>
        public static bool IsInvalid(this Coordinates coordinates)
        {
            return coordinates.Latitude == null || coordinates.Longitude == null;
        }
    }
}
