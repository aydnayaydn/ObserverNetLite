using Microsoft.Extensions.DependencyInjection;
using ObserverNetLite.Application.Abstractions;
using ObserverNetLite.Application.Services;

namespace ObserverNetLite.Application;

/// <summary>
/// Extension methods for setting up ObserverNetLite application services in an IServiceCollection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds ObserverNetLite application services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddScoped<IUserService, UserService>();

        return services;
    }
}