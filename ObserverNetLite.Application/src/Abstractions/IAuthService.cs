using ObserverNetLite.Application.DTOs;
using System.Threading.Tasks;

namespace ObserverNetLite.Application.Abstractions
{
    public interface IAuthService : IService
    {
        Task<TokenResponseDto> GenerateTokenAsync(string userName, string role);
    }
}
