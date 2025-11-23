using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObserverNetLite.API.Middlewares;
using ObserverNetLite.Service.Abstractions;
using ObserverNetLite.Service.DTOs;

namespace ObserverNetLite.API.Endpoints;

public static class MenuEndpoints
{
    public static void MapMenuEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/menus");

        group.MapGet("/", async ([FromServices] IMenuService menuService) =>
        {
            try
            {
                var menus = await menuService.GetAllMenusAsync();
                return Results.Ok(menus);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin"))
        .WithName("GetAllMenus")
        .WithOpenApi();

        group.MapGet("/hierarchy", async ([FromServices] IMenuService menuService) =>
        {
            try
            {
                var menus = await menuService.GetMenuHierarchyAsync();
                return Results.Ok(menus);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin", "guest"))
        .WithName("GetMenuHierarchy")
        .WithOpenApi();

        group.MapGet("/role/{roleId:guid}", async (
            [FromRoute] Guid roleId,
            [FromServices] IMenuService menuService) =>
        {
            try
            {
                var menus = await menuService.GetMenusByRoleAsync(roleId);
                return Results.Ok(menus);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin", "guest"))
        .WithName("GetMenusByRole")
        .WithOpenApi();

        group.MapGet("/{id:guid}", async (
            [FromRoute] Guid id,
            [FromServices] IMenuService menuService) =>
        {
            try
            {
                var menu = await menuService.GetMenuByIdAsync(id);
                if (menu == null)
                    return Results.NotFound(new { message = "Menu bulunamadı." });

                return Results.Ok(menu);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin"))
        .WithName("GetMenuById")
        .WithOpenApi();

        group.MapPost("/", async (
            [FromBody] CreateMenuDto createMenuDto,
            [FromServices] IMenuService menuService) =>
        {
            try
            {
                var menu = await menuService.CreateMenuAsync(createMenuDto);
                return Results.Created($"/api/menus/{menu.Id}", menu);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin"))
        .WithName("CreateMenu")
        .WithOpenApi();

        group.MapPut("/{id:guid}", async (
            [FromRoute] Guid id,
            [FromBody] MenuDto menuDto,
            [FromServices] IMenuService menuService) =>
        {
            try
            {
                if (id != menuDto.Id)
                    return Results.BadRequest(new { message = "ID uyuşmazlığı." });

                var updatedMenu = await menuService.UpdateMenuAsync(menuDto);
                if (updatedMenu == null)
                    return Results.NotFound(new { message = "Menu bulunamadı." });

                return Results.Ok(updatedMenu);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin"))
        .WithName("UpdateMenu")
        .WithOpenApi();

        group.MapDelete("/{id:guid}", async (
            [FromRoute] Guid id,
            [FromServices] IMenuService menuService) =>
        {
            try
            {
                var result = await menuService.DeleteMenuAsync(id);
                if (!result)
                    return Results.NotFound(new { message = "Menu bulunamadı." });

                return Results.Ok(new { message = "Menu başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithMetadata(new RequireRoleAttribute("admin"))
        .WithName("DeleteMenu")
        .WithOpenApi();
    }
}
