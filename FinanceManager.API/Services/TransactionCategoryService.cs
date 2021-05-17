using FinanceManager.API.Data;
using FinanceManager.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceManager.API.Services
{
    public class TransactionCategoryService : ITransactionCategoryService
    {
        private readonly DataContext _dataContext;

        public TransactionCategoryService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<TransactionCategory>> GetCategoriesAsync()
        {
            return await _dataContext.TransactionCategories.ToListAsync();
        }

        public async Task<TransactionCategory> GetCategoryByIdAsync(int id)
        {
            return await _dataContext.TransactionCategories.SingleOrDefaultAsync(category => (int)category.Id == id);
        }
    }
}
