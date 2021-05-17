using System;
using System.ComponentModel.DataAnnotations;
using FinanceManager.API.Validation;

namespace FinanceManager.API.Requests.v1
{
    public class CreateTransactionRequest
    {
        [Required]
        [MaxLength(25)]
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
