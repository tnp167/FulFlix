using AutoMapper;
using backend.DTOs;
using backend.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());

        CreateMap<User, UserDto>();

        CreateMap<Auth0UserResponseDto, User>()
            .ForMember(dest => dest.EmailVerified, opt => opt.MapFrom(src => src.EmailVerified))
            .ForMember(dest => dest.Auth0Id, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserMetadata.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserMetadata.LastName));
    }
}