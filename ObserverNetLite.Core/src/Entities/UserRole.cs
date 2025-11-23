namespace ObserverNetLite.Core.Entities;

/// <summary>
/// Junction entity for User-Role many-to-many relationship
/// </summary>
public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}
