namespace Sos.Application.Core.Abstractions.Cryptography
{
    /// <summary>
    /// Represents the verify code interface.
    /// </summary>
    public interface IVerifyCodeGenerator
    {
        /// <summary>
        /// Generates the verify code generator.
        /// </summary>
        /// <returns>The verify code.</returns>
        string Generate();
    }
}
