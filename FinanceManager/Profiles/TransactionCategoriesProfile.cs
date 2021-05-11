using AutoMapper;
using FinanceManager.Domain.Models;
using FinanceManager.Responses.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
