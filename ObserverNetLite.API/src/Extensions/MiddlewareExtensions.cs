using ObserverNetLite.API.Middlewares;

namespace ObserverNetLite.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRoleBasedAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RoleBasedAuthorizationMiddleware>();
        }
    }
}
