using FinanceManager.Data;
using FinanceManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Services
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
