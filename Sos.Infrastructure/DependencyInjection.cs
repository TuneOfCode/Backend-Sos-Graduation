using EventReminder.Infrastructure.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Common;
using Sos.Application.Core.Abstractions.Cryptography;
using Sos.Domain.UserAggregate.Services;
using Sos.Infrastructure.Authentication;
using Sos.Infrastructure.Authentication.Settings;
using Sos.Infrastructure.Common;
using System.Text;

namespace Sos.Infrastructure
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the infrastructure services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"]!)),
                };
            });

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SettingsKey));

            services.AddScoped<IJwtProvider, JwtProvider>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUserIdentifierProvider, UserIdentifierProvider>();

            services.AddTransient<IDateTime, MachineDateTime>();

            services.AddTransient<IPasswordHashCheckerService, PasswordHasher>();

            services.AddTransient<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
