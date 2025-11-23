namespace ObserverNetLite.Service.DTOs;

public class MenuDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string? Title { get; set; }
    
    public string? Icon { get; set; }
    
    public string? Route { get; set; }
    
    public int Order { get; set; }
    
    public Guid? ParentId { get; set; }
    
    public bool IsActive { get; set; }
    
    public List<MenuDto>? Children { get; set; }
}
