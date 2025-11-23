using ObserverNetLite.Service.DTOs;

namespace ObserverNetLite.Service.Abstractions;

public interface IPermissionService : IService
{
    Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync();
    
    Task<PermissionDto?> GetPermissionByIdAsync(Guid permissionId);
    
    Task<IEnumerable<PermissionDto>> GetPermissionsByCategoryAsync(string category);
}
