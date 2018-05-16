using AutoMapper;
using Wacomi.API.Dto;
using Wacomi.API.Models;

namespace Wacomi.API.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(){
          CreateMap<AppUser, UserForReturnDto>();
          CreateMap<AppUserUpdateDto, AppUser>();
          CreateMap<AppUser, AppUserForReturnDto>();
          CreateMap<UserRegistrationDto,AppUser>();

          CreateMap<MemberRegistrationDto,AppUser>();
          CreateMap<MemberRegistrationDto,Member>();
          CreateMap<MemberUpdateDto,AppUser>();
          CreateMap<MemberUpdateDto,Member>();
                    CreateMap<Member, MemberForReturnDto>()
            .ForMember(m => m.DisplayName, opt => opt.MapFrom(src => src.Identity.DisplayName))
            .ForMember(m => m.Created, opt => opt.MapFrom(src => src.Identity.Created))
            .ForMember(m => m.LastActive, opt => opt.MapFrom(src => src.Identity.LastActive))
            .ForMember(m => m.City, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(m => m.HomeTown, opt => opt.MapFrom(src => src.HomeTown.Prefecture)); 
          CreateMap<Member, MemberForListDto>();
          
          CreateMap<BusinessUserUpdateDto,BusinessUser>();
          CreateMap<BusinessUserUpdateDto,AppUser>();
          CreateMap<BusinessUser, BusinessUserForReturnDto>()
            .ForMember(m => m.DisplayName, opt => opt.MapFrom(src => src.Identity.DisplayName))
            .ForMember(m => m.Created, opt => opt.MapFrom(src => src.Identity.Created))
            .ForMember(m => m.LastActive, opt => opt.MapFrom(src => src.Identity.LastActive))
            .ForMember(m => m.City, opt => opt.MapFrom(src => src.City.Name));

          CreateMap<BlogForUpdateDto, Blog>();

          CreateMap<PhotoForCreationDto, Photo>();
          CreateMap<Photo, PhotoForReturnDto>();
        }
    }
}