using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Infrastructure.Configurations;

public class MenuMapping : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menus");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Title)
            .HasMaxLength(200);

        builder.Property(m => m.Icon)
            .HasMaxLength(100);

        builder.Property(m => m.Route)
            .HasMaxLength(500);

        builder.Property(m => m.Order)
            .IsRequired();

        builder.Property(m => m.IsActive)
            .IsRequired();

        builder.HasOne(m => m.Parent)
            .WithMany(m => m.Children)
            .HasForeignKey(m => m.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data
        var dashboardId = Guid.Parse("40000001-0000-0000-0000-000000000001");
        var usersId = Guid.Parse("40000002-0000-0000-0000-000000000002");
        var rolesId = Guid.Parse("40000003-0000-0000-0000-000000000003");
        var settingsId = Guid.Parse("40000004-0000-0000-0000-000000000004");
        var menusId = Guid.Parse("40000005-0000-0000-0000-000000000005");

        builder.HasData(
            new Menu { Id = dashboardId, Name = "dashboard", Title = "Dashboard", Icon = "dashboard", Route = "/dashboard", Order = 1, IsActive = true },
            new Menu { Id = usersId, Name = "users", Title = "Users", Icon = "people", Route = "/users", Order = 2, IsActive = true },
            new Menu { Id = rolesId, Name = "roles", Title = "Roles", Icon = "shield", Route = "/roles", Order = 3, IsActive = true },
            new Menu { Id = menusId, Name = "menus", Title = "Menus", Icon = "menu", Route = "/menus", Order = 4, IsActive = true },
            new Menu { Id = settingsId, Name = "settings", Title = "Settings", Icon = "settings", Route = "/settings", Order = 5, IsActive = true }
        );
    }
}
