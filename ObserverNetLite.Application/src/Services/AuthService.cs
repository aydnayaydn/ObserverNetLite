using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ObserverNetLite.Application.Abstractions;
using ObserverNetLite.Application.DTOs;
using ObserverNetLite.Application.Settings;
using ObserverNetLite.Core.Abstractions;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IRepository<User> _userRepository;

        public AuthService(IOptions<JwtSettings> jwtSettings, IRepository<User> userRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _userRepository = userRepository;
        }

        public async Task<TokenResponseDto> GenerateTokenAsync(string userName, string role)
        {
            var user = (await _userRepository.FindAsync(u => u.UserName == userName)).FirstOrDefault();
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
