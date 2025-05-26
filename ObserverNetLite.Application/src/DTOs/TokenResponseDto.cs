using System;

namespace ObserverNetLite.Application.DTOs
{
    public class TokenResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
