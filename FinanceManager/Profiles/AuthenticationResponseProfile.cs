using AutoMapper;
using FinanceManager.Domain.Models;
using FinanceManager.Responses.v1;

namespace FinanceManager.Profiles
{
    public class AuthenticationResponseProfile : Profile
    {
        public AuthenticationResponseProfile()
        {
            CreateMap<AuthenticationResult, AuthenticationSucceededResponse>()
                .ForMember(
                destination => destination.Token,
                option => option.MapFrom(src => src.Token))
                .ForMember(
                destination => destination.RefreshToken,
                option => option.MapFrom(src => src.RefreshToken));

            CreateMap<AuthenticationResult, AuthenticationFailedResponse>()
                .ForMember(
                destination => destination.Errors,
                option => option.MapFrom(src => src.Errors));
        }
    }
}
