using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sos.Application.Core.Abstractions.Data;
using Sos.Domain.FriendshipAggregate.Repositories;
using Sos.Domain.FriendshipAggregate.Services;
using Sos.Domain.UserAggregate.Repositories;
using Sos.Persistence.Data;
using Sos.Persistence.Modules.Friendships;
using Sos.Persistence.Modules.Users;

namespace Sos.Persistence
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the necessary services with Dependency Inversion.
        /// </summary>
        /// <param name="services">The servive collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddPersistence(
            this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(ConnectionString.SettingsKey);

            services.AddSingleton(new ConnectionString(connectionString!));

            services.AddDbContext<AppDbContext>(options
                => options.UseSqlServer(connectionString));

            services.AddScoped<IDbContext>(serviceProvider
                => serviceProvider.GetRequiredService<AppDbContext>());

            services.AddScoped<IUnitOfWork>(serviceProvider
                => serviceProvider.GetRequiredService<AppDbContext>());

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IFriendshipRequestRepository, FriendshipRequestRepository>();

            services.AddScoped<IFriendshipRepository, FriendshipRepository>();

            services.AddScoped<IFriendshipService, FriendshipService>();

            return services;
        }
    }
}
