using System;

namespace ObserverNetLite.Service.DTOs
{
    public class LoginDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
