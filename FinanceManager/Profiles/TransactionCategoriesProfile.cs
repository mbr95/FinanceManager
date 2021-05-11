using AutoMapper;
using FinanceManager.Domain.Models;
using FinanceManager.Responses.v1;

namespace FinanceManager.Profiles
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
