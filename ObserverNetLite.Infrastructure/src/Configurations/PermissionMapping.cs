using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Infrastructure.Configurations;

public class PermissionMapping : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.Code)
            .IsUnique();

        // Seed data - User permissions
        var userViewId = Guid.Parse("10000001-0000-0000-0000-000000000001");
        var userCreateId = Guid.Parse("10000002-0000-0000-0000-000000000002");
        var userEditId = Guid.Parse("10000003-0000-0000-0000-000000000003");
        var userDeleteId = Guid.Parse("10000004-0000-0000-0000-000000000004");

        // Role permissions
        var roleViewId = Guid.Parse("20000001-0000-0000-0000-000000000001");
        var roleCreateId = Guid.Parse("20000002-0000-0000-0000-000000000002");
        var roleEditId = Guid.Parse("20000003-0000-0000-0000-000000000003");
        var roleDeleteId = Guid.Parse("20000004-0000-0000-0000-000000000004");

        // Menu permissions
        var menuViewId = Guid.Parse("30000001-0000-0000-0000-000000000001");
        var menuCreateId = Guid.Parse("30000002-0000-0000-0000-000000000002");
        var menuEditId = Guid.Parse("30000003-0000-0000-0000-000000000003");
        var menuDeleteId = Guid.Parse("30000004-0000-0000-0000-000000000004");

        builder.HasData(
            // User permissions
            new Permission { Id = userViewId, Name = "View Users", Code = "USER_VIEW", Category = "User", Description = "View user list and details" },
            new Permission { Id = userCreateId, Name = "Create User", Code = "USER_CREATE", Category = "User", Description = "Create new users" },
            new Permission { Id = userEditId, Name = "Edit User", Code = "USER_EDIT", Category = "User", Description = "Edit existing users" },
            new Permission { Id = userDeleteId, Name = "Delete User", Code = "USER_DELETE", Category = "User", Description = "Delete users" },

            // Role permissions
            new Permission { Id = roleViewId, Name = "View Roles", Code = "ROLE_VIEW", Category = "Role", Description = "View role list and details" },
            new Permission { Id = roleCreateId, Name = "Create Role", Code = "ROLE_CREATE", Category = "Role", Description = "Create new roles" },
            new Permission { Id = roleEditId, Name = "Edit Role", Code = "ROLE_EDIT", Category = "Role", Description = "Edit existing roles" },
            new Permission { Id = roleDeleteId, Name = "Delete Role", Code = "ROLE_DELETE", Category = "Role", Description = "Delete roles" },

            // Menu permissions
            new Permission { Id = menuViewId, Name = "View Menus", Code = "MENU_VIEW", Category = "Menu", Description = "View menu list and details" },
            new Permission { Id = menuCreateId, Name = "Create Menu", Code = "MENU_CREATE", Category = "Menu", Description = "Create new menus" },
            new Permission { Id = menuEditId, Name = "Edit Menu", Code = "MENU_EDIT", Category = "Menu", Description = "Edit existing menus" },
            new Permission { Id = menuDeleteId, Name = "Delete Menu", Code = "MENU_DELETE", Category = "Menu", Description = "Delete menus" }
        );
    }
}
