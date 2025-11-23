using AutoMapper;
using ObserverNetLite.Service.Abstractions;
using ObserverNetLite.Service.DTOs;
using ObserverNetLite.Core.Abstractions;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Service.Services;

public class PermissionService : IPermissionService
{
    private readonly IRepository<Permission> _permissionRepository;
    private readonly IMapper _mapper;

    public PermissionService(
        IRepository<Permission> permissionRepository,
        IMapper mapper)
    {
        _permissionRepository = permissionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync()
    {
        var permissions = await _permissionRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
    }

    public async Task<PermissionDto?> GetPermissionByIdAsync(Guid permissionId)
    {
        var permission = await _permissionRepository.GetByIdAsync(permissionId);
        if (permission == null) return null;
        
        return _mapper.Map<PermissionDto>(permission);
    }

    public async Task<IEnumerable<PermissionDto>> GetPermissionsByCategoryAsync(string category)
    {
        var permissions = await _permissionRepository.FindAsync(p => p.Category == category);
        return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
    }
}
