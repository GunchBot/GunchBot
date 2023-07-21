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
        public (double latitude, double longitude) Geocode(string location)
        {
            var latitude = 0.0;
            var longitude = 0.0;

            if(apiKey != null)
            {
                var geocodeRequest = new GeocodeRequest
                {
                    BingMapsKey = apiKey,
                    Query = location,
                };

                var locationResource = GetResourcesFromRequest(geocodeRequest).FirstOrDefault() as Location;
                if(locationResource != null)
                {
                    latitude = locationResource.Point.Coordinates[0];
                    longitude = locationResource.Point.Coordinates[1];
                }
            }

            //TODO since technically 0,0 is a valid and real location,
            //we need to make a coordinate class and have some coordinate.invalid value
            //for the case where the lookup fails.
            return (latitude, longitude);
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