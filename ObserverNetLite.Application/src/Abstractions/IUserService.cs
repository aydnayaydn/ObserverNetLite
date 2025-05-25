using ObserverNetLite.Application.DTOs;

namespace ObserverNetLite.Application.Abstractions;

public interface IUserService : IService
{
    Task<UserDto> GetUserByIdAsync(Guid userId);

    Task<UserDto> GetUserByUserNameAsync(string userName);

    Task<IEnumerable<UserDto>> GetAllUsersAsync();

    Task<UserDto> CreateUserAsync(UserDto userDto);

    Task<UserDto> UpdateUserAsync(UserDto userDto);

    Task<bool> DeleteUserAsync(Guid userId);
}
