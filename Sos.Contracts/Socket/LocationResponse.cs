namespace Sos.Contracts.Socket
{
    /// <summary>
    /// Represents the location of victim response.
    /// </summary>
    public class LocationResponse
    {
        /// <summary>
        /// Gets or sets the victim id.
        /// </summary>
        public Guid VictimId { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public double Latitude { get; set; }
    }
}
