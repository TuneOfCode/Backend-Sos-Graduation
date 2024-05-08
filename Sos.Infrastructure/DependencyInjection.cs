using EventReminder.Infrastructure.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Cache;
using Sos.Application.Core.Abstractions.Common;
using Sos.Application.Core.Abstractions.Cryptography;
using Sos.Application.Core.Abstractions.Email;
using Sos.Application.Core.Abstractions.MessageQueue;
using Sos.Domain.UserAggregate.Services;
using Sos.Infrastructure.Authentication;
using Sos.Infrastructure.Authentication.Settings;
using Sos.Infrastructure.Cache;
using Sos.Infrastructure.Cache.Settings;
using Sos.Infrastructure.Common;
using Sos.Infrastructure.Crytography;
using Sos.Infrastructure.Email;
using Sos.Infrastructure.Email.Settings;
using Sos.Infrastructure.MessageQueue;
using Sos.Infrastructure.MessageQueue.Settings;
using Sos.Infrastructure.Socket;
using StackExchange.Redis;
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
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var path = context.Request.Path;
                        var accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Request.Headers.Append("Authorization", new[] { $"Bearer {accessToken}" });
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SettingsKey));

            services.Configure<MailSettings>(configuration.GetSection(MailSettings.SettingsKey));

            services.Configure<CacheSettings>(configuration.GetSection(CacheSettings.SettingsKey));

            services.Configure<MessageQueueSettings>(configuration.GetSection(MessageQueueSettings.SettingsKey));

            services.AddScoped<IJwtProvider, JwtProvider>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUserIdentifierProvider, UserIdentifierProvider>();

            services.AddTransient<IUserIdProvider, UserIdentifierSocketProvider>();

            services.AddTransient<IDateTime, MachineDateTime>();

            services.AddTransient<IPasswordHashCheckerService, PasswordHasher>();

            services.AddTransient<IPasswordHasher, PasswordHasher>();

            services.AddTransient<IVerifyCodeGenerator, VerifyCodeGenerator>();

            services.AddTransient<IEmailService, EmailService>();

            services.AddSignalR();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetSection(CacheSettings.SettingsKey).Value!;
            });

            services.AddSingleton<IConnectionMultiplexer>(
                _ => ConnectionMultiplexer.Connect(configuration.GetSection(CacheSettings.SettingsKey).Value!
            ));

            services.AddScoped<ICacheService, CacheService>();

            services.AddScoped<IProducer, KafkaProducer>(provider => new KafkaProducer(
                provider.GetRequiredService<ILogger<KafkaProducer>>(),
                configuration.GetSection(MessageQueueSettings.SettingsKey).Value!
            ));

            services.AddSingleton<IConsumer, KafkaConsumer>(provider => new KafkaConsumer(
                provider.GetRequiredService<ILogger<KafkaConsumer>>(),
                configuration.GetSection(MessageQueueSettings.SettingsKey).Value!
            ));

            services.AddHostedService<BackgroundKafkaService>();

            return services;
        }
    }
}
