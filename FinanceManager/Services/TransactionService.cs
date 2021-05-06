using FinanceManager.Data;
using FinanceManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceManager.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly DataContext _dataContext;

        public TransactionService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync()
        {
            return await _dataContext.Transactions.ToListAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _dataContext.Transactions.SingleOrDefaultAsync(transaction => transaction.Id == id);
        }

        public async Task<bool> CreateTransactionAsync(Transaction transaction)
        {
            await _dataContext.Transactions.AddAsync(transaction);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> UpdateTransactionAsync(Transaction transactionToUpdate)
        {
            _dataContext.Transactions.Update(transactionToUpdate);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            var transaction = await GetTransactionByIdAsync(id);
            _dataContext.Transactions.Remove(transaction);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }
    }
}
