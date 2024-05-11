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

        public static Error RangeOfLongitudeIsInvalid =>
            new
            (
                "Location.RangeOfLongitudeIsNotAllowed",
                "Kinh độ phải trong đoạn [-180, 180].",
                "The range of longitude is invalid."
            );

        public static Error LatitudeIsNotNumber =>
            new
            (
                "Location.LatitudeIsNotNumber",
                "Vĩ độ không phải là số.",
                "The latitude is not a number."
            );

        public static Error RangeOfLatitudeIsInvalid =>
            new
            (
                "Location.RangeOfLatitudeIsInvalid",
                "Vĩ độ phải trong đoạn [-90, 90].",
                "The range of latitude is invalid."
            );
    }
}
