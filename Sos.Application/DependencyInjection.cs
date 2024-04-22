using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Sos.Application.Core.Behaviours;
using System.Reflection;

namespace Sos.Application
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the necessary services with Dependency Inversion.
        /// </summary>
        /// <param name="services">The servive collection.</param>
        /// <returns></returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);

                options.AddOpenBehavior(typeof(ValidationBehaviour<,>));

                options.AddOpenBehavior(typeof(TransactionBehaviour<,>));

                services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}
