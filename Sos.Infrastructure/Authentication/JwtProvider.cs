﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Common;
using Sos.Domain.UserAggregate;
using Sos.Infrastructure.Authentication.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sos.Infrastructure.Authentication
{
    /// <summary>
    /// Represents the JWT provider.
    /// </summary>
    internal sealed class JwtProvider : IJwtProvider
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtProvider"/> class.
        /// </summary>
        /// <param name="jwtOptions">The JWT options.</param>
        /// <param name="dateTime">The current date and time.</param>
        public JwtProvider(
            IOptions<JwtSettings> jwtOptions,
            IDateTime dateTime
        )
        {
            _jwtSettings = jwtOptions.Value;
            _dateTime = dateTime;
        }

        /// <inheritdoc />
        public string GenerateAccessToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims =
            {
                new (ClaimsType.UserId, user.Id.ToString()),
                new (ClaimsType.EmailUnique, user.Email!.Value),
                new (ClaimsType.Role, user.Role),
                new (ClaimsType.RoleApp, user.Role),
            };

            DateTime tokenExpirationTime = _dateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes);

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                null,
                tokenExpirationTime,
                signingCredentials
            );

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        // <inheritdoc />
        public string GenerateRefreshToken()
        {
            var refreshToken = Guid.NewGuid().ToString()[..10];

            return refreshToken;
        }
    }
}
