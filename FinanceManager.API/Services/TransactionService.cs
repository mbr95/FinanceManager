using FinanceManager.API.Data;
using FinanceManager.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManager.API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly DataContext _dataContext;

        public TransactionService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(string userId)
        {
            var transactions = _dataContext.Transactions.AsQueryable();

            var userTransactions = transactions.Where(t => t.UserId == userId);

            return await userTransactions.ToListAsync();
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
            var transactionExists = _dataContext.Transactions.FindAsync(transactionToUpdate.Id).Result;

            if (transactionExists == null)
                return false;

            _dataContext.Entry(transactionExists).CurrentValues.SetValues(transactionToUpdate);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            var transaction = _dataContext.Transactions.FindAsync(id).Result;

            if (transaction == null)
                return false;

            _dataContext.Transactions.Remove(transaction);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> IsTransactionOwnerAsync(int transactionId, string userId)
        {
            var transaction = await _dataContext.Transactions.AsNoTracking().SingleOrDefaultAsync(e => e.Id == transactionId);

            if (transaction == null)
                return false;

            return transaction.UserId == userId;
        }
    }
}
