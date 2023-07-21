namespace GunchBot.Contracts
{
    /// <summary>
    /// Defines methods used to take user-readable/typable location data and convert it into computer readable data.
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        /// Returns the latitude and longitude for a specified location.
        /// </summary>
        /// <param name="location">The name of the location.This can be formatted in most user-readable names. e.g. "35806", or "Huntsville, Alabama", or even "University of Alabama Huntsville".</param>
        /// <returns>The latitude and longitude in degrees.</returns>
        (double latitude, double longitude) Geocode(string location);
    }
}
