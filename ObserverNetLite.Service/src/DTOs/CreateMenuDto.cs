namespace ObserverNetLite.Service.DTOs;

public class CreateMenuDto
{
    public string Name { get; set; } = string.Empty;
    
    public string? Title { get; set; }
    
    public string? Icon { get; set; }
    
    public string? Route { get; set; }
    
    public int Order { get; set; }
    
    public Guid? ParentId { get; set; }
    
    public bool IsActive { get; set; } = true;
}
