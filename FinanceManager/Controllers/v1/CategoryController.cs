using AutoMapper;
using FinanceManager.Responses.v1;
using FinanceManager.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Controllers.v1
{
    [Route("api/v{version:apiVersion}/categories/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CategoryController : ControllerBase
    {
        private readonly ITransactionCategoryService _transactionCategoryService;
        private readonly IMapper _mapper;

        public CategoryController(ITransactionCategoryService transactionCategoryService, IMapper mapper)
        {
            _transactionCategoryService = transactionCategoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _transactionCategoryService.GetCategoriesAsync();
            var categoriesResponse = _mapper.Map<IEnumerable<TransactionCategoryResponse>>(categories);

            return Ok(categoriesResponse);
        }

        [HttpGet("{categoryId:int}")]
        public async Task<IActionResult> Get([FromRoute] int categoryId)
        {
            var category = await _transactionCategoryService.GetCategoryByIdAsync(categoryId);

            if (category == null)
                return NotFound();

            var categoryResponse = _mapper.Map<TransactionCategoryResponse>(category);
            return Ok(categoryResponse);
        }
    }
}
