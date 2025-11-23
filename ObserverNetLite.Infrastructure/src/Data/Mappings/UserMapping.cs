using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObserverNetLite.Core.Helpers;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Data.Mappings;
public class UserMapping
{
    public static void OnModelCreating(EntityTypeBuilder<User> builder)
    {
        _ = builder.Property(u => u.UserName)
            .HasMaxLength(200)
            .IsRequired();

        _ = builder.Property(u => u.Password)
            .HasMaxLength(255)
            .IsRequired();

        _ = builder.Property(u => u.Email)
            .HasMaxLength(255);

        _ = builder.Property(u => u.PasswordResetToken)
            .HasMaxLength(500);

        _ = builder.Property(u => u.PasswordResetTokenExpiry);

        SeedData(builder);
    }

    private static void SeedData(EntityTypeBuilder<User> modelBuilder)
    {
        modelBuilder.HasData(
            new User
            {
                Id = Guid.Parse("8e445865-a24d-4543-a6c6-9443d048cdb9"),
                UserName = "admin",
                Password = EncryptionHelper.ComputeMd5Hash("admin123"),
                Email = "admin@observernetlite.com"
            },
            new User
            {
                Id = Guid.Parse("9e225865-a24d-4543-a6c6-9443d048cdb9"),
                UserName = "guest",
                Password = EncryptionHelper.ComputeMd5Hash("guest123"),
                Email = "guest@observernetlite.com"
            }
        );
    }
}