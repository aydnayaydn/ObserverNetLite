using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObserverNetLite.API.Middlewares;
using ObserverNetLite.Application.Abstractions;
using ObserverNetLite.Application.DTOs;

namespace ObserverNetLite.API.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/auth");
            
            group.MapPost("/login", async (
                [FromBody] LoginDto loginDto,
                [FromServices] IUserService userService,
                HttpContext httpContext) =>
            {
                try
                {
                    var token = await userService.AuthenticateAsync(loginDto);
                    return Results.Ok(token);
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Results.Unauthorized();
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
            .WithName("Login")
            .WithOpenApi();

            group.MapGet("/test-admin", () => Results.Ok("Hello Admin!"))
                .WithMetadata(new RequireRoleAttribute("admin"))
                .WithName("TestAdmin")
                .WithOpenApi();

            group.MapGet("/test-user", () => Results.Ok("Hello User!"))
                .WithMetadata(new RequireRoleAttribute("user", "admin"))
                .WithName("TestUser")
                .WithOpenApi();
        }
    }
}
