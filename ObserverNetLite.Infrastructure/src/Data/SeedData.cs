using Microsoft.EntityFrameworkCore;
using ObserverNetLite.Core.Entities;
using ObserverNetLite.Core.Helpers;

namespace ObserverNetLite.Data;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Use existing Role IDs
        var adminRoleId = new Guid("11111111-1111-1111-1111-111111111111");
        var guestRoleId = new Guid("22222222-2222-2222-2222-222222222222");

        // Permissions
        var permissions = new List<Permission>
        {
            // Dashboard
            new Permission { Id = new Guid("10000000-0000-0000-0000-000000000001"), Code = "view_dashboard", Name = "View Dashboard", Description = "Access to dashboard page", Category = "Dashboard" },
            
            // Users
            new Permission { Id = new Guid("10000000-0000-0000-0000-000000000002"), Code = "view_users", Name = "View Users", Description = "Access to users page", Category = "Users" },
            new Permission { Id = new Guid("10000000-0000-0000-0000-000000000003"), Code = "create_user", Name = "Create User", Description = "Create new users", Category = "Users" },
            new Permission { Id = new Guid("10000000-0000-0000-0000-000000000004"), Code = "edit_user", Name = "Edit User", Description = "Edit existing users", Category = "Users" },
            new Permission { Id = new Guid("10000000-0000-0000-0000-000000000005"), Code = "delete_user", Name = "Delete User", Description = "Delete users", Category = "Users" },
            
            // Roles
            new Permission { Id = new Guid("10000000-0000-0000-0000-000000000006"), Code = "view_roles", Name = "View Roles", Description = "Access to roles page", Category = "Roles" },
            new Permission { Id = new Guid("10000000-0000-0000-0000-000000000007"), Code = "create_role", Name = "Create Role", Description = "Create new roles", Category = "Roles" },
            new Permission { Id = new Guid("10000000-0000-0000-0000-000000000008"), Code = "edit_role", Name = "Edit Role", Description = "Edit existing roles", Category = "Roles" },
            new Permission { Id = new Guid("10000000-0000-0000-0000-000000000009"), Code = "delete_role", Name = "Delete Role", Description = "Delete roles", Category = "Roles" },
            new Permission { Id = new Guid("10000000-0000-0000-0000-00000000000A"), Code = "assign_permissions", Name = "Assign Permissions", Description = "Assign permissions to roles", Category = "Roles" },
            
            // Menus
            new Permission { Id = new Guid("10000000-0000-0000-0000-00000000000B"), Code = "view_menus", Name = "View Menus", Description = "Access to menus page", Category = "Menus" },
            new Permission { Id = new Guid("10000000-0000-0000-0000-00000000000C"), Code = "create_menu", Name = "Create Menu", Description = "Create new menus", Category = "Menus" },
            new Permission { Id = new Guid("10000000-0000-0000-0000-00000000000D"), Code = "edit_menu", Name = "Edit Menu", Description = "Edit existing menus", Category = "Menus" },
            new Permission { Id = new Guid("10000000-0000-0000-0000-00000000000E"), Code = "delete_menu", Name = "Delete Menu", Description = "Delete menus", Category = "Menus" }
        };

        modelBuilder.Entity<Permission>().HasData(permissions);

        // Assign all permissions to admin role
        var rolePermissions = new List<RolePermission>();
        int counter = 1;
        foreach (var permission in permissions)
        {
            rolePermissions.Add(new RolePermission
            {
                Id = new Guid($"20000000-0000-0000-0000-{counter:D12}"),
                RoleId = adminRoleId,
                PermissionId = permission.Id
            });
            counter++;
        }

        // Assign only view permissions to guest role
        foreach (var permission in permissions.Where(p => p.Code.StartsWith("view_")))
        {
            rolePermissions.Add(new RolePermission
            {
                Id = new Guid($"20000000-0000-0000-0000-{counter:D12}"),
                RoleId = guestRoleId,
                PermissionId = permission.Id
            });
            counter++;
        }

        modelBuilder.Entity<RolePermission>().HasData(rolePermissions);
    }
}
