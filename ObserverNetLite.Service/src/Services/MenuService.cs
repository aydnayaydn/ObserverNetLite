using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ObserverNetLite.Service.Abstractions;
using ObserverNetLite.Service.DTOs;
using ObserverNetLite.Core.Abstractions;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Service.Services;

public class MenuService : IMenuService
{
    private readonly IRepository<Menu> _menuRepository;
    private readonly IRepository<MenuPermission> _menuPermissionRepository;
    private readonly IRepository<RolePermission> _rolePermissionRepository;
    private readonly IMapper _mapper;

    public MenuService(
        IRepository<Menu> menuRepository,
        IRepository<MenuPermission> menuPermissionRepository,
        IRepository<RolePermission> rolePermissionRepository,
        IMapper mapper)
    {
        _menuRepository = menuRepository;
        _menuPermissionRepository = menuPermissionRepository;
        _rolePermissionRepository = rolePermissionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MenuDto>> GetAllMenusAsync()
    {
        var menus = await _menuRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<MenuDto>>(menus);
    }

    public async Task<IEnumerable<MenuDto>> GetMenusByRoleAsync(Guid roleId)
    {
        // Get all permissions for this role
        var rolePermissions = await _rolePermissionRepository.FindAsync(rp => rp.RoleId == roleId);
        var permissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList();

        // Get menus that require these permissions
        var menuPermissions = await _menuPermissionRepository.FindAsync(
            mp => permissionIds.Contains(mp.PermissionId));

        var menuIds = menuPermissions.Select(mp => mp.MenuId).Distinct().ToList();
        var allMenus = await _menuRepository.GetAllAsync();
        var menus = allMenus.Where(m => menuIds.Contains(m.Id) && m.IsActive);
        
        return _mapper.Map<IEnumerable<MenuDto>>(menus);
    }

    public async Task<MenuDto?> GetMenuByIdAsync(Guid menuId)
    {
        var menu = await _menuRepository.GetByIdAsync(menuId);
        if (menu == null) return null;
        
        return _mapper.Map<MenuDto>(menu);
    }

    public async Task<MenuDto> CreateMenuAsync(CreateMenuDto createMenuDto)
    {
        var menu = _mapper.Map<Menu>(createMenuDto);
        var createdMenu = await _menuRepository.AddAsync(menu);
        await _menuRepository.SaveChangesAsync();
        
        return _mapper.Map<MenuDto>(createdMenu);
    }

    public async Task<MenuDto?> UpdateMenuAsync(MenuDto menuDto)
    {
        var existingMenu = await _menuRepository.GetByIdAsync(menuDto.Id);
        if (existingMenu == null) return null;
        
        _mapper.Map(menuDto, existingMenu);
        await _menuRepository.UpdateAsync(existingMenu);
        await _menuRepository.SaveChangesAsync();
        
        return _mapper.Map<MenuDto>(existingMenu);
    }

    public async Task<bool> DeleteMenuAsync(Guid menuId)
    {
        var menu = await _menuRepository.GetByIdAsync(menuId);
        if (menu == null) return false;
        
        await _menuRepository.DeleteAsync(menu);
        await _menuRepository.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<MenuDto>> GetMenuHierarchyAsync()
    {
        var allMenus = (await _menuRepository.FindAsync(m => m.IsActive)).ToList();
        var rootMenus = allMenus.Where(m => m.ParentId == null).OrderBy(m => m.Order).ToList();
        
        var result = new List<MenuDto>();
        foreach (var rootMenu in rootMenus)
        {
            var menuDto = _mapper.Map<MenuDto>(rootMenu);
            menuDto.Children = BuildMenuHierarchy(rootMenu.Id, allMenus);
            result.Add(menuDto);
        }
        
        return result;
    }

    private List<MenuDto> BuildMenuHierarchy(Guid parentId, List<Menu> allMenus)
    {
        var children = allMenus.Where(m => m.ParentId == parentId).OrderBy(m => m.Order).ToList();
        var result = new List<MenuDto>();
        
        foreach (var child in children)
        {
            var childDto = _mapper.Map<MenuDto>(child);
            childDto.Children = BuildMenuHierarchy(child.Id, allMenus);
            result.Add(childDto);
        }
        
        return result;
    }
}
