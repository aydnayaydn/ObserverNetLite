namespace ObserverNetLite.Core.Entities;

public class User
{
    public Guid Id { get; set; }

    public required string UserName { get; set; }
    
    public required string Password { get; set; } 

    public required string Role { get; set; } // admin, stackholder

    public string? Email { get; set; }

    public string? PasswordResetToken { get; set; }

    public DateTime? PasswordResetTokenExpiry { get; set; }
}