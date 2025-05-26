using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObserverNetLite.Core.Helpers;
using ObserverNetLite.Entities;

namespace ObserverNetLite.Data.Mappings;
public class UserMapping
{
    public static void OnModelCreating(EntityTypeBuilder<User> builder)
    {
        _ = builder.Property(u => u.UserName)
            .HasMaxLength(200)
            .IsRequired();

        _ = builder.Property(u => u.Password)
            .HasMaxLength(20)
            .IsRequired();

        _ = builder.Property(u => u.Role)
            .HasMaxLength(50)
            .IsRequired();

        SeedData(builder);
    }

    private static void SeedData(EntityTypeBuilder<User> modelBuilder)
    {
        modelBuilder.HasData(
            new User
            {
                Id = Guid.Parse("E0CB33F3-591A-4A25-AABA-BD05F796B5FB"),
                UserName = "observer",
                Password = EncryptionHelper.ComputeMd5Hash("lite_1qaz"),
                Role = "admin"
            }
        );
    }
}