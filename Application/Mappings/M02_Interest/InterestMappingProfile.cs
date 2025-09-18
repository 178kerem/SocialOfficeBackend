using AutoMapper;
using SocialOffice.Application.DTOs.M03_Interest;
using SocialOffice.Domain.Entitites.M02_Interest;


namespace SocialOffice.Application.Mappings.M02_Interest
{
    public class InterestMappingProfile : Profile
    {
        public InterestMappingProfile()
        {
            // Interest -> InterestDto
            CreateMap<Interest, InterestDto>();

            CreateMap<InterestCreateDto, Interest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

            // InterestDeleteDto -> Interest
            CreateMap<InterestDeleteDto, Interest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
