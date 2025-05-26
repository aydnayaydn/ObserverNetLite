using ObserverNetLite.Application.DTOs;

namespace ObserverNetLite.Application.Abstractions;

public interface IUserService : IService
{
    Task<TokenResponseDto> AuthenticateAsync(LoginDto loginDto);
    
    Task<bool> ValidateUserAsync(string userName, string password);

    Task<UserDto?> GetUserByIdAsync(Guid userId);

    Task<UserDto?> GetUserByUserNameAsync(string userName);

    Task<IEnumerable<UserDto>> GetAllUsersAsync();

    Task<UserDto> CreateUserAsync(UserDto userDto);

    Task<UserDto?> UpdateUserAsync(UserDto userDto);

    Task<bool> DeleteUserAsync(Guid userId);
}
