using Sos.Application.Core.Abstractions.Cryptography;

namespace Sos.Infrastructure.Crytography
{
    /// <summary>
    /// Represents the verify code generator.
    /// </summary>
    public class VerifyCodeGenerator : IVerifyCodeGenerator
    {
        // < inheritdoc />
        public string Generate() => new Random().Next(100000, 999999).ToString();
    }
}
