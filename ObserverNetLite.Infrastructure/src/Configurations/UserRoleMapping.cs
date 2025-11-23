using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Infrastructure.Configurations;

public class UserRoleMapping : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(ur => ur.Id);

        builder.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ur => new { ur.UserId, ur.RoleId })
            .IsUnique();

        // Seed data
        var adminUserId = Guid.Parse("8e445865-a24d-4543-a6c6-9443d048cdb9");
        var guestUserId = Guid.Parse("9e225865-a24d-4543-a6c6-9443d048cdb9");
        var adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var guestRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        builder.HasData(
            new UserRole 
            { 
                Id = Guid.Parse("70000001-0000-0000-0000-000000000001"), 
                UserId = adminUserId, 
                RoleId = adminRoleId 
            },
            new UserRole 
            { 
                Id = Guid.Parse("70000002-0000-0000-0000-000000000002"), 
                UserId = guestUserId, 
                RoleId = guestRoleId 
            }
        );
    }
}
