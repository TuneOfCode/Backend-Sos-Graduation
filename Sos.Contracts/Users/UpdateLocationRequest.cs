namespace Sos.Contracts.Users
{
    /// <summary>
    /// Represents the update location request.
    /// </summary>
    public record UpdateLocationRequest(
        double Longitude,
        double Latitude
    );
}
