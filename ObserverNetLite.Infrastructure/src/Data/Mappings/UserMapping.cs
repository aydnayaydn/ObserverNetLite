using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObserverNetLite.Entities;

namespace ObserverNetLite.Data.Mappings;
public class UserMapping
{
    public static void OnModelCreating(EntityTypeBuilder<User> builder)
    {
        _ = builder.Property(u => u.UserName)
            .HasMaxLength(200)
            .IsRequired();

        _ = builder.Property(u => u.Role)
            .HasMaxLength(50)
            .IsRequired();

        SeedData(builder);
    }

    private static void SeedData(EntityTypeBuilder<User> modelBuilder)
    {
       
       //TODO: Add seed data here
    }
}