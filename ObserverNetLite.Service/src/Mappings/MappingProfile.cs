using AutoMapper;
using ObserverNetLite.Service.DTOs;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Service.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<CreateUserDto, User>();
    }
}
