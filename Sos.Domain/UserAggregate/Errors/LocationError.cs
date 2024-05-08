using Sos.Domain.Core.Commons.Bases;

namespace Sos.Domain.UserAggregate.Errors
{
    /// <summary>
    /// Contains the location errors.
    /// </summary>
    public class LocationError
    {
        public static Error LongitudeIsNotNumber =>
            new
            (
                "Location.LongitudeIsNotNumber",
                "Kinh độ không phải là số.",
                "The longitude is not a number."
            );

        public static Error LatitudeIsNotNumber =>
            new
            (
                "Location.LatitudeIsNotNumber",
                "Vĩ độ không phải là số.",
                "The latitude is not a number."
            );
    }
}
