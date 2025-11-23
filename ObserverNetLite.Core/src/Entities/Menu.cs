namespace ObserverNetLite.Core.Entities;

public class Menu : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    
    public string? Title { get; set; }
    
    public string? Icon { get; set; }
    
    public string? Route { get; set; }
    
    public int Order { get; set; }
    
    public Guid? ParentId { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public Menu? Parent { get; set; }
    
    public ICollection<Menu> Children { get; set; } = new List<Menu>();
    
    public ICollection<MenuPermission> MenuPermissions { get; set; } = new List<MenuPermission>();
}
