namespace ObserverNetLite.Core.Entities;

public class Permission : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    
    public string Code { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public string Category { get; set; } = string.Empty; // Menu, User, Role, etc.
    
    // Navigation properties
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    
    public ICollection<MenuPermission> MenuPermissions { get; set; } = new List<MenuPermission>();
}
