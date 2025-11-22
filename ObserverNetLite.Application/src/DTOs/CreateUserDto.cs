namespace ObserverNetLite.Application.DTOs;

public class CreateUserDto
{
    public string UserName { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public string Role { get; set; } = string.Empty;
}
