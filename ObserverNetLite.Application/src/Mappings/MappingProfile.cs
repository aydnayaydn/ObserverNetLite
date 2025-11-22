using AutoMapper;
using ObserverNetLite.Application.DTOs;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<CreateUserDto, User>();
    }
}
