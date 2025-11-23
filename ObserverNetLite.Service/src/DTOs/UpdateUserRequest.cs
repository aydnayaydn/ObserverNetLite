namespace ObserverNetLite.Service.DTOs;

public class UpdateUserRequest
{
    public string? UserName { get; set; }
    
    public string? Email { get; set; }
    
    public string? Password { get; set; }
    
    public List<Guid>? RoleIds { get; set; }
}
