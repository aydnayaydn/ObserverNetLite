using ObserverNetLite.Service.DTOs;

namespace ObserverNetLite.Service.Abstractions;

public interface IRoleService : IService
{
    Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    
    Task<RoleDto?> GetRoleByIdAsync(Guid roleId);
    
    Task<RoleWithPermissionsDto?> GetRoleWithPermissionsAsync(Guid roleId);
    
    Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto);
    
    Task<RoleDto?> UpdateRoleAsync(RoleDto roleDto);
    
    Task<bool> DeleteRoleAsync(Guid roleId);
    
    Task<bool> AssignPermissionsToRoleAsync(AssignPermissionsDto assignPermissionsDto);
    
    Task<IEnumerable<PermissionDto>> GetRolePermissionsAsync(Guid roleId);
}
