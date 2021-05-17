using Microsoft.AspNetCore.Identity;
using System;

namespace FinanceManager.API.Domain.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }

        public TransactionCategoryId CategoryId { get; set; }
        public TransactionCategory Category { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
