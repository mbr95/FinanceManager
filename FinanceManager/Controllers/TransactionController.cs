using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Controllers
{
    public class TransactionController : ControllerBase
    {
        [HttpGet("api/v1/transactions")]
        public IActionResult GetAll()
        {
            return Ok();
        }
    }
}
