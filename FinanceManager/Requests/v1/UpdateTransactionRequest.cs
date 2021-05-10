using FinanceManager.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Requests.v1
{
    public class UpdateTransactionRequest
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [TransactionCategory]
        public int CategoryId { get; set; }
    }
}
