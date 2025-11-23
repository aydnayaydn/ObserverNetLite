namespace ObserverNetLite.Service.DTOs;

public class CreateRoleDto
{
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public bool IsActive { get; set; } = true;
}
