using FinanceManager.Domain.Models;
using FinanceManager.Requests.v1;
using FinanceManager.Responses.v1;
using FinanceManager.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManager.Controllers.v1
{
    [Route("api/v{version:apiVersion}/transactions/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _transactionService.GetTransactionsAsync();
            return Ok(transactions.Select(e => new TransactionResponse{ Id = e.Id, Description = e.Description, Amount = e.Amount, Date = e.Date, CategoryId = (int)e.CategoryId}));
        }

        [HttpGet("{transactionId:int}", Name = "GetTransaction")]
        public async Task<IActionResult> Get(int transactionId)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(transactionId);

            if (transaction == null)
                return NotFound();

            var response = new TransactionResponse { Id = transaction.Id, Description = transaction.Description, Amount = transaction.Amount, Date = transaction.Date, CategoryId = (int)transaction.CategoryId };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionRequest transactionRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var transaction = new Transaction { Description = transactionRequest.Description, Amount = transactionRequest.Amount, Date = transactionRequest.Date, CategoryId = (TransactionCategoryId)transactionRequest.CategoryId };

            var createdTransaction = await _transactionService.CreateTransactionAsync(transaction);

            if (!createdTransaction)
                return BadRequest();
           
            var transactionResponse = new TransactionResponse { Id = transaction.Id, Description = transaction.Description, Amount = transaction.Amount, Date = transaction.Date, CategoryId = (int)transaction.CategoryId };

            return CreatedAtRoute("GetTransaction", new { transactionId = transactionResponse.Id }, transactionResponse);

        }


        [HttpPut("{transactionId:int}")]
        public async Task<IActionResult> Update([FromRoute] int transactionId, [FromBody] UpdateTransactionRequest transactionRequest)
        {
            var transaction = new Transaction()
            {
                Id = transactionId,
                Description = transactionRequest.Description,
                Amount = transactionRequest.Amount,
                Date = transactionRequest.Date,
                CategoryId = (TransactionCategoryId)transactionRequest.CategoryId,
            };

            var updated = await _transactionService.UpdateTransactionAsync(transaction);

            if (!updated)
                return NotFound();

            var transactionResponse = new TransactionResponse { Id = transaction.Id, Description = transaction.Description, Amount = transaction.Amount, Date = transaction.Date, CategoryId = (int)transaction.CategoryId };

            return Ok(transactionResponse);
        }

        [HttpDelete("{transactionId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int transactionId)
        {
            var deleted = await _transactionService.DeleteTransactionAsync(transactionId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
