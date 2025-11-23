namespace ObserverNetLite.Service.DTOs;

public class AssignPermissionsDto
{
    public Guid RoleId { get; set; }
    
    public List<Guid> PermissionIds { get; set; } = new();
}
