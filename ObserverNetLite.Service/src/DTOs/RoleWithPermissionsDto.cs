namespace ObserverNetLite.Service.DTOs;

public class RoleWithPermissionsDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public bool IsActive { get; set; }
    
    public List<PermissionDto> Permissions { get; set; } = new();
}
