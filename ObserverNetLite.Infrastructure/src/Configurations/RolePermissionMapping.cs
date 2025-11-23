using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Infrastructure.Configurations;

public class RolePermissionMapping : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("RolePermissions");

        builder.HasKey(rp => rp.Id);

        builder.HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(rp => new { rp.RoleId, rp.PermissionId })
            .IsUnique();

        // Admin role - all permissions
        var adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var guestRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        // Seed data - Admin gets all permissions
        builder.HasData(
            // Admin - User permissions
            new RolePermission { Id = Guid.Parse("50000001-0000-0000-0000-000000000001"), RoleId = adminRoleId, PermissionId = Guid.Parse("10000001-0000-0000-0000-000000000001") },
            new RolePermission { Id = Guid.Parse("50000002-0000-0000-0000-000000000002"), RoleId = adminRoleId, PermissionId = Guid.Parse("10000002-0000-0000-0000-000000000002") },
            new RolePermission { Id = Guid.Parse("50000003-0000-0000-0000-000000000003"), RoleId = adminRoleId, PermissionId = Guid.Parse("10000003-0000-0000-0000-000000000003") },
            new RolePermission { Id = Guid.Parse("50000004-0000-0000-0000-000000000004"), RoleId = adminRoleId, PermissionId = Guid.Parse("10000004-0000-0000-0000-000000000004") },

            // Admin - Role permissions
            new RolePermission { Id = Guid.Parse("50000005-0000-0000-0000-000000000005"), RoleId = adminRoleId, PermissionId = Guid.Parse("20000001-0000-0000-0000-000000000001") },
            new RolePermission { Id = Guid.Parse("50000006-0000-0000-0000-000000000006"), RoleId = adminRoleId, PermissionId = Guid.Parse("20000002-0000-0000-0000-000000000002") },
            new RolePermission { Id = Guid.Parse("50000007-0000-0000-0000-000000000007"), RoleId = adminRoleId, PermissionId = Guid.Parse("20000003-0000-0000-0000-000000000003") },
            new RolePermission { Id = Guid.Parse("50000008-0000-0000-0000-000000000008"), RoleId = adminRoleId, PermissionId = Guid.Parse("20000004-0000-0000-0000-000000000004") },

            // Admin - Menu permissions
            new RolePermission { Id = Guid.Parse("50000009-0000-0000-0000-000000000009"), RoleId = adminRoleId, PermissionId = Guid.Parse("30000001-0000-0000-0000-000000000001") },
            new RolePermission { Id = Guid.Parse("5000000A-0000-0000-0000-00000000000A"), RoleId = adminRoleId, PermissionId = Guid.Parse("30000002-0000-0000-0000-000000000002") },
            new RolePermission { Id = Guid.Parse("5000000B-0000-0000-0000-00000000000B"), RoleId = adminRoleId, PermissionId = Guid.Parse("30000003-0000-0000-0000-000000000003") },
            new RolePermission { Id = Guid.Parse("5000000C-0000-0000-0000-00000000000C"), RoleId = adminRoleId, PermissionId = Guid.Parse("30000004-0000-0000-0000-000000000004") },

            // Guest - Only view permissions
            new RolePermission { Id = Guid.Parse("5000000D-0000-0000-0000-00000000000D"), RoleId = guestRoleId, PermissionId = Guid.Parse("10000001-0000-0000-0000-000000000001") }, // USER_VIEW
            new RolePermission { Id = Guid.Parse("5000000E-0000-0000-0000-00000000000E"), RoleId = guestRoleId, PermissionId = Guid.Parse("30000001-0000-0000-0000-000000000001") }  // MENU_VIEW
        );
    }
}
