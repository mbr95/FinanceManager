using AutoMapper;
using FinanceManager.API.Domain.Models;
using FinanceManager.API.Requests.v1;
using FinanceManager.API.Responses.v1;

namespace FinanceManager.API.Profiles
{
    public class TransactionsProfile : Profile
    {
        public TransactionsProfile()
        {
            CreateMap<Transaction, TransactionResponse>()
                .ForMember(
                destination => destination.CategoryId,
                option => option.MapFrom(src => (int)src.CategoryId));

            CreateMap<CreateTransactionRequest, Transaction>()
                .ForMember(
                destination => destination.CategoryId,
                option => option.MapFrom(src => src.CategoryId));

            CreateMap<UpdateTransactionRequest, Transaction>()
                .ForMember(
                destination => destination.CategoryId,
                option => option.MapFrom(src => src.CategoryId));
        }
    }
}
