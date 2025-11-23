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

            group.MapPost("/reset-password", async (
                [FromBody] ResetPasswordDto resetPasswordDto,
                [FromServices] IUserService userService) =>
            {
                try
                {
                    var result = await userService.ResetPasswordAsync(resetPasswordDto);
                    if (!result)
                    {
                        return Results.BadRequest(new { message = "Kullanıcı adı veya eski şifre hatalı." });
                    }

                    return Results.Ok(new { message = "Şifre başarıyla değiştirildi." });
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
            .WithName("ResetPassword")
            .WithOpenApi();

            group.MapPost("/forgot-password", async (
                [FromBody] ForgotPasswordDto forgotPasswordDto,
                [FromServices] IUserService userService) =>
            {
                try
                {
                    var result = await userService.ForgotPasswordAsync(forgotPasswordDto);
                    if (!result)
                    {
                        return Results.BadRequest(new { message = "Email adresi bulunamadı." });
                    }

                    return Results.Ok(new { message = "Şifre sıfırlama bağlantısı email adresinize gönderildi." });
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
            .WithName("ForgotPassword")
            .WithOpenApi();

            group.MapPost("/reset-password-with-token", async (
                [FromBody] ResetPasswordWithTokenDto resetDto,
                [FromServices] IUserService userService) =>
            {
                try
                {
                    var result = await userService.ResetPasswordWithTokenAsync(resetDto);
                    if (!result)
                    {
                        return Results.BadRequest(new { message = "Geçersiz veya süresi dolmuş token." });
                    }

                    return Results.Ok(new { message = "Şifreniz başarıyla sıfırlandı." });
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
            .WithName("ResetPasswordWithToken")
            .WithOpenApi();
        }
    }
}
