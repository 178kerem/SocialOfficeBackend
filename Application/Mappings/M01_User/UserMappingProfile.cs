using AutoMapper;
using SocialOffice.Application.DTOs.M01_UserManagement;
using SocialOffice.Domain.Entitites.M01_User;
namespace SocialOffice.Application.Mappings.M01_User;
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // User -> UserReadDto
        CreateMap<User, UserReadDto>();

        // UserCreateDto -> User
        CreateMap<UserCreateDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Şifre servis katmanında hash'lenecek
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));

        // UserUpdateDto -> User
        CreateMap<UserUpdateDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Şifre ayrı güncellenecek
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

        // UserDeleteDto -> User (Opsiyonel, eğer silmede kullanıyorsanız)
        CreateMap<UserDeleteDto, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            
    }
}