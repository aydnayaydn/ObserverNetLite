using ObserverNetLite.Service.DTOs;
using System.Threading.Tasks;

namespace ObserverNetLite.Service.Abstractions
{
    public interface IAuthService : IService
    {
        Task<TokenResponseDto> GenerateTokenAsync(string userName, string role);
    }
}
