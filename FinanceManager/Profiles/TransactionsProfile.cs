﻿using AutoMapper;
using FinanceManager.Domain.Models;
using FinanceManager.Requests.v1;
using FinanceManager.Responses.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Profiles
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
