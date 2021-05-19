using AutoMapper;
using FinanceManager.API.Domain.Models;
using FinanceManager.API.Extensions;
using FinanceManager.API.Requests.v1;
using FinanceManager.API.Responses.v1;
using FinanceManager.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceManager.API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/transactions/")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var transactions = await _transactionService.GetTransactionsAsync();
            var transactionsResponse = _mapper.Map<IEnumerable<TransactionResponse>>(transactions);

            return Ok(transactionsResponse);
        }

        [HttpGet("{transactionId:int}", Name = "GetTransaction")]
        public async Task<IActionResult> GetAsync(int transactionId)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(transactionId);
            if (transaction == null)
                return NotFound();

            var userId = HttpContext.GetUserId();
            var isValidUser = await _transactionService.IsTransactionOwnerAsync(transactionId, userId);
            if (!isValidUser)
                return BadRequest(new { error = "User is not transaction owner." });

            var transactionResponse = _mapper.Map<TransactionResponse>(transaction);
            return Ok(transactionResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTransactionRequest transactionRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var transaction = _mapper.Map<Transaction>(transactionRequest);
            transaction.UserId = HttpContext.GetUserId();

            var createdTransaction = await _transactionService.CreateTransactionAsync(transaction);

            if (!createdTransaction)
                return BadRequest();

            var transactionResponse = _mapper.Map<TransactionResponse>(transaction);

            return CreatedAtRoute("GetTransaction", new { transactionId = transactionResponse.Id }, transactionResponse);
        }

        [HttpPut("{transactionId:int}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int transactionId, [FromBody] UpdateTransactionRequest transactionRequest)
        {
            var userId = HttpContext.GetUserId();
            var isValidUser = await _transactionService.IsTransactionOwnerAsync(transactionId, userId);

            if (!isValidUser)
                return BadRequest(new { error = "User is not transaction owner." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var transaction = _mapper.Map<Transaction>(transactionRequest);
            transaction.Id = transactionId;
            transaction.UserId = userId;

            var updated = await _transactionService.UpdateTransactionAsync(transaction);

            if (!updated)
                return NotFound();

            var transactionResponse = _mapper.Map<TransactionResponse>(transaction);

            return Ok(transactionResponse);
        }

        [HttpDelete("{transactionId:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int transactionId)
        {
            var userId = HttpContext.GetUserId();
            var isValidUser = await _transactionService.IsTransactionOwnerAsync(transactionId, userId);

            if (!isValidUser)
                return BadRequest(new { error = "User is not transaction owner." });

            var deleted = await _transactionService.DeleteTransactionAsync(transactionId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
