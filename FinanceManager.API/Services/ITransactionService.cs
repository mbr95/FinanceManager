using FinanceManager.API.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceManager.API.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetTransactionsAsync(string userId);
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task<bool> CreateTransactionAsync(Transaction transaction);
        Task<bool> UpdateTransactionAsync(Transaction transactionToUpdate);
        Task<bool> DeleteTransactionAsync(int id);
        Task<bool> IsTransactionOwnerAsync(int transactionId, string userId);
    }
}
