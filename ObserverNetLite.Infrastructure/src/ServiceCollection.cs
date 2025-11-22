using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ObserverNetLite.Core.Abstractions;
using ObserverNetLite.Data;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Infrastructure;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds ObserverNetLite infrastructure services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        // Add DbContext
        services.AddDbContext<ObserverNetLiteDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ObserverNetLiteDbContext).Assembly.FullName))
        );

        // Register repositories
        services.AddScoped<DbContext, ObserverNetLiteDbContext>();
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

        return services;
    }
}