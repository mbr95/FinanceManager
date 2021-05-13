using System;

namespace FinanceManager.Domain.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }

        public TransactionCategoryId CategoryId { get; set; }
        public TransactionCategory Category { get; set; }
    }
}
