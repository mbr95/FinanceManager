using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Controllers.v1
{
    [Route("api/v{version:apiVersion}/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TransactionController : ControllerBase
    {
        [HttpGet("transactions")]
        public IActionResult GetAll()
        {

            return Ok("data costam");
        }
    }
}
