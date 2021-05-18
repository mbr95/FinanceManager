using AutoMapper;
using FinanceManager.API.Responses.v1;
using FinanceManager.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceManager.API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/categories/")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public async Task<IActionResult> GetAllAsync()
        {
            var categories = await _transactionCategoryService.GetCategoriesAsync();
            var categoriesResponse = _mapper.Map<IEnumerable<TransactionCategoryResponse>>(categories);

            return Ok(categoriesResponse);
        }

        [HttpGet("{categoryId:int}")]
        public async Task<IActionResult> GetAsync([FromRoute] int categoryId)
        {
            var category = await _transactionCategoryService.GetCategoryByIdAsync(categoryId);

            if (category == null)
                return NotFound();

            var categoryResponse = _mapper.Map<TransactionCategoryResponse>(category);
            return Ok(categoryResponse);
        }
    }
}
