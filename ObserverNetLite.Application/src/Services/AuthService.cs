using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ObserverNetLite.Application.Abstractions;
using ObserverNetLite.Application.DTOs;
using ObserverNetLite.Application.Settings;

namespace ObserverNetLite.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUserService _userService;

        public AuthService(IOptions<JwtSettings> jwtSettings, IUserService userService)
        {
            _jwtSettings = jwtSettings.Value;
            _userService = userService;
        }

        public async Task<TokenResponseDto> GenerateTokenAsync(string userName, string role)
        {
            var user = await _userService.GetUserByUserNameAsync(userName);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return new TokenResponseDto
            {
                Token = token,
                Expiration = jwtSecurityToken.ValidTo
            };
        }
    }
}
