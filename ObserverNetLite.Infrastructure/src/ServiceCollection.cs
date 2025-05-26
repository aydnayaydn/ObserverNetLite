using Microsoft.Extensions.DependencyInjection;
using ObserverNetLite.Core.Abstractions;
using ObserverNetLite.Entities;

namespace ObserverNetLite.Infrastructure;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds ObserverNetLite infrastructure services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddScoped<IRepository<User>, GenericRepository<User>>();

        return services;
    }
}