using Sos.Domain.Core.Commons.Bases;

namespace Sos.Application.Modules.Users.Errors
{
    /// <summary>
    /// Contains the update location errors.
    /// </summary>
    public static class UpdateLocationError
    {
        public static Error LongitudeIsRequired =>
            new
            (
                "UpdateLocationError.LongitudeIsRequired",
                "Kinh độ bắt buộc phải có.",
                "The longitude is required."
            );

        public static Error LatitudeIsRequired =>
            new
            (
                "UpdateLocationError.LatitudeIsRequired",
                "Vĩ độ bắt buộc phải có.",
                "The latitude is required."
            );
    }
}
