namespace ObserverNetLite.Service.DTOs;

public class UserDto
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = string.Empty;
    
    public string Role { get; set; } = string.Empty; // admin, stakeholder
}
