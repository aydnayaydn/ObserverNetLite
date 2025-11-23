using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ObserverNetLite.Service.Abstractions;
using ObserverNetLite.Service.Services;
using ObserverNetLite.Service.Settings;
using ObserverNetLite.Core.Helpers;

namespace ObserverNetLite.Service;

/// <summary>
/// Extension methods for setting up ObserverNetLite application services in an IServiceCollection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds ObserverNetLite application services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="configuration">The IConfiguration to read settings from.</param>
    /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        // Read mail password from mailpassword.txt
        var mailPasswordPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "mailpassword.txt");
        var mailPassword = File.Exists(mailPasswordPath) 
            ? File.ReadAllText(mailPasswordPath).Trim() 
            : string.Empty;

        // Configure EmailSettings from appsettings.json
        var emailSection = configuration.GetSection("EmailSettings");
        var emailSettings = new EmailSettings
        {
            Host = emailSection["Host"] ?? string.Empty,
            Port = int.Parse(emailSection["Port"] ?? "587"),
            Username = emailSection["Username"] ?? string.Empty,
            Password = mailPassword,
            FromEmail = emailSection["FromEmail"] ?? string.Empty,
            FromName = emailSection["FromName"] ?? string.Empty,
            EnableSsl = bool.Parse(emailSection["EnableSsl"] ?? "true")
        };

        // Configure PasswordResetSettings from appsettings.json
        var resetSection = configuration.GetSection("PasswordResetSettings");
        var resetSettings = new PasswordResetSettings
        {
            ResetUrl = resetSection["ResetUrl"] ?? string.Empty
        };

        // Register EmailHelper as singleton with settings
        services.AddSingleton(sp => new EmailHelper(
            emailSettings.Host,
            emailSettings.Port,
            emailSettings.Username,
            emailSettings.Password,
            emailSettings.FromEmail,
            emailSettings.FromName,
            emailSettings.EnableSsl
        ));

        // Register PasswordResetSettings as singleton
        services.AddSingleton(resetSettings);

        // Add AutoMapper
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        // Add services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IMenuService, MenuService>();

        return services;
    }
}
