namespace GunchBot.Contracts
{
    /// <summary>
    /// Latitude and longitude coordinates.
    /// </summary>
    public struct Coordinates
    {
        /// <summary>
        /// An invalid coordinate.
        /// </summary>
        public static Coordinates Invalid = new Coordinates
        {
            Latitude = Double.NaN,
            Longitude = Double.NaN
        };

        /// <summary>
        /// Latitude in degrees.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude in degrees.
        /// </summary>
        public double Longitude { get; set; }
    }
}
