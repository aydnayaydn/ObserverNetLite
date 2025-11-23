using ObserverNetLite.Service.DTOs;

namespace ObserverNetLite.Service.Abstractions;

public interface IMenuService : IService
{
    Task<IEnumerable<MenuDto>> GetAllMenusAsync();
    
    Task<IEnumerable<MenuDto>> GetMenusByRoleAsync(Guid roleId);
    
    Task<MenuDto?> GetMenuByIdAsync(Guid menuId);
    
    Task<MenuDto> CreateMenuAsync(CreateMenuDto createMenuDto);
    
    Task<MenuDto?> UpdateMenuAsync(MenuDto menuDto);
    
    Task<bool> DeleteMenuAsync(Guid menuId);
    
    Task<IEnumerable<MenuDto>> GetMenuHierarchyAsync();
}
