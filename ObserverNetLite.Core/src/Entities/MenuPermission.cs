namespace ObserverNetLite.Core.Entities;

public class MenuPermission : BaseEntity
{
    public Guid MenuId { get; set; }
    
    public Guid PermissionId { get; set; }
    
    // Navigation properties
    public Menu Menu { get; set; } = null!;
    
    public Permission Permission { get; set; } = null!;
}
