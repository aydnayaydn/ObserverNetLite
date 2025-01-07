using ObserverNetLite.Entities;

namespace ObserverNetLite.Abstractions;

public interface IUserService
{
    Task<User> GetUserById(Guid userId);
    Task<User> GetUserByApiKey(string apiKey);
    Task Create (User user);
    Task Delete (Guid userId);
}