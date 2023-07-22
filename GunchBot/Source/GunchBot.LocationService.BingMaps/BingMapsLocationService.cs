using BingMapsRESTToolkit;
using GunchBot.Contracts;

namespace GunchBot.LocationService.BingMaps
{
    /// <summary>
    /// Uses BingMaps for location lookups.
    /// </summary>
    public class BingMapsLocationService : ILocationService
    {
        private readonly string? apiKey;

        /// <summary>
        /// Creates a new instance of <see cref="BingMapsLocationService"/>
        /// </summary>
        public BingMapsLocationService(string apiKey)
        {
            this.apiKey = apiKey;
        }

        /// <inheritdoc/>
        public Coordinates Geocode(string location)
        {
            if(apiKey != null)
            {
                var geocodeRequest = new GeocodeRequest
                {
                    BingMapsKey = apiKey,
                    Query = location,
                };

                if (GetResourcesFromRequest(geocodeRequest).FirstOrDefault() is Location locationResource)
                {
                    return new Coordinates
                    {
                        Latitude = locationResource.Point.Coordinates[0],
                        Longitude = locationResource.Point.Coordinates[1],
                    };
                }
            }
            
            return Coordinates.Invalid;
        }


        private Resource[] GetResourcesFromRequest(BaseRestRequest rest_request)
        {
            var response = ServiceManager.GetResponseAsync(rest_request).GetAwaiter().GetResult();

            if (response != null && response.ResourceSets != null &&
                response.ResourceSets.Length > 0 &&
                response.ResourceSets[0].Resources != null &&
                response.ResourceSets[0].Resources.Length > 0)
            {
                return response.ResourceSets[0].Resources;
            }

            return Array.Empty<Resource>();
        }
    }
}