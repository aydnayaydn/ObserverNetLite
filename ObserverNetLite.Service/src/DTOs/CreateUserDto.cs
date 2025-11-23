namespace ObserverNetLite.Service.DTOs;

public class CreateUserDto
{
    public string UserName { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public List<Guid> RoleIds { get; set; } = new();
    
    public string Email { get; set; } = string.Empty;
}
