using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObserverNetLite.API.Middlewares;
using ObserverNetLite.Service.Abstractions;

namespace ObserverNetLite.API.Endpoints;

public static class PermissionEndpoints
{
    public static void MapPermissionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/permissions");

        group.MapGet("/", async ([FromServices] IPermissionService permissionService) =>
        {
            try
            {
                var permissions = await permissionService.GetAllPermissionsAsync();
                return Results.Ok(permissions);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin"))
        .WithName("GetAllPermissions")
        .WithOpenApi();

        group.MapGet("/{id:guid}", async (
            [FromRoute] Guid id,
            [FromServices] IPermissionService permissionService) =>
        {
            try
            {
                var permission = await permissionService.GetPermissionByIdAsync(id);
                if (permission == null)
                    return Results.NotFound(new { message = "Permission bulunamadı." });

                return Results.Ok(permission);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin"))
        .WithName("GetPermissionById")
        .WithOpenApi();

        group.MapGet("/category/{category}", async (
            [FromRoute] string category,
            [FromServices] IPermissionService permissionService) =>
        {
            try
            {
                var permissions = await permissionService.GetPermissionsByCategoryAsync(category);
                return Results.Ok(permissions);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin"))
        .WithName("GetPermissionsByCategory")
        .WithOpenApi();
    }
}
