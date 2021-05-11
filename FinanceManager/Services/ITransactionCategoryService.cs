using FinanceManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Services
{
    public interface ITransactionCategoryService
    {
        Task<IEnumerable<TransactionCategory>> GetCategoriesAsync();
        Task<TransactionCategory> GetCategoryByIdAsync(int id);
    }
}
