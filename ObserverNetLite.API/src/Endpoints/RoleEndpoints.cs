using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObserverNetLite.API.Middlewares;
using ObserverNetLite.Service.Abstractions;
using ObserverNetLite.Service.DTOs;

namespace ObserverNetLite.API.Endpoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/roles");

        group.MapGet("/", async ([FromServices] IRoleService roleService) =>
        {
            try
            {
                var roles = await roleService.GetAllRolesAsync();
                return Results.Ok(roles);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin"))
        .WithName("GetAllRoles")
        .WithOpenApi();

        group.MapGet("/{id:guid}", async (
            [FromRoute] Guid id,
            [FromServices] IRoleService roleService) =>
        {
            try
            {
                var role = await roleService.GetRoleByIdAsync(id);
                if (role == null)
                    return Results.NotFound(new { message = "Role bulunamadı." });

                return Results.Ok(role);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin"))
        .WithName("GetRoleById")
        .WithOpenApi();

        group.MapGet("/{id:guid}/permissions", async (
            [FromRoute] Guid id,
            [FromServices] IRoleService roleService) =>
        {
            try
            {
                var roleWithPermissions = await roleService.GetRoleWithPermissionsAsync(id);
                if (roleWithPermissions == null)
                    return Results.NotFound(new { message = "Role bulunamadı." });

                return Results.Ok(roleWithPermissions);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin"))
        .WithName("GetRoleWithPermissions")
        .WithOpenApi();

        group.MapPost("/", async (
            [FromBody] CreateRoleDto createRoleDto,
            [FromServices] IRoleService roleService) =>
        {
            try
            {
                var role = await roleService.CreateRoleAsync(createRoleDto);
                return Results.Created($"/api/roles/{role.Id}", role);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin"))
        .WithName("CreateRole")
        .WithOpenApi();

        group.MapPut("/{id:guid}", async (
            [FromRoute] Guid id,
            [FromBody] RoleDto roleDto,
            [FromServices] IRoleService roleService) =>
        {
            try
            {
                if (id != roleDto.Id)
                    return Results.BadRequest(new { message = "ID uyuşmazlığı." });

                var updatedRole = await roleService.UpdateRoleAsync(roleDto);
                if (updatedRole == null)
                    return Results.NotFound(new { message = "Role bulunamadı." });

                return Results.Ok(updatedRole);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin"))
        .WithName("UpdateRole")
        .WithOpenApi();

        group.MapDelete("/{id:guid}", async (
            [FromRoute] Guid id,
            [FromServices] IRoleService roleService) =>
        {
            try
            {
                var result = await roleService.DeleteRoleAsync(id);
                if (!result)
                    return Results.NotFound(new { message = "Role bulunamadı." });

                return Results.Ok(new { message = "Role başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin"))
        .WithName("DeleteRole")
        .WithOpenApi();

        group.MapPost("/{id:guid}/assign-permissions", async (
            [FromRoute] Guid id,
            [FromBody] AssignPermissionsDto assignPermissionsDto,
            [FromServices] IRoleService roleService) =>
        {
            try
            {
                // Ensure the ID from route matches the DTO
                if (id != assignPermissionsDto.RoleId)
                    return Results.BadRequest(new { message = "Role ID uyuşmazlığı." });

                var result = await roleService.AssignPermissionsToRoleAsync(assignPermissionsDto);
                if (!result)
                    return Results.BadRequest(new { message = "İzinler atanamadı." });

                return Results.Ok(new { message = "İzinler başarıyla atandı." });
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin"))
        .WithName("AssignPermissions")
        .WithOpenApi();
    }
}
