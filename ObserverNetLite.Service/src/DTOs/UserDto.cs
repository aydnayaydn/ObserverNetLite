namespace ObserverNetLite.Service.DTOs;

public class UserDto
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = string.Empty;
    
    public List<Guid> RoleIds { get; set; } = new();
    
    public List<string> RoleNames { get; set; } = new();

    public string? Email { get; set; }
    
    public List<PermissionDto> Permissions { get; set; } = new();
}
