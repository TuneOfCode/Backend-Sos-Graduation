using Sos.Application.Core.Abstractions.Common;

namespace Sos.Infrastructure.Common
{
    /// <summary>
    /// Represents the machine date time service.
    /// </summary>
    internal sealed class MachineDateTime : IDateTime
    {
        /// <inheritdoc />
        public DateTime UtcNow => DateTime.UtcNow;

        // <inheritdoc />
        public DateTime LocalNow => DateTime.Now;
    }
}
