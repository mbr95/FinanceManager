using System;

namespace FinanceManager.Responses.v1
{
    public class TransactionResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
    }
}
