using AutoMapper;
using FinanceManager.API.Requests.v1;
using Microsoft.AspNetCore.Identity;

namespace FinanceManager.API.Profiles
{
    public class IdentityUserProfile : Profile
    {
        public IdentityUserProfile()
        {
            CreateMap<RegisterUserRequest, IdentityUser>()
                .ForMember(
                destination => destination.UserName,
                option => option.MapFrom(src => src.UserName));

            CreateMap<RegisterUserRequest, IdentityUser>()
                .ForMember(
                destination => destination.Email,
                option => option.MapFrom(src => src.Email));

            CreateMap<RegisterUserRequest, IdentityUser>()
                .ForMember(
                destination => destination.PasswordHash,
                option => option.MapFrom(src => src.Password));
        }
    }
}
