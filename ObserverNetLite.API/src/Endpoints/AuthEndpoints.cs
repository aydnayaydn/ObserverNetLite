using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObserverNetLite.API.Middlewares;
using ObserverNetLite.Service.Abstractions;
using ObserverNetLite.Service.DTOs;
using System;

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
                [FromServices] IAuthService authService,
                HttpContext httpContext) =>
            {
                try
                {
                    // Validate user credentials
                    var isValid = await userService.ValidateUserAsync(loginDto.UserName, loginDto.Password);
                    if (!isValid)
                    {
                        return Results.Unauthorized();
                    }

                    // Get user to retrieve role
                    var user = await userService.GetUserByUserNameAsync(loginDto.UserName);
                    if (user == null)
                    {
                        return Results.Unauthorized();
                    }

                    // Generate token
                    var token = await authService.GenerateTokenAsync(user.UserName, user.Role);
                    return Results.Ok(token);
                }
                catch (UnauthorizedAccessException)
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
