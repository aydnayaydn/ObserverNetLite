using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Infrastructure.Configurations;

public class MenuPermissionMapping : IEntityTypeConfiguration<MenuPermission>
{
    public void Configure(EntityTypeBuilder<MenuPermission> builder)
    {
        builder.ToTable("MenuPermissions");

        builder.HasKey(mp => mp.Id);

        builder.HasOne(mp => mp.Menu)
            .WithMany(m => m.MenuPermissions)
            .HasForeignKey(mp => mp.MenuId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mp => mp.Permission)
            .WithMany(p => p.MenuPermissions)
            .HasForeignKey(mp => mp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(mp => new { mp.MenuId, mp.PermissionId })
            .IsUnique();

        // Seed data - Menu to permission mappings
        var menuViewPermId = Guid.Parse("30000001-0000-0000-0000-000000000001");
        
        var dashboardId = Guid.Parse("40000001-0000-0000-0000-000000000001");
        var usersId = Guid.Parse("40000002-0000-0000-0000-000000000002");
        var rolesId = Guid.Parse("40000003-0000-0000-0000-000000000003");
        var menusId = Guid.Parse("40000005-0000-0000-0000-000000000005");
        var settingsId = Guid.Parse("40000004-0000-0000-0000-000000000004");

        builder.HasData(
            // All menus require MENU_VIEW permission to be visible
            new MenuPermission { Id = Guid.Parse("60000001-0000-0000-0000-000000000001"), MenuId = dashboardId, PermissionId = menuViewPermId },
            new MenuPermission { Id = Guid.Parse("60000002-0000-0000-0000-000000000002"), MenuId = usersId, PermissionId = menuViewPermId },
            new MenuPermission { Id = Guid.Parse("60000003-0000-0000-0000-000000000003"), MenuId = rolesId, PermissionId = menuViewPermId },
            new MenuPermission { Id = Guid.Parse("60000004-0000-0000-0000-000000000004"), MenuId = menusId, PermissionId = menuViewPermId },
            new MenuPermission { Id = Guid.Parse("60000005-0000-0000-0000-000000000005"), MenuId = settingsId, PermissionId = menuViewPermId }
        );
    }
}
