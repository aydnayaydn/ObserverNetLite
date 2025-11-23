using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ObserverNetLite.Service.Abstractions;
using ObserverNetLite.Service.DTOs;
using ObserverNetLite.Core.Abstractions;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Service.Services;

public class RoleService : IRoleService
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IRepository<RolePermission> _rolePermissionRepository;
    private readonly IRepository<Permission> _permissionRepository;
    private readonly IMapper _mapper;

    public RoleService(
        IRepository<Role> roleRepository,
        IRepository<RolePermission> rolePermissionRepository,
        IRepository<Permission> permissionRepository,
        IMapper mapper)
    {
        _roleRepository = roleRepository;
        _rolePermissionRepository = rolePermissionRepository;
        _permissionRepository = permissionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }

    public async Task<RoleDto?> GetRoleByIdAsync(Guid roleId)
    {
        var role = await _roleRepository.GetByIdAsync(roleId);
        if (role == null) return null;
        
        return _mapper.Map<RoleDto>(role);
    }

    public async Task<RoleWithPermissionsDto?> GetRoleWithPermissionsAsync(Guid roleId)
    {
        var role = await _roleRepository.GetByIdAsync(roleId);
        if (role == null) return null;
        
        // Get role permissions
        var rolePermissions = await _rolePermissionRepository.FindAsync(rp => rp.RoleId == roleId);
        var permissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList();
        
        // Get permissions
        var allPermissions = await _permissionRepository.GetAllAsync();
        var permissions = allPermissions.Where(p => permissionIds.Contains(p.Id)).ToList();
        
        var result = _mapper.Map<RoleWithPermissionsDto>(role);
        result.Permissions = _mapper.Map<List<PermissionDto>>(permissions);
        
        return result;
    }

    public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto)
    {
        var role = _mapper.Map<Role>(createRoleDto);
        var createdRole = await _roleRepository.AddAsync(role);
        await _roleRepository.SaveChangesAsync();
        
        return _mapper.Map<RoleDto>(createdRole);
    }

    public async Task<RoleDto?> UpdateRoleAsync(RoleDto roleDto)
    {
        var existingRole = await _roleRepository.GetByIdAsync(roleDto.Id);
        if (existingRole == null) return null;
        
        _mapper.Map(roleDto, existingRole);
        await _roleRepository.UpdateAsync(existingRole);
        await _roleRepository.SaveChangesAsync();
        
        return _mapper.Map<RoleDto>(existingRole);
    }

    public async Task<bool> DeleteRoleAsync(Guid roleId)
    {
        var role = await _roleRepository.GetByIdAsync(roleId);
        if (role == null) return false;
        
        await _roleRepository.DeleteAsync(role);
        await _roleRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AssignPermissionsToRoleAsync(AssignPermissionsDto assignPermissionsDto)
    {
        // Remove existing permissions for this role
        var existingRolePermissions = await _rolePermissionRepository
            .FindAsync(rp => rp.RoleId == assignPermissionsDto.RoleId);
        
        foreach (var rp in existingRolePermissions)
        {
            await _rolePermissionRepository.DeleteAsync(rp);
        }

        // Add new permissions
        foreach (var permissionId in assignPermissionsDto.PermissionIds)
        {
            var rolePermission = new RolePermission
            {
                RoleId = assignPermissionsDto.RoleId,
                PermissionId = permissionId
            };
            await _rolePermissionRepository.AddAsync(rolePermission);
        }

        await _rolePermissionRepository.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<PermissionDto>> GetRolePermissionsAsync(Guid roleId)
    {
        var rolePermissions = await _rolePermissionRepository.FindAsync(rp => rp.RoleId == roleId);
        var permissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList();
        
        var allPermissions = await _permissionRepository.GetAllAsync();
        var permissions = allPermissions.Where(p => permissionIds.Contains(p.Id));
        
        return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
    }
}
