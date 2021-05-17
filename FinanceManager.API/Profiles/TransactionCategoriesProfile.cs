using AutoMapper;
using FinanceManager.API.Domain.Models;
using FinanceManager.API.Responses.v1;

namespace FinanceManager.API.Profiles
{
    public class TransactionCategoriesProfile : Profile
    {
        public TransactionCategoriesProfile()
        {
            CreateMap<TransactionCategory, TransactionCategoryResponse>()
                .ForMember(
                destination => destination.Id,
                option => option.MapFrom(src => (int)src.Id));
        }
    }
}
