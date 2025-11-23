using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Infrastructure.Configurations;

public class RoleMapping : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Description)
            .HasMaxLength(500);

        builder.Property(r => r.IsActive)
            .IsRequired();

        builder.HasIndex(r => r.Name)
            .IsUnique();

        // Seed data
        builder.HasData(
            new Role
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "admin",
                Description = "Administrator role with full access",
                IsActive = true
            },
            new Role
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "guest",
                Description = "Guest role with limited access",
                IsActive = true
            }
        );
    }
}
