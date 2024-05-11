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
        private const double _defaultLatitudeOfHue = 16.462622;
        private const double _defaultLongitudeOfHue = 107.585217;
        private const double _minOfLatitude = -90;
        private const double _maxOfLatitude = 90;
        private const double _minOfLongitude = -180;
        private const double _maxOfLongitude = 180;

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

        public static implicit operator string(Location location) => $"{location.Latitude};{location.Longitude}";

        /// <summary>
        /// Creates a new <see cref="Location"/> instance based on the specified value.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <param name="latitude">The latitude.</param>
        /// <returns>The result of the location creattion process containing the location or an error.</returns>
        public static Result<Location> Create(double longitude = _defaultLongitudeOfHue, double latitude = _defaultLatitudeOfHue)
            => Result.Create($"{latitude};{longitude}", Error.None)
                    .Ensure(l => !double.IsNaN(longitude), LocationError.LongitudeIsNotNumber)
                    .Ensure(l => longitude >= _minOfLongitude && longitude <= _maxOfLongitude, LocationError.RangeOfLongitudeIsInvalid)
                    .Ensure(l => !double.IsNaN(latitude), LocationError.LatitudeIsNotNumber)
                    .Ensure(l => latitude >= _minOfLatitude && latitude <= _maxOfLatitude, LocationError.RangeOfLatitudeIsInvalid)
                    .Map(l => new Location(longitude, latitude));

        // < inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return $"{Latitude};{Longitude}";
        }
    }
}
