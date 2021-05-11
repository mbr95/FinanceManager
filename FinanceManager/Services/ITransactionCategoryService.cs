using FinanceManager.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceManager.Services
{
    public interface ITransactionCategoryService
    {
        Task<IEnumerable<TransactionCategory>> GetCategoriesAsync();
        Task<TransactionCategory> GetCategoryByIdAsync(int id);
    }
}
