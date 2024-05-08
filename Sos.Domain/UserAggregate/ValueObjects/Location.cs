using Sos.Domain.Core.Commons.Bases;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.UserAggregate.Errors;

namespace Sos.Domain.UserAggregate.ValueObjects
{
    /// <summary>
    /// Represents the location value object.
    /// </summary>
    public sealed class Location : ValueObject
    {
        private const double _defaultLongitudeOfHue = 16.462622;
        private const double _defaultLatitudeOfHue = 107.585217;

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <param name="latitude">The latitude.</param>
        private Location(
            double longitude,
            double latitude
        )
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        /// <summary>
        /// Gets the longitude.
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        /// Gets the latitude.
        /// </summary>
        public double Latitude { get; }

        public static implicit operator string(Location location) => $"{location.Longitude};{location.Latitude}";

        /// <summary>
        /// Creates a new <see cref="Location"/> instance based on the specified value.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <param name="latitude">The latitude.</param>
        /// <returns>The result of the location creattion process containing the location or an error.</returns>
        public static Result<Location> Create(double longitude = _defaultLongitudeOfHue, double latitude = _defaultLatitudeOfHue)
            => Result.Create($"{longitude};{latitude}", Error.None)
                    .Ensure(l => !double.IsNaN(longitude), LocationError.LongitudeIsNotNumber)
                    .Ensure(l => !double.IsNaN(latitude), LocationError.LatitudeIsNotNumber)
                    .Map(l => new Location(longitude, latitude));

        // < inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return $"{Longitude};{Latitude}";
        }
    }
}
