using AutoMapper;
using ObserverNetLite.Service.DTOs;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Service.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.RoleIds, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.RoleId)))
            .ForMember(dest => dest.RoleNames, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore());
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore());
        
        // Role mappings
        CreateMap<Role, RoleDto>();
        CreateMap<RoleDto, Role>();
        CreateMap<CreateRoleDto, Role>();
        
        // Permission mappings
        CreateMap<Permission, PermissionDto>();
        
        // Menu mappings
        CreateMap<Menu, MenuDto>();
        CreateMap<CreateMenuDto, Menu>();
        
        // Role with permissions
        CreateMap<Role, RoleWithPermissionsDto>()
            .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => 
                src.RolePermissions.Select(rp => rp.Permission)));
    }
}
