namespace ObserverNetLite.Service.DTOs;

public class ResetPasswordWithTokenDto
{
    public string Token { get; set; } = string.Empty;
    
    public string NewPassword { get; set; } = string.Empty;
}
