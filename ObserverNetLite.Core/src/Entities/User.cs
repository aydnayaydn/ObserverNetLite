namespace ObserverNetLite.Core.Entities;

public class User : BaseEntity
{
    public required string UserName { get; set; }
    
    public required string Password { get; set; } 

    public string? Email { get; set; }

    public string? PasswordResetToken { get; set; }

    public DateTime? PasswordResetTokenExpiry { get; set; }
    
    // Navigation properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}