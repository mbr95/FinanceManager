using System.Collections.Generic;

namespace FinanceManager.API.Domain.Models
{
    public class TransactionCategory
    {
        public TransactionCategoryId Id { get; set; }
        public string Name { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}