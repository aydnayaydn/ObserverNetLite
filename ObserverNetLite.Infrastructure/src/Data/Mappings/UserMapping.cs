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
                Id = Guid.Parse("8e445865-a24d-4543-a6c6-9443d048cdb9"),
                UserName = "admin",
                Password = EncryptionHelper.ComputeMd5Hash("admin123"), // 0192023a7bbd73250516f069df18b500
                Role = "admin"
            },
            new User
            {
                Id = Guid.Parse("9e225865-a24d-4543-a6c6-9443d048cdb9"),
                UserName = "user",
                Password = EncryptionHelper.ComputeMd5Hash("user123"), // e10adc3949ba59abbe56e057f20f883e
                Role = "user"
            }
        );
    }
}